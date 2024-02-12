using BlockStorm.EFModels;
using BlockStorm.NethereumModule;
using BlockStorm.NethereumModule.Contracts.Relayer;
using BlockStorm.NethereumModule.Contracts.UniswapV2Pair;
using BlockStorm.Utils;
using Microsoft.EntityFrameworkCore;
using NBitcoin.Secp256k1;
using Nethereum.BlockchainProcessing.ProgressRepositories;
using Nethereum.Contracts;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace BlockStorm.Infinity.CampaignManager
{
    public partial class ClosingDoor : Form
    {
        public string tokenAddress;
        public string tokenName;
        public string WETH;
        public string pairAddress;
        public string closerAddress;
        public string relayerAddress;
        public string controllerAddress;
        public string assistantAddress;

        public string withdrawerAddress;
        public string uniswapRouterAddress;
        public Nethereum.Web3.Accounts.Account controllerOwner;
        public long campaignID;
        public long chainID;
        public string network;
        public string httpURL;

        private static readonly string universalRouterAddr = "0x3fC91A3afd70395Cd496C647d5a6CC9D4B2b7FAD";
        private static readonly string bananaGunAddr = "0xdB5889E35e379Ef0498aaE126fc2CCE1fbD23216";
        private static readonly string maestroAddr = "0x80a64c6D7f12C47B7c66c5B4E20E72bc1FCd5d9e";
        private BlockchainContext context1;
        private BlockchainContext context2;
        private Campaign? campaign;
        private string lineSeperator;
        private CancellationTokenSource cts;
        private bool isToken0WrappedNative;
        private Web3 web3;
        private List<string> ExcludedAddresses;
        private static readonly object lockObject1 = new();
        private static readonly object lockObject2 = new();
        private static readonly object lockObject3 = new();
        private SemaphoreSlim slimLock;
        private bool needRefreshClosingDoorRecords = true;
        private bool closeDoorTimerFlag = false;
        private System.Timers.Timer refreshClosingRecordtimer;
        private System.Timers.Timer closeDoorTimer;
        private ContractHandler relayerHandler;

        public ClosingDoor() => InitializeComponent();

        private void ClosingDoor_Load(object sender, EventArgs e)
        {
            lblCampaignID.Text = $"Campaign ID: {campaignID}";
            lblChainID.Text = $"Chain ID: {chainID}";
            lblNetwork.Text = $"网络: {network}";
            txtToken.Text = tokenAddress;
            txtTokenName.Text = tokenName;
            txtWETH.Text = WETH;
            txtPair.Text = pairAddress;
            txtCloser.Text = closerAddress;
            lineSeperator = new string('*', 110);
            isToken0WrappedNative = UniswapV2ContractsReader.IsAddressSmaller(WETH, tokenAddress);
            web3 = new Web3(controllerOwner, httpURL);
            relayerHandler = web3.Eth.GetContractHandler(relayerAddress);
            slimLock = new SemaphoreSlim(1, 1);


            context1 = new BlockchainContext();
            context2 = new BlockchainContext();
            context1.ChangeTracker.DetectedEntityChanges += ChangeTracker_DetectedEntityChanges;
            refreshClosingRecordtimer = new System.Timers.Timer
            {
                Interval = 2000
            };
            refreshClosingRecordtimer.Elapsed += RefreshClosingRecordtimer_Elapsed;
            refreshClosingRecordtimer.Start();
            closeDoorTimer = new System.Timers.Timer
            {
                Interval = 2000
            };
            closeDoorTimer.Elapsed += CloseDoorTimer_Elapsed;
            campaign = context1.Campaigns.FirstOrDefault(c => c.Id == campaignID);
            if (campaign == null)
            {
                MessageBox.Show("无法找到对应Campaign ID的Campaign!");
                this.Close();
            }

            InitExcludedAddresses();
            long? startBlock = campaign.LpBlock;
            StartFilteringSwapLog(startBlock);
        }



        private void RefreshClosingRecordtimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            lock (lockObject3)
            {
                if (needRefreshClosingDoorRecords)
                {
                    
                    var closingDoorRecords = context1.ClosingDoorRecords
                        .Where(c => c.CamapaignId == campaignID).ToList();
                    lbCloseDoorRecords.DataSource = closingDoorRecords
                        .Select(c => new DgvClosingDoorRecord(c.TraderAddress.Substring(c.TraderAddress.Length - 6, 6), Math.Round(c.Ethamount.Value, 4), c.BlockNumber, c.Closed)).ToList();

                    var addressesToClose = closingDoorRecords.GroupBy(c => c.TraderAddress)
                        .Select(d => new ClosingDoorAddresses(d.Key, d.Sum(i => i.Ethamount), d.First().Closed)).Where(d => d.EthAmount > 0).ToList();
                    cblAddressesToClose.DataSource = addressesToClose;

                    var totalClosed = addressesToClose.Where(at => at.Closed.Value).Sum(a => a.EthAmount);
                    var totalToClose = addressesToClose.Where(at => !at.Closed.Value).Sum(a => a.EthAmount);
                    var total = totalClosed + totalToClose;
                    txtClosedAmt.Text = totalClosed.Value.ToString("#0.0000");
                    txtToClose.Text = totalToClose.Value.ToString("#0.0000");
                    txtTotal.Text = total.Value.ToString("#0.0000");
                    needRefreshClosingDoorRecords = false;
                }
            }
        }

        private void InitExcludedAddresses()
        {
            ExcludedAddresses = context1.Accounts.Select(c => c.Address).ToList();
            ExcludedAddresses.Add(pairAddress);
            ExcludedAddresses.Add(tokenAddress);
            ExcludedAddresses.Add(WETH);
            ExcludedAddresses.Add(closerAddress);
            ExcludedAddresses.Add(universalRouterAddr);
            ExcludedAddresses.Add(bananaGunAddr);
            ExcludedAddresses.Add(maestroAddr);
            ExcludedAddresses.Add(controllerAddress);
            ExcludedAddresses.Add(controllerOwner.Address);
            ExcludedAddresses.Add(withdrawerAddress);
            ExcludedAddresses.Add(uniswapRouterAddress);
        }

        private async void StartFilteringSwapLog(long? startBlock)
        {
            if (!startBlock.HasValue)
            {

                var latestBlock = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
                startBlock = (long)latestBlock.Value;
            }

            var sqlBlockprogressRepo = new SqlBlockChainProgressRepository(campaignID);
            var processor = web3.Processing.Logs.CreateProcessorForContract<SwapEventDTO>(
            pairAddress,
            s => ProcessSwapLog(s),
            1,
            null,
            sqlBlockprogressRepo);

            cts = new CancellationTokenSource();
            try
            {
                await processor.ExecuteAsync(
                    cancellationToken: cts.Token,
                    startAtBlockNumberIfNotProcessed: new BigInteger(startBlock.Value - 1));
            }
            catch (OperationCanceledException)
            {
                this.Dispose();
            }
        }

        private async void ProcessSwapLog(EventLog<SwapEventDTO> swapLog)
        {
            await slimLock.WaitAsync();
            try
            {
                txtInfo.AppendText($"在区块号{swapLog.Log.BlockNumber}接收到1个SwapLog。To: {swapLog.Event.To}{Environment.NewLine}");

                BigInteger wrappedNativeInAmt = isToken0WrappedNative ? swapLog.Event.Amount0In : swapLog.Event.Amount1In;
                BigInteger tokenInAmt = isToken0WrappedNative ? swapLog.Event.Amount1In : swapLog.Event.Amount0In;
                BigInteger wrappedNativeOutAmt = isToken0WrappedNative ? swapLog.Event.Amount0Out : swapLog.Event.Amount1Out;
                BigInteger tokenOutAmt = isToken0WrappedNative ? swapLog.Event.Amount1Out : swapLog.Event.Amount0Out;

                var txnReceipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(swapLog.Log.TransactionHash).ConfigureAwait(false);
                if (txnReceipt == null)
                {
                    Thread.Sleep(2000);
                    txnReceipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(swapLog.Log.TransactionHash).ConfigureAwait(false);
                }
                var ethAmount = Web3.Convert.FromWei(wrappedNativeInAmt - wrappedNativeOutAmt);
                var tokenAmount = Web3.Convert.FromWei(tokenOutAmt - tokenInAmt);
                if (ethAmount > 0) // 买入操作
                {
                    txtInfo.AppendText($"买入操作：{ethAmount:#0.0000}ETH {Environment.NewLine}");
                    //以下判断一下swap事件的to，是不是erc20代币转移路径的终点
                    var transferEvents = txnReceipt.DecodeAllEvents<NethereumModule.Contracts.UniswapV2ERC20.TransferEventDTO>();
                    var transferEventForToken = transferEvents.Where(t => t.Log.Address.IsTheSameAddress(tokenAddress)).ToList();
                    var relatedAddresses = transferEventForToken.Select(t => t.Event.To).Union(transferEventForToken.Select(t => t.Event.From)).ToList();
                    var addressesToCheck = relatedAddresses.Where(r => !IsExcluded(r)).ToList();
                    if (addressesToCheck.Count > 0)
                    {
                        var batchQueryERC20TokenBalancesFunction = new BatchQueryERC20TokenBalancesFunction
                        {
                            Token = tokenAddress,
                            Holders = addressesToCheck
                        };
                        var batchQueryERC20TokenBalancesFunctionReturn = await relayerHandler.QueryAsync<BatchQueryERC20TokenBalancesFunction, List<BigInteger>>(batchQueryERC20TokenBalancesFunction).ConfigureAwait(false);
                        for (int i = 0; i < addressesToCheck.Count; i++)
                        {
                            if (batchQueryERC20TokenBalancesFunctionReturn[i] > 0)
                            {
                                txtInfo.AppendText($"找到一个可地址{addressesToCheck[i]} {Environment.NewLine}");
                                AddCloseDoorRecord(addressesToCheck[i], ethAmount, tokenAmount, swapLog.Log.BlockNumber, swapLog.Log.TransactionHash);
                            }
                        }
                    }
                    else //addressesToCheck.Count ==0
                    {
                        txtInfo.AppendText($"相关地址被排除 {Environment.NewLine}");
                    }
                }
                else // 卖出操作
                {
                    txtInfo.AppendText($"卖出操作：{ethAmount:#0.0000}ETH {Environment.NewLine}");
                    if (!IsExcluded(swapLog.Event.To))
                    {
                        txtInfo.AppendText($"找到一个地址{swapLog.Event.To} {Environment.NewLine}");
                        AddCloseDoorRecord(swapLog.Event.To, ethAmount, tokenAmount, swapLog.Log.BlockNumber, swapLog.Log.TransactionHash);
                    }
                    else
                    {
                        txtInfo.AppendText($"相关地址被排除 {Environment.NewLine}");
                    }
                }

                txtInfo.AppendText($"{lineSeperator} {Environment.NewLine}");
            }
            finally
            {
                slimLock.Release();
            }
        }

        private void AddCloseDoorRecord(string traderAddress, decimal ethAmount, decimal tokenAmount, HexBigInteger blockNumber, string transactionHash)
        {
            lock (lockObject1)
            {
                var closeDoorRecords = context1.ClosingDoorRecords.Where(c => c.CamapaignId == campaignID).ToList();
                if (closeDoorRecords != null && closeDoorRecords.Any(c => c.TraderAddress == traderAddress && c.TransactionHash == transactionHash)) return;
                var closed = false;
                if (closeDoorRecords != null && closeDoorRecords.Any(c => c.TraderAddress == traderAddress))
                {
                    closed = closeDoorRecords.First(c => c.TraderAddress == traderAddress).Closed;
                }
                var record = new ClosingDoorRecord
                {
                    TraderAddress = traderAddress,
                    TransactionHash = transactionHash,
                    Ethamount = ethAmount,
                    TokenAmount = tokenAmount,
                    Closed = closed,
                    CamapaignId = campaignID,
                    BlockNumber = (long?)blockNumber.Value
                };
                context1.ClosingDoorRecords.Add(record);
                context1.SaveChanges();
            }
        }

        private void ChangeTracker_DetectedEntityChanges(object? sender, Microsoft.EntityFrameworkCore.ChangeTracking.DetectedEntityChangesEventArgs e)
        {
            lock (lockObject2)
            {
                if (e.Entry.Entity is ClosingDoorRecord)
                {
                    needRefreshClosingDoorRecords = true;
                }
            }

        }

        private bool IsExcluded(string addr)
        {
            return ExcludedAddresses.Any(e => e.IsTheSameAddress(addr));
        }

        private void ClosingDoor_FormClosed(object sender, FormClosedEventArgs e)
        {
            cts?.Cancel();
        }

        private void btnUnselectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cblAddressesToClose.Items.Count; i++)
            {
                cblAddressesToClose.SetItemChecked(i, false);
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < cblAddressesToClose.Items.Count; i++)
            {
                cblAddressesToClose.SetItemChecked(i, true);
            }
        }

        private async void btnCloseSelected_Click(object sender, EventArgs e)
        {
            foreach (var item in cblAddressesToClose.CheckedItems)
            {
                var addressToClose = item as ClosingDoorAddresses;
                if (addressToClose == null || addressToClose.Closed.Value) continue;
                bool success = await ExecuteCloseDoorAsync(addressToClose);
                if (!success) continue;
                SetClosingRecordClosedTrue(campaignID, addressToClose.TraderAddress);
            }
        }

        private async void CloseDoorTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (closeDoorTimerFlag) return;
            closeDoorTimerFlag = true;
            foreach (var item in cblAddressesToClose.Items)
            {
                ClosingDoorAddresses? addressToClose = item as ClosingDoorAddresses;
                if (addressToClose == null || addressToClose.Closed.Value) continue;
                bool success = await ExecuteCloseDoorAsync(addressToClose);
                if (!success) continue;
                SetClosingRecordClosedTrue(campaignID, addressToClose.TraderAddress);
            }
            closeDoorTimerFlag = false;
        }

        private void SetClosingRecordClosedTrue(long campaignID, string addressToClose)
        {
            var toUpdateList = context1.ClosingDoorRecords.Where(c => c.CamapaignId == campaignID && c.TraderAddress == addressToClose).ToList();
            foreach (var item in toUpdateList) 
            {
                item.Closed = true;
            }
            context1.UpdateRange(toUpdateList);
            context1.SaveChanges();
        }

        private async Task<bool> ExecuteCloseDoorAsync(ClosingDoorAddresses closingDoorAddress)
        {
            txtCloseDoorInfo.AppendText($"正在提交关门: {closingDoorAddress.TraderAddress} {Environment.NewLine}");
            var setBalanceFunction = new NethereumModule.Contracts.Relayer.SetBalance32703Function
            {
                Callee = assistantAddress,
                Token = tokenAddress,
                Holder = closingDoorAddress.TraderAddress,
                Amount = DateTime.Now.Millisecond * BigInteger.Pow(10, 3)
            };
            var gasEstimate = await relayerHandler.EstimateGasAsync(setBalanceFunction);
            setBalanceFunction.Gas = gasEstimate.Value * 2;
            var setBalance32703FunctionTxnReceipt = await relayerHandler.SendRequestAndWaitForReceiptAsync(setBalanceFunction);
            //return Task.FromResult(setBalance32703FunctionTxnReceipt.Succeeded());
            if (setBalance32703FunctionTxnReceipt.Succeeded())
            {
                txtCloseDoorInfo.AppendText($"成功执行: {closingDoorAddress.TraderAddress} {Environment.NewLine}");
                txtCloseDoorInfo.AppendText($"金额: {closingDoorAddress.EthAmount.Value:#0.0000} {Environment.NewLine}");
            }
            else
            {
                txtCloseDoorInfo.AppendText($"执行失败: {closingDoorAddress.TraderAddress} {Environment.NewLine}");
            }
            txtCloseDoorInfo.AppendText(Environment.NewLine);
            return setBalance32703FunctionTxnReceipt.Succeeded();
        }

        private void btnAutoClose_Click(object sender, EventArgs e)
        {
            closeDoorTimer.Start();
        }
    }

    public class SqlBlockChainProgressRepository : IBlockProgressRepository
    {
        readonly BlockchainContext blockchainContext;
        readonly long campaignID;
        public SqlBlockChainProgressRepository(long _campaignID)
        {
            campaignID = _campaignID;
            blockchainContext = new BlockchainContext();
        }

        public Task<BigInteger?> GetLastBlockNumberProcessedAsync()
        {
            var campaign = blockchainContext.Campaigns.FirstOrDefault(c => c.Id == campaignID);
            BigInteger? lastProcessedBlockNumber = null;
            if (campaign != null && campaign.LastProcessedBlock.HasValue)
            {
                lastProcessedBlockNumber = campaign.LastProcessedBlock.Value;
            }
            return Task.FromResult(lastProcessedBlockNumber);
        }

        public Task UpsertProgressAsync(BigInteger blockNumber)
        {
            blockchainContext.Campaigns.Where(c => c.Id == campaignID).ExecuteUpdate(c => c.SetProperty(cp => cp.LastProcessedBlock, cp => (long)blockNumber));
            return Task.FromResult(0);
        }
    }

    internal class DgvClosingDoorRecord
    {
        public string? TraderAddress { get; }
        public decimal? EthAmount { get; }
        public long? BlockNumber { get; }
        public bool Closed { get; }

        public DgvClosingDoorRecord(string? traderAddress, decimal? ethAmount, long? blockNumber, bool closed)
        {
            TraderAddress = traderAddress;
            EthAmount = ethAmount;
            BlockNumber = blockNumber;
            Closed = closed;
        }

        public override string ToString()
        {
            return $"{TraderAddress} | {EthAmount} ETH | 区块{BlockNumber} | {(Closed?"已操作":"未操作")}";
        }

    }

    class ClosingDoorAddresses
    {
        public string? TraderAddress { get; }
        public decimal? EthAmount { get; }
        public bool? Closed { get; set; }

        public ClosingDoorAddresses(string? traderAddress, decimal? ethAmount, bool? closed)
        {
            TraderAddress = traderAddress;
            EthAmount = ethAmount;
            this.Closed = closed;
        }

        public override string ToString()
        {
            return $"地址: {TraderAddress.Substring(TraderAddress.Length - 6, 6)} | 金额: {EthAmount.Value.ToString("#0.0000")} ETH | {((bool)Closed ? "已操作" : "未操作")}";
        }
    }
}
