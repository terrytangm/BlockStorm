using BlockStorm.EFModels;
using BlockStorm.NethereumModule;
using BlockStorm.NethereumModule.Contracts.Controller;
using BlockStorm.NethereumModule.Contracts.LoopArbitrage;
using BlockStorm.NethereumModule.Contracts.PinkLock02;
using BlockStorm.NethereumModule.Contracts.Relayer;
using BlockStorm.NethereumModule.Contracts.UniswapV2Pair;
using BlockStorm.NethereumModule.Contracts.UniswapV2Router02;
using BlockStorm.NethereumModule.Contracts.WETH;
using BlockStorm.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts.Standards.ENS.ETHRegistrarController.ContractDefinition;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Fee1559Suggestions;
using Nethereum.RPC.TransactionManagers;
using Nethereum.Util;
using Nethereum.Web3;
using System.Data;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using System.Timers;
using static Nethereum.Util.UnitConversion;
using Web3Accounts = Nethereum.Web3.Accounts;

namespace BlockStorm.Infinity.CampaignManager
{
    public partial class CampaignManager : Form
    {
        public DepolyContractForm? deployContractFrom;
        private long? chainID;
        private string? chainName;
        private string? httpURL;
        private string? webSocketURL;
        private string wrappedNativeAddr;
        private string tokenAddr;
        private string pairAddr;
        private string controllerAddr;
        private string RelayerAddr;
        private string routerAddr;
        private string pinkLock02Addr;
        private readonly string trasnferEventHash = "0xddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef";

        private Token? token = null;
        private Campaign? campaign = null;
        private BlockchainContext context;
        private Web3Accounts.Account controllerOwner;
        private EFModels.Account contractDeployer;
        private EFModels.Account doorCloser;
        private EFModels.Account withdrawer;
        ContractHandler controllerContractHandlerForOwner = null;
        ContractHandler pairContractHandlerForDeployer = null;
        ContractHandler relayerContractHandlerForOwner = null;
        private Web3 web3ForControllerOwner;
        private Web3 web3ForContractDeployer;
        private UniswapV2ContractsReader uniswapV2Reader;
        private Dictionary<string, int> dgColumnIndex;
        public ObservableDictionary<string, decimal> ethBalances = new(new Dictionary<string, decimal>());
        public ObservableDictionary<string, decimal> wethBalances = new(new Dictionary<string, decimal>());
        public ObservableDictionary<string, (decimal, decimal)> pairReserves = new(new Dictionary<string, (decimal, decimal)>());

        private List<string>? sendNativeAddressList = null;
        private List<BigInteger>? sendNativeAmounts = null;
        private List<string>? WETH2ETHAddressList = null;
        private List<BigInteger>? WETH2ETHAmounts = null;
        private List<EFModels.Account> traderAccounts;

        private List<string>? sendWrappedNativeAddressList = null;
        private List<BigInteger>? sendWrappedNativeAmounts = null;

        private List<TradeTask>? tradeTaskList = null;
        private System.Timers.Timer tradeTaskTimer;
        private System.Timers.Timer updateGasTimer;
        private int currentTaskIndex;
        private HexBigInteger gasPrice;

        private static bool tradeTaksLockFlag = false;
        private static bool balanceModifyCheckPassed = false;
        private List<DgvRowObject> campaignAccountRowObjects;


        public CampaignManager()
        {
            InitializeComponent();
        }

        private void BtnDeployContract_Click(object sender, EventArgs e)
        {
            if (deployContractFrom != null)
            {
                deployContractFrom.Close();
                deployContractFrom.Dispose();
            }
            deployContractFrom = new DepolyContractForm
            {
                StartPosition = FormStartPosition.CenterScreen,
                ShowInTaskbar = false,
                chainID = chainID,
                chainName = chainName,
                httpURL = httpURL,
                controllerAddr = controllerAddr,
                wrappedNativeAddr = wrappedNativeAddr,
                controllerOwnerAccount = controllerOwner,
                gasPrice = gasPrice
            };
            deployContractFrom.Show();
        }

        private async void CampaignManager_LoadAsync(object sender, EventArgs e)
        {
            httpURL = Config.ConfigInfo(null, ChainConfigPart.HttpURL);
            webSocketURL = Config.ConfigInfo(null, ChainConfigPart.WebsocketURL);
            chainID = System.Convert.ToInt64(Config.ConfigInfo(null, ChainConfigPart.ChainID));
            chainName = Config.GetValueByKey("TargetChainConfig");
            controllerAddr = Config.GetControllerAddress(chainID.ToString());
            RelayerAddr = Config.GetRelayerAddress(chainID.ToString());
            wrappedNativeAddr = Config.GetWrappedNativeAddress(chainID.ToString());
            string? controllerOwnerPK = Config.GetControllerOwnerPK(chainID.ToString());
            routerAddr = Config.GetUniswapV2RouterAddress(chainID.ToString());
            pinkLock02Addr = Config.GetPinkLock02Address(chainID.ToString());
            controllerOwner = new Web3Accounts.Account(controllerOwnerPK);
            uniswapV2Reader = new UniswapV2ContractsReader(httpURL);
            context = new BlockchainContext();
            web3ForControllerOwner = new Web3(controllerOwner, httpURL);
            controllerContractHandlerForOwner = web3ForControllerOwner.Eth.GetContractHandler(controllerAddr);
            relayerContractHandlerForOwner = web3ForControllerOwner.Eth.GetContractHandler(RelayerAddr);
            ethBalances.OnValueChanged += EthBalances_OnBalanceChanged;
            wethBalances.OnValueChanged += WethBalances_OnBalanceChanged;
            pairReserves.OnValueChanged += PairReserves_OnValueChanged;
            await UpdateControllerBalances();
            await UpdateControllerOwnerBalances();
            lblControllerAddress.Text = controllerAddr;
            lblControllerOwnerAddress.Text = controllerOwner.Address;
            lblRelayerAddress.Text = RelayerAddr;
            lblChainName.Text = chainName;
            Control.CheckForIllegalCrossThreadCalls = false;
            tradeTaskTimer = new System.Timers.Timer
            {
                Interval = 1000
            };
            tradeTaskTimer.Elapsed += TradeTaskTimer_ElapsedAsync;
            updateGasTimer = new System.Timers.Timer
            {
                Interval = 1000,
            };
            updateGasTimer.Elapsed += UpdateGasTimer_Elapsed;
            
            updateGasTimer.Start();
        }

        private async void UpdateGasTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            updateGasTimer.Interval = 1000 * 30;
            try
            {
                gasPrice = await web3ForControllerOwner.Eth.GasPrice.SendRequestAsync();
                lblGasPrice.Text = $"Gas Price: {Web3.Convert.FromWei(gasPrice, EthUnit.Gwei)} GWei - {DateTime.Now.ToString("HH:mm:ss")}";
            }
            catch (Exception ex)
            {
                lblGasPrice.Text = ex.Message;
            }

        }

        private void PairReserves_OnValueChanged(object? sender, ValueChangedEventArgs<string, (decimal, decimal)> e)
        {
            if (!e.Key.IsTheSameAddress(pairAddr)) return;
            txtPairReserveWETH.Text = e.Value.Item1.ToString("#0.0000");
            txtPairReserveToken.Text = e.Value.Item2.ToString("#0.0000");
            if (txtInitPrice.Text.IsNullOrEmpty())
            {
                if (e.Value.Item2 != 0)
                    txtInitPrice.Text = (e.Value.Item1 / e.Value.Item2).ToString();
            }
            else
            {
                decimal currentPrice = 0;
                if (e.Value.Item2 != 0)
                    currentPrice = e.Value.Item1 / e.Value.Item2;
                decimal initPrice = decimal.Parse(txtInitPrice.Text);
                txtCurrentPrice.Text = currentPrice.ToString("");
                decimal changeRatio = 0;
                if (initPrice != 0)
                    changeRatio = currentPrice / initPrice - 1;
                if (changeRatio > 0)
                {
                    lblPriceUpPercentage.ForeColor = Color.Red;
                    lblPriceUpPercentage.Text = "↑" + changeRatio.ToString("P");
                }
                else if (changeRatio < 0)
                {
                    lblPriceUpPercentage.ForeColor = Color.Green;
                    lblPriceUpPercentage.Text = "↓" + changeRatio.ToString("P");
                }
                else
                {
                    lblPriceUpPercentage.Text = string.Empty;
                }

            }
        }

        private async void TradeTaskTimer_ElapsedAsync(object? sender, ElapsedEventArgs e)
        {
            if (tradeTaksLockFlag) return;
            tradeTaksLockFlag = true;
            if (tradeTaskList.IsNullOrEmpty() || currentTaskIndex < 0 || currentTaskIndex >= tradeTaskList.Count)
            {
                tradeTaskTimer?.Stop();
                tradeTaksLockFlag = false;
                return;
            }
            tradeTaskList[currentTaskIndex].TradeInterval--;
            UpdateTaskGridViewInfo();
            if (tradeTaskList[currentTaskIndex].TradeInterval <= 0 && tradeTaskList[currentTaskIndex].TradeInterval >= -3)
            {
                var cancelTokenSource = new CancellationTokenSource();
                cancelTokenSource.CancelAfter(70 * 1000);
                try
                {
                    (bool success, BigInteger amount) = await tradeTaskList[currentTaskIndex].ExcuteTaskAsync(cancelTokenSource.Token, gasPrice);
                    if (success)
                    {
                        var campaignAccount = context.CampaignAccounts.Where(c => c.AccountId == tradeTaskList[currentTaskIndex].Trader.Id && c.CampaignId == campaign.Id).FirstOrDefault();
                        if (tradeTaskList[currentTaskIndex].TaskType == TradeTaskType.buy)
                        {

                            campaignAccount.BoughtTimes += 1;
                            BigInteger boughtAmount = campaignAccount.BoughtVolumn.IsNullOrEmpty() ? 0 : BigInteger.Parse(campaignAccount.BoughtVolumn);
                            boughtAmount += amount;
                            campaignAccount.BoughtVolumn = boughtAmount.ToString();
                            campaignAccount.TradeTimes += 1;
                            BigInteger tradeAmount = campaignAccount.TradeVolumn.IsNullOrEmpty() ? 0 : BigInteger.Parse(campaignAccount.TradeVolumn);
                            tradeAmount += amount;
                            campaignAccount.TradeVolumn = tradeAmount.ToString();
                        }
                        else
                        {
                            campaignAccount.SoldTimes += 1;
                            BigInteger soldAmount = campaignAccount.SoldVolumn.IsNullOrEmpty() ? 0 : BigInteger.Parse(campaignAccount.SoldVolumn);
                            soldAmount += amount;
                            campaignAccount.SoldVolumn = soldAmount.ToString();
                            campaignAccount.TradeTimes += 1;
                            BigInteger tradeAmount = campaignAccount.TradeVolumn.IsNullOrEmpty() ? 0 : BigInteger.Parse(campaignAccount.TradeVolumn);
                            tradeAmount += amount;
                            campaignAccount.TradeVolumn = tradeAmount.ToString();
                        }
                        context.CampaignAccounts.Update(campaignAccount);
                        context.SaveChanges();
                        UpdateGridViewRow(campaignAccount);
                        await RefreshTradersBalances();
                        await UpdateWETHBalance(pairAddr);
                        await UpdatePairReserves();
                        currentTaskIndex++;
                        RefreshTradeTaskList();
                    }
                }
                catch (OperationCanceledException ex) when (ex.CancellationToken == cancelTokenSource.Token)
                {

                }
                catch (RpcClientTimeoutException ex)
                {

                }
                catch (RpcClientUnknownException ex)
                {

                }
                catch (RpcResponseException ex)
                {

                }
                catch (SmartContractRevertException ex)
                {
                }
                finally
                {
                    cancelTokenSource.Dispose();
                }
            }
            else if (tradeTaskList[currentTaskIndex].TradeInterval < -3)
            {
                UpdateGridViewRowSkipped(tradeTaskList[currentTaskIndex].Trader.Id);
                currentTaskIndex++;
                RefreshTradeTaskList();
            }
            tradeTaksLockFlag = false;
        }



        private void WethBalances_OnBalanceChanged(object? sender, ValueChangedEventArgs<string, decimal> e)
        {
            if (e.Key.Equals(controllerAddr))
            {
                txtControllerWETHBalance.Text = e.Value.ToString("#0.0000");
            }
            if (e.Key.Equals(controllerOwner.Address))
            {
                txtControllerOwnerBalanceWETH.Text = e.Value.ToString("#0.0000");
            }
            if (e.Key.Equals(pairAddr))
            {
                txtPairBalanceWETH.Text = e.Value.ToString("#0.0000");
            }
            if (contractDeployer != null && e.Key.Equals(contractDeployer.Address))
            {
                txtDeployerBalanceWETH.Text = e.Value.ToString("#0.0000");
            }
            if (!traderAccounts.IsNullOrEmpty() && traderAccounts.Any(a => e.Key.Equals(a.Address)))
            {
                decimal tradersTotalWeth = wethBalances.Dict.Where(kvp => traderAccounts.Any(a => kvp.Key.Equals(a.Address))).Sum(x => x.Value);
                txtTradersBalanceWETH.Text = tradersTotalWeth.ToString("#0.0000");
                var row = dgvTraderList.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[dgColumnIndex["地址"]].Value.ToString().Equals(e.Key)).FirstOrDefault();
                if (row != null)
                {
                    row.Cells[dgColumnIndex["WETH余额"]].Value = e.Value.ToString("#0.0000");
                }
            }
            decimal totalWeth = wethBalances.Dict.Sum(x => x.Value);
            txtTotalWETH.Text = totalWeth.ToString("#0.0000");
            decimal totalWethAndEth = totalWeth + ethBalances.Dict.Sum(x => x.Value);
            txtWETHANDETH.Text = totalWethAndEth.ToString("#0.0000");
        }

        private void EthBalances_OnBalanceChanged(object? sender, ValueChangedEventArgs<string, decimal> e)
        {
            if (e.Key.Equals(controllerAddr))
            {
                txtControllerETHBalance.Text = e.Value.ToString("#0.0000");
            }
            if (e.Key.Equals(controllerOwner.Address))
            {
                txtControllerOwnerBalanceETH.Text = e.Value.ToString("#0.0000");
            }
            if (contractDeployer != null && e.Key.Equals(contractDeployer.Address))
            {
                txtDeployerBalanceETH.Text = e.Value.ToString("#0.0000");
            }
            if (doorCloser != null && e.Key.Equals(doorCloser.Address))
            {
                txtDoorCloserBalanceETH.Text = e.Value.ToString("#0.0000");
            }
            if (withdrawer != null && e.Key.Equals(withdrawer.Address))
            {
                txtWithdrawerBalanceETH.Text = e.Value.ToString("#0.0000");
            }
            if (!traderAccounts.IsNullOrEmpty() && traderAccounts.Any(a => e.Key.Equals(a.Address)))
            {
                decimal tradersTotalEth = ethBalances.Dict.Where(kvp => traderAccounts.Any(a => kvp.Key.Equals(a.Address))).Sum(x => x.Value);
                txtTradersBalanceETH.Text = tradersTotalEth.ToString("#0.0000");
                var row = dgvTraderList.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[dgColumnIndex["地址"]].Value.ToString().Equals(e.Key)).FirstOrDefault();
                if (row != null)
                {
                    row.Cells[dgColumnIndex["ETH余额"]].Value = e.Value.ToString("#0.0000");
                }
            }
            decimal totalEth = ethBalances.Dict.Sum(x => x.Value);
            txtTotalBalanceETH.Text = totalEth.ToString("#0.0000");
            decimal totalWethAndEth = wethBalances.Dict.Sum(x => x.Value) + totalEth;
            txtWETHANDETH.Text = totalWethAndEth.ToString("#0.0000");
        }

        private async void BtnLoadTokenContract_Click(object sender, EventArgs e)
        {
            token = context.Tokens.Where(t => t.ChainId == chainID && t.DeployerID != null && t.CampaignID == null).FirstOrDefault();
            if (token == null)
            {
                MessageBox.Show("没有可供加载的Token合约！");
                return;
            }
            contractDeployer = context.Accounts.Where(a => a.Id == token.DeployerID).FirstOrDefault();
            if (contractDeployer == null)
            {
                MessageBox.Show("找不到部署者的信息！");
                return;
            }
            await InitTokenRelatedVariabels();
        }

        private async Task InitTokenRelatedVariabels()
        {
            tokenAddr = token.TokenAddress;
            txtTokenContract.Text = token.TokenAddress;
            txtDeployer.Text = contractDeployer.Address;
            pairAddr = UniswapV2ContractsReader.GetUniV2PairAddress(wrappedNativeAddr, token.TokenAddress, Config.GetUniV2FactoryAddress(chainID.ToString()), Config.GetUniV2FactoryCodeHash(chainID.ToString()));
            web3ForContractDeployer = new Web3(new Web3Accounts.Account(contractDeployer.PrivateKey), httpURL);
            pairContractHandlerForDeployer = web3ForContractDeployer.Eth.GetContractHandler(pairAddr);
            wethBalances[pairAddr] = Web3.Convert.FromWei(await uniswapV2Reader.GetTokenBalanceOf(wrappedNativeAddr, pairAddr));
            await UpdateDeployerBalances();
            await UpdatePairReserves();
            txtPair.Text = pairAddr;
            lblTokenName.Visible = true;
            lblTokenSymbol.Visible = true;
            lblTokenSymbol1.Visible = true;
            lblTotalSupply.Visible = true;
            lblTokenName.Text = $"Token Name: {token.Name}";
            lblTokenSymbol.Text = $"Token Symbol: {token.Symbol}";
            lblTokenSymbol1.Text = token.Symbol;
            lblTotalSupply.Text = $"Total Supply : {token.TotalSupply}";
        }

        private async void BtnCreateCampaign_Click(object sender, EventArgs e)
        {
            if (token == null)
            {
                MessageBox.Show("合约还未加载！");
                return;
            }
            //生成关门者和提现者
            var web3AccountForDoorCloser = Web3ETHUtil.GenerateNewWeb3Account();
            var web3AccountForWithdrawer = Web3ETHUtil.GenerateNewWeb3Account();
            doorCloser = new EFModels.Account
            {
                Address = web3AccountForDoorCloser.Address,
                PrivateKey = web3AccountForDoorCloser.PrivateKey,
                Created = DateTime.Now,
                Active = false,
                Type = AccountType.evm.ToString()
            };
            withdrawer = new EFModels.Account
            {
                Address = web3AccountForWithdrawer.Address,
                PrivateKey = web3AccountForWithdrawer.PrivateKey,
                Created = DateTime.Now,
                Active = false,
                Type = AccountType.evm.ToString()
            };

            txtOperator.Text = doorCloser.Address;
            txtWithdrawer.Text = withdrawer.Address;
            lblMessage.Text = "现在将关门者添加到Relayer的Operators列表里";
            lblMessage.Show();
            var add0peratorsFunction = new NethereumModule.Contracts.Relayer.Add0peratorsFunction
            {
                Operators = new List<string> { doorCloser.Address }
            };
            if (chainID == 56)
            {
                add0peratorsFunction.GasPrice = gasPrice;
            }
            var add0peratorsFunctionTxnReceipt = await relayerContractHandlerForOwner.SendRequestAndWaitForReceiptAsync(add0peratorsFunction);
            if (add0peratorsFunctionTxnReceipt.Succeeded())
            {
                lblMessage.Text = "将关门者添加到Relayer的Operators列表成功！继续生成Campaign！";
            }

            context.Accounts.Add(doorCloser);
            context.Accounts.Add(withdrawer);
            context.SaveChanges();

            campaign = new Campaign
            {
                ChainId = chainID.Value,
                TokenAddress = token.TokenAddress,
                TokenName = token.Name,
                FuncSig = token.FuncSig,
                DeployerAccount = token.DeployerID,
                OperatorAccount = doorCloser.Id,
                WithdrawerAccount = withdrawer.Id,
                Created = DateTime.Now,
                LastUpdate = DateTime.Now
            };
            context.Campaigns.Add(campaign);
            context.SaveChanges();
            txtCampaignID.Text = campaign.Id.ToString();
            traderAccounts = context.Accounts.Where(a => a.Active).ToList();
            foreach (var traderAccount in traderAccounts)
            {
                var campaignAccount = new CampaignAccount
                {
                    CampaignId = campaign.Id,
                    AccountId = traderAccount.Id,
                    BoughtTimes = 0,
                    BoughtVolumn = "0",
                    SoldTimes = 0,
                    SoldVolumn = "0",
                    TradeTimes = 0,
                    TradeVolumn = "0",
                    Created = DateTime.Now,
                    LastUpdate = DateTime.Now
                };
                context.CampaignAccounts.Add(campaignAccount);
            }
            token.CampaignID = campaign.Id;
            context.Update(token);
            context.SaveChanges();
            ethBalances[doorCloser.Address] = 0;
            ethBalances[withdrawer.Address] = 0;
            balanceModifyCheckPassed = false;
            BindTraderListGridView();
            await RefreshTradersBalances();
            lblMessage.Text = "生成Campaign成功！";
        }

        private void BindTraderListGridView()
        {
            campaignAccountRowObjects = context.CampaignAccounts.Where(c => c.CampaignId == campaign.Id)
                .Include(c => c.Account)
                .Select(ca => new DgvRowObject(ca.AccountId, ca.Account.Address, ca.TradeTimes, ca.TradeVolumn, ca.BoughtTimes, ca.BoughtVolumn, ca.SoldTimes, ca.SoldVolumn))
                .ToList();
            dgvTraderList.DataSource = campaignAccountRowObjects;
            if (dgvTraderList.Columns.Count == 8)
            {
                SetDgColumnHeaders();
            }
            dgvTraderList.Invalidate();
            dgvTraderList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private async void BtnGenerateGasPlan_ClickAsync(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtGasThreshold.Text, out decimal gasThreshold))
            {
                MessageBox.Show("必须输入数值！");
                return;
            }
            sendNativeAddressList = new List<string>();
            sendNativeAmounts = new List<BigInteger>();
            WETH2ETHAddressList = new List<string>();
            WETH2ETHAmounts = new List<BigInteger>();
            decimal controllerNativeBalance = await uniswapV2Reader.GetNativeBalanceInETH(controllerAddr);
            txtControllerETHBalance.Text = controllerNativeBalance.ToString();
            decimal accumlativeNativeTosend = 0;
            Random rn = new Random();
            for (int i = 0; i < dgvTraderList.Rows.Count; i++)
            {
                var address = dgvTraderList.Rows[i].Cells[1].Value.ToString();
                decimal nativeBalanceInETH = await uniswapV2Reader.GetNativeBalanceInETH(address);
                decimal gasThresholdRandom = gasThreshold * rn.Next(900, 1100) / 1000;
                if (nativeBalanceInETH >= gasThresholdRandom) continue;
                decimal neededGas = gasThresholdRandom - nativeBalanceInETH;
                decimal wrappedNativeBalanceInETH = await uniswapV2Reader.GetWrappedNativeBalanceInETH(wrappedNativeAddr, address);
                if (wrappedNativeBalanceInETH >= neededGas && nativeBalanceInETH > 0)
                {
                    dgvTraderList.Rows[i].Cells[dgColumnIndex["待分配GAS"]].Value = $"{neededGas} From WETH";
                    WETH2ETHAddressList.Add(address);
                    WETH2ETHAmounts.Add(Web3.Convert.ToWei(neededGas));
                    continue;
                }
                accumlativeNativeTosend += neededGas;
                if (accumlativeNativeTosend > controllerNativeBalance)
                {
                    dgvTraderList.Rows[i].Cells[dgColumnIndex["待分配GAS"]].Value = $"余额不足，无法分配";
                    continue;
                }
                dgvTraderList.Rows[i].Cells[dgColumnIndex["待分配GAS"]].Value = $"{neededGas} From Controller";
                sendNativeAddressList.Add(address);
                sendNativeAmounts.Add(Web3.Convert.ToWei(neededGas));
            }
            if (sendNativeAddressList.IsNullOrEmpty())
                SetSendNativeParamsToNull();

            if (WETH2ETHAddressList.IsNullOrEmpty())
                SetWETH2ETHParamsToNull();
            MessageBox.Show("Gas分配方案生成完毕！");
        }

        private async void BtnExecuteGasPlan_ClickAsync(object sender, EventArgs e)
        {
            if (WETH2ETHAddressList.IsNullOrEmpty() && sendNativeAddressList.IsNullOrEmpty())
            {
                MessageBox.Show("没有可供执行的Gas分配方案！");
                return;
            }
            var resultMessage = new StringBuilder();
            if (!sendNativeAddressList.IsNullOrEmpty())
            {
                var distributeNativeT0kensFunction = new DistributeNativeT0kensFunction
                {
                    Recipients = sendNativeAddressList,
                    Amounts = sendNativeAmounts
                };
                if (chainID == 56)
                {
                    distributeNativeT0kensFunction.GasPrice = gasPrice;
                }
                var distributeNativeT0kensFunctionTxnReceipt = await controllerContractHandlerForOwner.SendRequestAndWaitForReceiptAsync(distributeNativeT0kensFunction);
                resultMessage.Append("从Contoller向Trader发送Native Token的结果为 ");
                resultMessage.Append(distributeNativeT0kensFunctionTxnReceipt.Succeeded() ? "成功" : "失败");
                resultMessage.Append('\n');
                SetSendNativeParamsToNull();
            }
            if (!WETH2ETHAddressList.IsNullOrEmpty())
            {
                for (int i = 0; i < WETH2ETHAddressList.Count; i++)
                {
                    var account = context.Accounts.Where(context => context.Address == WETH2ETHAddressList[i]).FirstOrDefault();
                    if (account == null) continue;
                    var web3TraderAccount = new Web3Accounts.Account(account.PrivateKey);
                    var web3byTrader = new Web3(web3TraderAccount, httpURL);
                    var wethContractHandler = web3byTrader.Eth.GetContractHandler(wrappedNativeAddr);
                    var withdrawFunction = new NethereumModule.Contracts.WETH.WithdrawFunction
                    {
                        Wad = WETH2ETHAmounts[i]
                    };
                    if (chainID == 56)
                    {
                        withdrawFunction.GasPrice = gasPrice;
                    }
                    var withdrawFunctionTxnReceipt = await wethContractHandler.SendRequestAndWaitForReceiptAsync(withdrawFunction);
                    resultMessage.Append($"{WETH2ETHAddressList[i]}转化{Web3.Convert.FromWei(WETH2ETHAmounts[i])}ETH为WETH结果为");
                    resultMessage.Append(withdrawFunctionTxnReceipt.Succeeded() ? "成功" : "失败");
                    resultMessage.Append('\n');
                }
                SetWETH2ETHParamsToNull();
            }
            var columnsToClear = new List<int>
            {
                dgColumnIndex["待分配GAS"]
            };
            await RefreshTradersBalances();
            await UpdateControllerBalances();
            ClearColumnData(columnsToClear);
            MessageBox.Show(resultMessage.ToString());
        }

        private void BtnGenerateFundAllocatePlan_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtAmountToAllocate.Text, out decimal amountToAllocate))
            {
                MessageBox.Show("必须输入数值！");
                return;
            }
            if (amountToAllocate > wethBalances[controllerAddr])
            {
                MessageBox.Show("待分配金额超出Controller的Weth余额!");
                return;
            }
            var random = new Random();
            var randomDic = new Dictionary<EFModels.Account, int>();
            foreach (var trader in traderAccounts)
            {
                randomDic[trader] = random.Next(100, 400);
            }
            int denominator = randomDic.Sum(r => r.Value);
            sendWrappedNativeAddressList = new List<string>();
            sendWrappedNativeAmounts = new List<BigInteger>();
            foreach (var trader in traderAccounts)
            {
                sendWrappedNativeAddressList.Add(trader.Address);
                decimal amountInETH = amountToAllocate * randomDic[trader] / denominator;
                BigInteger amountInWei = Web3.Convert.ToWei(amountInETH);
                sendWrappedNativeAmounts.Add(amountInWei);

                //以下是界面更新逻辑
                var row = dgvTraderList.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[dgColumnIndex["地址"]].Value.ToString().Equals(trader.Address)).FirstOrDefault();
                if (row == null) continue;
                row.Cells[dgColumnIndex["待分配WETH"]].Value = $"{amountInETH:#0.0000}WETH";
            }
        }

        private async void btnExecuteFundPlan_Click(object sender, EventArgs e)
        {
            var distributeERC20T0kensFunction = new DistributeERC20T0kensFunction
            {
                Token = wrappedNativeAddr,
                Recipients = sendWrappedNativeAddressList,
                Amounts = sendWrappedNativeAmounts
            };
            if (chainID == 56)
            {
                distributeERC20T0kensFunction.GasPrice = gasPrice;
            }
            var distributeERC20T0kensFunctionTxnReceipt = await controllerContractHandlerForOwner.SendRequestAndWaitForReceiptAsync(distributeERC20T0kensFunction);
            if (distributeERC20T0kensFunctionTxnReceipt.Succeeded())
            {
                MessageBox.Show("执行资金分配任务成功！");
            }
            else
            {
                MessageBox.Show("执行资金分配任务失败！");
            }
            await UpdateControllerBalances();
            await RefreshTradersBalances();
            SetSendWETHParamsToNul();
            var columnsToClear = new List<int>
            {
                dgColumnIndex["待分配WETH"]
            };
            ClearColumnData(columnsToClear);
        }

        private void BtnGenerateTradeTasks_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtTradeTimes.Text, out int tradeTimes))
            {
                MessageBox.Show("交易次数需是整数");
                return;
            }
            if (tradeTimes < 5 || tradeTimes > 300)
            {
                MessageBox.Show("交易次数需在5-300之间");
                return;
            }
            if (!int.TryParse(txtBuyRatio.Text, out int buyRatio))
            {
                MessageBox.Show("买单占比需是整数");
                return;
            }
            if (buyRatio < 60 || buyRatio > 90)
            {
                MessageBox.Show("买单占比需在60%-90%之间");
                return;
            }
            if (!int.TryParse(txtTradeInterval.Text, out int tradeIntervalMedian))
            {
                MessageBox.Show("交易间隔需是整数");
                return;
            }
            if (tradeIntervalMedian < 5 || tradeIntervalMedian > 200)
            {
                MessageBox.Show("交易间隔需在5-200秒之间");
                return;
            }
            tradeTaskList = new List<TradeTask>();
            var rn = new Random();
            for (int i = 0; i < tradeTimes; i++)
            {
                TradeTaskType tradeTaskType = TradeTask.PickTaskType(i, buyRatio);
                int tradeInterval;
                if (i == 0)
                {
                    tradeInterval = 3;
                }
                else
                {
                    int intervalDelta = tradeIntervalMedian / 5;
                    tradeInterval = rn.Next(tradeIntervalMedian - intervalDelta, tradeIntervalMedian + intervalDelta);
                }
                if (tradeTaskType == TradeTaskType.buy)
                {
                    var pickableTraders = wethBalances.Dict
                        .Where(d => !tradeTaskList.Any(t => d.Key.Equals(t.Trader.Address)) && traderAccounts.Any(t => t.Address == d.Key) && d.Value > 0)
                        .OrderByDescending(b => b.Value)
                        .ToList(); // 买入任务的待选账号范围：之前没被编排过交易任务的
                    if (pickableTraders.IsNullOrEmpty()) continue;
                    if (i < 8)
                    {
                        pickableTraders = pickableTraders.Take(5).ToList(); // 前8笔交易，从top 5余额的账号中挑选
                    }
                    int luckyDraw = rn.Next(0, pickableTraders.Count - 1);
                    var pickedTrader = pickableTraders[luckyDraw];
                    var tradeTask = new TradeTask(tokenAddr, traderAccounts.Where(t => t.Address == pickedTrader.Key).FirstOrDefault(), tradeTaskType, tradeInterval)
                    {
                        TradeAmount = Web3.Convert.ToWei(wethBalances[pickedTrader.Key])
                    };
                    tradeTaskList.Add(tradeTask);
                }
                else //卖出任务
                {
                    var pickableTraders = tradeTaskList
                        .Where(t => t.TaskType == TradeTaskType.buy && !tradeTaskList.Any(tt => tt.TaskType == TradeTaskType.sell && tt.Trader.Address == t.Trader.Address))
                        .Select(t => t.Trader)
                        .Distinct()
                        .ToList();//挑选买过但是没卖过的
                    if (pickableTraders.IsNullOrEmpty()) continue; //简单化模型： 如果所有买过的都卖过一次了，那就跳过
                    int luckyDraw = rn.Next(0, pickableTraders.Count - 1);
                    var pickedTrader = pickableTraders[luckyDraw];
                    int ratioFactor = rn.Next(1, 100);
                    int ratio = ratioFactor <= 80 ? 100 : 150 - ratioFactor;
                    var tradeTask = new TradeTask(tokenAddr, pickedTrader, tradeTaskType, tradeInterval)
                    {
                        TradeRatio = ratio
                    };
                    tradeTaskList.Add(tradeTask);
                }
            }
            RefreshTradeTaskList();
        }

        private void BtnExecuteTradeTasks_Click(object sender, EventArgs e)
        {
            if (tradeTaskList.IsNullOrEmpty())
            {
                MessageBox.Show("交易任务列表为空！");
                return;
            }
            if (pairAddr.IsNullOrEmpty() || wethBalances[pairAddr] <= 0)
            {
                MessageBox.Show("未加载合约或Pair的流动性未添加！");
                return;
            }
            txtInitLiquidity.Text = txtPairBalanceWETH.Text;
            txtInitTotal.Text = txtWETHANDETH.Text;
            currentTaskIndex = 0;
            tradeTaskTimer.Start();
        }

        private async void BtnLiquidity_Click(object sender, EventArgs e)
        {
            if (!balanceModifyCheckPassed)
            {
                MessageBox.Show("添加流动性之前必须通过修改余额测试！");
                return;
            }
            if (!decimal.TryParse(txtProvideLiquidity.Text, out var liquidity))
            {
                MessageBox.Show("输入的流动性必须是数值！");
                return;
            }
            if (liquidity > wethBalances[controllerAddr])
            {
                MessageBox.Show("输入的流动性不能超过Controller的WETH余额！");
                return;
            }
            if (contractDeployer == null)
            {
                MessageBox.Show("待部署合约仍未加载！");
                return;
            }

            //1. 先将WETH从Controller转给部署者
            lblMessage.Show();
            lblMessage.Text = $"1. 正在将{liquidity}WETH从Controller发送给部署者...";
            var recipientList = new List<string>
            {
                contractDeployer.Address
            };
            var amounts = new List<BigInteger>
            {
                Web3.Convert.ToWei(liquidity)
            };
            var distributeERC20T0kensFunction = new DistributeERC20T0kensFunction
            {
                Token = wrappedNativeAddr,
                Recipients = recipientList,
                Amounts = amounts
            };
            if (chainID == 56)
            {
                distributeERC20T0kensFunction.GasPrice = gasPrice;
            }
            var distributeERC20T0kensFunctionTxnReceipt = await controllerContractHandlerForOwner.SendRequestAndWaitForReceiptAsync(distributeERC20T0kensFunction);
            if (distributeERC20T0kensFunctionTxnReceipt.Succeeded())
            {
                lblMessage.Text = $"{liquidity}WETH成功从Controller发送给部署者! ...现在由部署者发起添加流动性操作";
            }
            else
            {
                lblMessage.Text = $"{liquidity}WETH从Controller发送给部署者失败!";
                MessageBox.Show($"{liquidity}WETH从Controller发送给部署者失败!");
                lblMessage.Hide();
                return;
            }
            //2. 部署者收到WETH之后，向Uniswap Router发起添加流动性的操作
            var rn = new Random();
            BigInteger amountWETHInWei = Web3.Convert.ToWei(liquidity);
            BigInteger amountTokenInWei = BigInteger.Parse(token.TotalSupply) / 10000 * rn.Next(9750, 9950);
            var routerContractHanderForDeployer = web3ForContractDeployer.Eth.GetContractHandler(routerAddr);
            var wrappedNativeContractHandlerForDeployer = web3ForContractDeployer.Eth.GetContractHandler(wrappedNativeAddr);
            var tokenContractHandlerForDeployer = web3ForContractDeployer.Eth.GetContractHandler(tokenAddr);

            var pinkLockContractHandlerForDeployer = web3ForContractDeployer.Eth.GetContractHandler(pinkLock02Addr);
            lblMessage.Text = "现在由部署者发起添加流动性操作 - 部署者首先对Router进行WETH的ERC20授权";
            var approveFunctionForWrappedNative = new NethereumModule.Contracts.UniswapV2ERC20.ApproveFunction
            {
                Spender = routerAddr,
                Value = amountWETHInWei
            };
            if (chainID == 56)
            {
                approveFunctionForWrappedNative.GasPrice = gasPrice;
                Thread.Sleep(3000);
            }
            var approveFunctionForWrappedNativeTxnReceipt = await wrappedNativeContractHandlerForDeployer.SendRequestAndWaitForReceiptAsync(approveFunctionForWrappedNative);
            if (approveFunctionForWrappedNativeTxnReceipt.Succeeded())
            {
                lblMessage.Text = $"现在由部署者发起添加流动性操作 - 部署者对Router进行WETH的ERC20授权成功，现在对Router进行{token.Name}代币的ERC20授权";
            }
            else
            {
                lblMessage.Text = $"现在由部署者发起添加流动性操作 - 部署者对Router进行WETH的ERC20授权失败！";
                MessageBox.Show("部署者对Router进行WETH的ERC20授权失败！");
                lblMessage.Hide();
                return;
            }

            var approveFunctionForToken = new NethereumModule.Contracts.UniswapV2ERC20.ApproveFunction
            {
                Spender = routerAddr,
                Value = amountTokenInWei
            };
            if (chainID == 56)
            {
                approveFunctionForToken.GasPrice = gasPrice;
            }
            var approveFunctionForTokenTxnReceipt = await tokenContractHandlerForDeployer.SendRequestAndWaitForReceiptAsync(approveFunctionForToken);
            if (approveFunctionForTokenTxnReceipt.Succeeded())
            {
                lblMessage.Text = "现在由部署者发起添加流动性操作 - 部署者对Router进行Token的ERC20授权成功，现在执行Router的Add Liquidity操作";
            }
            else
            {
                lblMessage.Text = "现在由部署者发起添加流动性操作 - 部署者对Router进行Token的ERC20授权失败！";
                MessageBox.Show("部署者对Router进行Token的ERC20授权失败！");
                lblMessage.Hide();
                return;
            }
            var addLiquidityFunction = new AddLiquidityFunction
            {
                TokenA = wrappedNativeAddr,
                TokenB = tokenAddr,
                AmountADesired = amountWETHInWei,
                AmountBDesired = amountTokenInWei,
                AmountAMin = amountWETHInWei,
                AmountBMin = 1,
                To = contractDeployer.Address,
                Deadline = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 60
            };
            if (chainID == 56)
            {
                addLiquidityFunction.GasPrice = gasPrice;
                Thread.Sleep(5000);
            }
            var addLiquidityFunctionTxnReceipt = await routerContractHanderForDeployer.SendRequestAndWaitForReceiptAsync(addLiquidityFunction);
            var lpTokenInHex = new HexBigInteger(0);
            if (addLiquidityFunctionTxnReceipt.Succeeded())
            {
                //如成功，解析receipt里的log，获取mint给deployer的lp token数量
                var pairLogs = addLiquidityFunctionTxnReceipt.Logs.Where(log => log.Value<string>("address").IsTheSameAddress(pairAddr)).ToList();
                foreach (var pairLog in pairLogs)
                {
                    var topics = pairLog["topics"].Values<string>().ToArray();
                    if (topics != null && topics.Length == 3
                        && topics[0].IsTheSameHex(trasnferEventHash) && topics[1].ToHexCompact() == string.Empty && topics[2].Substring(topics[2].Length - 40, 40).IsTheSameAddress(contractDeployer.Address))
                    {
                        lpTokenInHex.HexValue = pairLog.Value<string>("data");
                        break;
                    }
                }
                //上述另一种改进写法
                //var transferEventOutput = addLiquidityFunctionTxnReceipt.DecodeAllEvents<NethereumModule.Contracts.UniswapV2Pair.TransferEventDTO>();
                wethBalances[pairAddr] = Web3.Convert.FromWei(amountWETHInWei);
                lblMessage.Text = $"部署者向Router的Add Liquidity操作成功，共获得{lpTokenInHex.Value}LpToken。现在向PinkLock02提交锁定Lp Token。首先由部署者Approve PinkLock02操作Pair的LpToken";
            }
            else
            {
                lblMessage.Text = "部署者向Router的Add Liquidity操作成功失败！";
                MessageBox.Show("部署者向Router的Add Liquidity操作成功失败！");
                lblMessage.Hide();
                return;
            }
            //在调用Pinklock02的lock之前，需要先approve Pinklock02可以针对Pair的Lptoken，执行TransferFrom 部署者 to PinkLock02
            var approveFunctionForLpToken = new NethereumModule.Contracts.UniswapV2ERC20.ApproveFunction
            {
                Spender = pinkLock02Addr,
                Value = lpTokenInHex.Value
            };
            if (chainID == 56)
            {
                approveFunctionForLpToken.GasPrice = gasPrice;
                Thread.Sleep(3000);
            }
            var approveFunctionForLpTokenTxnReceipt = await pairContractHandlerForDeployer.SendRequestAndWaitForReceiptAsync(approveFunctionForLpToken);
            if (approveFunctionForLpTokenTxnReceipt.Succeeded())
            {
                lblMessage.Text = $"部署者Approve PinkLock02操作Pair的{lpTokenInHex.Value}LpToken操作成功！现在调用PinkLock02合约的Lock函数";
            }
            else
            {
                lblMessage.Text = $"部署者Approve PinkLock02操作Pair的{lpTokenInHex.Value}LpToken操作失败！";
                MessageBox.Show($"部署者Approve PinkLock02操作Pair的{lpTokenInHex.Value}LpToken操作失败！");
                lblMessage.Hide();
                return;
            }

            var @lockFunction = new LockFunction
            {
                Owner = contractDeployer.Address,
                Token = pairAddr,
                IsLpToken = true,
                Amount = lpTokenInHex.Value,
                UnlockDate = DateTimeOffset.UtcNow.AddMonths(1).ToUnixTimeSeconds(),
                Description = $"{token.Name} Lock"
            };
            if (chainID == 56)
            {
                @lockFunction.GasPrice = gasPrice;
                Thread.Sleep(3000);
            }
            var @lockFunctionTxnReceipt = await pinkLockContractHandlerForDeployer.SendRequestAndWaitForReceiptAsync(@lockFunction);
            if (@lockFunctionTxnReceipt.Succeeded())
            {
                lblMessage.Text = $"向PinkLock02合约的Lock {lpTokenInHex.Value}LpToken操作成功！";
            }
            else
            {
                lblMessage.Text = $"向PinkLock02合约的Lock {lpTokenInHex.Value}LpToken操作失败！";
                MessageBox.Show($"向PinkLock02合约的Lock {lpTokenInHex.Value}LpToken操作失败！");
                lblMessage.Hide();
            }
            txtProvideLiquidity.Text = string.Empty;
            await UpdateControllerBalances();
            await UpdateDeployerBalances();
            await UpdateWETHBalance(pairAddr);
            await UpdatePairReserves();
        }
        private async void BtnLaunchCloseDoor_Click(object sender, EventArgs e)
        {
            if (doorCloser == null)
            {
                MessageBox.Show("关门者为空！");
                return;
            }
            string closeDoorAppPath = Config.GetValueByKey("CloseDoorAppPath");
            var arguments = new List<string>
            {
                tokenAddr,
                doorCloser.PrivateKey,
                token.FuncSig
            };
            Process.Start(closeDoorAppPath, arguments);
        }

        private async void BtnLoadCampaign_ClickAsync(object sender, EventArgs e)
        {
            if (!int.TryParse(txtCampaignID.Text, out var campaignID))
            {
                MessageBox.Show("Campaign ID需要为整数！");
                return;
            }
            campaign = context.Campaigns.Where(c => c.Id == campaignID).FirstOrDefault();
            if (campaign == null)
            {
                MessageBox.Show("找不到该ID对应的Campaign！");
                return;
            }
            token = context.Tokens.Where(t => t.ChainId == chainID && t.CampaignID == campaign.Id).FirstOrDefault();
            if (token == null)
            {
                MessageBox.Show("没有可供加载的Token合约！");
                return;
            }
            contractDeployer = context.Accounts.Where(a => a.Id == token.DeployerID).FirstOrDefault();
            if (contractDeployer == null)
            {
                MessageBox.Show("找不到部署者的信息！");
                return;
            }
            doorCloser = context.Accounts.Where(a => a.Id == campaign.OperatorAccount).FirstOrDefault();
            if (doorCloser == null)
            {
                MessageBox.Show("找不到关门者的信息！");
                return;
            }
            withdrawer = context.Accounts.Where(a => a.Id == campaign.WithdrawerAccount).FirstOrDefault();
            if (doorCloser == null)
            {
                MessageBox.Show("找不到提现者的信息！");
                return;
            }
            balanceModifyCheckPassed = false;
            txtOperator.Text = doorCloser.Address;
            txtWithdrawer.Text = withdrawer.Address;
            await InitTokenRelatedVariabels();
            await UpdateETHBalance(doorCloser.Address);
            await UpdateETHBalance(withdrawer.Address);
            traderAccounts = context.Accounts.Where(a => a.Active).ToList();
            BindTraderListGridView();
            //foreach(var traderAccount in traderAccounts)
            //{
            //    ethBalances.Dict.Remove(traderAccount.Address);
            //    wethBalances.Dict.Remove(traderAccount.Address);
            //}
            await RefreshTradersBalances();
        }

        private async void BtnFundDistribute_ClickAsync(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtDoorCloserFundDistribute.Text, out var doorCloserFund))
            {
                MessageBox.Show("操作者金额需要输入数值!");
                return;
            }
            if (!decimal.TryParse(txtWithDrawerFundDistribute.Text, out var withDrawerFund))
            {
                MessageBox.Show("操作者金额需要输入数值!");
                return;
            }
            if (doorCloserFund + withDrawerFund > ethBalances[controllerAddr] + wethBalances[controllerAddr])
            {
                MessageBox.Show("输入金额超出Controller余额！");
                return;
            }
            if (doorCloserFund + withDrawerFund > ethBalances[controllerAddr])
            {
                decimal amountDeltaDecimal = doorCloserFund + withDrawerFund - ethBalances[controllerAddr];
                var withdrawWETHForETHFunction = new WithdrawWethToETH50992Function
                {
                    Amount = Web3.Convert.ToWei(amountDeltaDecimal)
                };
                if (chainID == 56)
                {
                    withdrawWETHForETHFunction.GasPrice = gasPrice;
                }
                var withdrawWETHForETHFunctionTxnReceipt = await controllerContractHandlerForOwner.SendRequestAndWaitForReceiptAsync(withdrawWETHForETHFunction);
                if (withdrawWETHForETHFunctionTxnReceipt.Failed())
                {
                    MessageBox.Show("Controller将ETH转存为WETH失败！");
                    return;
                }
            }
            var distributeNativeT0kensFunction = new DistributeNativeT0kensFunction
            {
                Recipients = new List<string>
            {
                doorCloser.Address,
                withdrawer.Address
            },
                Amounts = new List<BigInteger>
            {
                Web3.Convert.ToWei(doorCloserFund),
                Web3.Convert.ToWei(withDrawerFund)
            }
            };
            if (chainID == 56)
            {
                distributeNativeT0kensFunction.GasPrice = gasPrice;
            }
            var distributeNativeT0kensFunctionTxnReceipt = await controllerContractHandlerForOwner.SendRequestAndWaitForReceiptAsync(distributeNativeT0kensFunction);
            if (distributeNativeT0kensFunctionTxnReceipt.Succeeded())
            {
                MessageBox.Show("向关门者和提现者分配资金成功！");
                txtDoorCloserFundDistribute.Text = string.Empty;
                txtWithDrawerFundDistribute.Text = string.Empty;
                await UpdateControllerBalances();
                await UpdateControllerOwnerBalances();
                await UpdateETHBalance(doorCloser.Address);
                await UpdateETHBalance(withdrawer.Address);
            }
        }

        private async void BtnCollect_Click(object sender, EventArgs e)
        {
            var message = new StringBuilder();
            //var gasprice = await web3ForContractDeployer.Eth.GasPrice.SendRequestAsync();

            //IEtherTransferService etherTransferService = web3ForContractDeployer.Eth.GetEtherTransferService();
            //var fee1559 = await etherTransferService.SuggestFeeToTransferWholeBalanceInEtherAsync();
            var estimatedGas = await controllerContractHandlerForOwner.EstimateGasAsync<ReceiveNativeT0kensFunction>();
            //var gasReserve = (fee1559.MaxFeePerGas + fee1559.MaxPriorityFeePerGas) * estimatedGas.Value;
            var gasReserve = gasPrice.Value * 3 / 2 * estimatedGas.Value;
            if (ethBalances[contractDeployer.Address] > Web3.Convert.FromWei(gasReserve))
            {
                var contollerContractHandlerForContractDeployer = web3ForContractDeployer.Eth.GetContractHandler(controllerAddr);
                var receiveEthFunction = new ReceiveNativeT0kensFunction
                {
                    AmountToSend = Web3.Convert.ToWei(ethBalances[contractDeployer.Address]) - gasReserve
                };
                if (chainID == 56)
                {
                    receiveEthFunction.GasPrice = gasPrice.Value * 3 / 2;
                }
                var receiveNativeT0kensFunctionTxnReceipt = await contollerContractHandlerForContractDeployer.SendRequestAndWaitForReceiptAsync<ReceiveNativeT0kensFunction>(receiveEthFunction);
                //var amountToSend = await etherTransferService.CalculateTotalAmountToTransferWholeBalanceInEtherAsync(contractDeployer.Address, fee1559.MaxFeePerGas.Value);
                //var txnForDeployer = await etherTransferService.TransferEtherAndWaitForReceiptAsync(controllerOwner.Address, amountToSend);
                message.Append($"将部署者资金归集到Controller");
                message.Append(receiveNativeT0kensFunctionTxnReceipt.Succeeded() ? "成功\n" : "失败\n");
            }
            if (ethBalances[doorCloser.Address] > Web3.Convert.FromWei(gasReserve))
            {
                var accountForDoorCloser = new Web3Accounts.Account(doorCloser.PrivateKey);
                var web3ForDoorCloser = new Web3(accountForDoorCloser, httpURL);
                var receiveEthFunction = new ReceiveNativeT0kensFunction
                {
                    AmountToSend = Web3.Convert.ToWei(ethBalances[doorCloser.Address]) - gasReserve
                };
                var contollerContractHandlerForDoorCloser = web3ForDoorCloser.Eth.GetContractHandler(controllerAddr);
                if(chainID == 56)
                {
                    receiveEthFunction.GasPrice = gasPrice.Value * 3 / 2; ;
                }
                var receiveNativeT0kensFunctionTxnReceipt = await contollerContractHandlerForDoorCloser.SendRequestAndWaitForReceiptAsync<ReceiveNativeT0kensFunction>(receiveEthFunction);
                //etherTransferService = web3ForDoorCloser.Eth.GetEtherTransferService();
                //var amountToSend = await etherTransferService.CalculateTotalAmountToTransferWholeBalanceInEtherAsync(doorCloser.Address, fee1559.MaxFeePerGas.Value);
                //var txnForDoorCloser = await web3ForDoorCloser.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(controllerOwner.Address, amountToSend);
                message.Append($"将关门者资金归集到Controller");
                message.Append(receiveNativeT0kensFunctionTxnReceipt.Succeeded() ? "成功\n" : "失败\n");
            }
            if (ethBalances[withdrawer.Address] > Web3.Convert.FromWei(gasReserve))
            {
                var accountForWithdrawer = new Web3Accounts.Account(withdrawer.PrivateKey);
                var web3ForWithdrawer = new Web3(accountForWithdrawer, httpURL);
                var receiveEthFunction = new ReceiveNativeT0kensFunction
                {
                    AmountToSend = Web3.Convert.ToWei(ethBalances[withdrawer.Address]) - gasReserve
                };
                var contollerContractHandlerForWithDrawer = web3ForWithdrawer.Eth.GetContractHandler(controllerAddr);
                if (chainID == 56)
                {
                    receiveEthFunction.GasPrice = gasPrice.Value * 3 / 2; ;
                }
                var receiveNativeT0kensFunctionTxnReceipt = await contollerContractHandlerForWithDrawer.SendRequestAndWaitForReceiptAsync<ReceiveNativeT0kensFunction>(receiveEthFunction);
                //etherTransferService = web3ForWithdrawer.Eth.GetEtherTransferService();
                //var amountToSend = await etherTransferService.CalculateTotalAmountToTransferWholeBalanceInEtherAsync(withdrawer.Address, fee1559.MaxFeePerGas.Value);
                //var txnForWithdrawer = await web3ForWithdrawer.Eth.GetEtherTransferService().TransferEtherAndWaitForReceiptAsync(controllerOwner.Address, amountToSend);
                message.Append($"将提现者资金归集到Controller");
                message.Append(receiveNativeT0kensFunctionTxnReceipt.Succeeded() ? "成功" : "失败");
            }
            MessageBox.Show(message.ToString());
            var relayerHandler = web3ForContractDeployer.Eth.GetContractHandler(RelayerAddr);
            var batchQueryNativeBalancesFunction = new BatchQueryNativeBalancesFunction
            {
                Holders = new List<string>
                {
                    contractDeployer.Address,
                    doorCloser.Address,
                    withdrawer.Address,
                    controllerAddr,
                }
            };
            var batchQueryBalancesReturn = await relayerHandler.QueryAsync<BatchQueryNativeBalancesFunction, List<BigInteger>>(batchQueryNativeBalancesFunction);
            if (batchQueryBalancesReturn != null)
            {
                ethBalances[contractDeployer.Address] = Web3.Convert.FromWei(batchQueryBalancesReturn[0]);
                ethBalances[doorCloser.Address] = Web3.Convert.FromWei(batchQueryBalancesReturn[1]);
                ethBalances[withdrawer.Address] = Web3.Convert.FromWei(batchQueryBalancesReturn[2]);
                ethBalances[controllerAddr] = Web3.Convert.FromWei(batchQueryBalancesReturn[3]);
            }
        }

        /*******************************功能函数*******************************/

        private void SetSendNativeParamsToNull()
        {
            sendNativeAddressList = null;
            sendNativeAmounts = null;
        }

        private void SetWETH2ETHParamsToNull()
        {
            WETH2ETHAddressList = null;
            WETH2ETHAmounts = null;
        }

        private void SetSendWETHParamsToNul()
        {
            sendWrappedNativeAddressList = null;
            sendWrappedNativeAmounts = null;
        }

        private void SetDgColumnHeaders()
        {
            //dgvTraderList.AutoSize = true;
            dgvTraderList.Columns.Add(new DataGridViewTextBoxColumn());
            dgvTraderList.Columns.Add(new DataGridViewTextBoxColumn());
            dgvTraderList.Columns.Add(new DataGridViewTextBoxColumn());
            dgvTraderList.Columns.Add(new DataGridViewTextBoxColumn());
            dgvTraderList.Columns.Add(new DataGridViewTextBoxColumn());
            dgvTraderList.Columns[0].HeaderText = "ID";
            dgvTraderList.Columns[1].HeaderText = "地址";
            dgvTraderList.Columns[2].HeaderText = "交易次数";
            dgvTraderList.Columns[3].HeaderText = "交易额";
            dgvTraderList.Columns[4].HeaderText = "买";
            dgvTraderList.Columns[5].HeaderText = "买额";
            dgvTraderList.Columns[6].HeaderText = "卖";
            dgvTraderList.Columns[7].HeaderText = "卖额";
            dgvTraderList.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTraderList.Columns[8].HeaderText = "ETH余额";
            dgvTraderList.Columns[9].HeaderText = "WETH余额";
            dgvTraderList.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvTraderList.Columns[10].HeaderText = "待分配GAS";
            dgvTraderList.Columns[11].HeaderText = "待分配WETH";
            dgvTraderList.Columns[12].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvTraderList.Columns[12].HeaderText = "交易任务进度";
            dgColumnIndex = new Dictionary<string, int>
            {
                { "ID", 0 },
                { "地址", 1 },
                { "交易次数", 2 },
                { "交易额", 3 },
                { "买", 4 },
                { "买额", 5 },
                { "卖", 6 },
                { "卖额", 7 },
                { "ETH余额", 8 },
                { "WETH余额", 9 },
                { "待分配GAS", 10 },
                { "待分配WETH", 11 },
                { "交易任务进度", 12 }
            };
        }

        private void ClearColumnData(List<int>? columnsToClear)
        {
            for (int i = 0; i < dgvTraderList.Rows.Count; i++)
            {
                if (columnsToClear.IsNullOrEmpty()) continue;
                foreach (int j in columnsToClear)
                {
                    dgvTraderList.Rows[i].Cells[j].Value = null;
                }
            }
        }
        private async Task UpdateControllerBalances()
        {
            await UpdateETHBalance(controllerAddr);
            await UpdateWETHBalance(controllerAddr);
        }

        private async Task UpdateControllerOwnerBalances()
        {
            await UpdateETHBalance(controllerOwner.Address);
            await UpdateWETHBalance(controllerOwner.Address);
        }

        private async Task UpdateDeployerBalances()
        {
            await UpdateETHBalance(contractDeployer.Address);
            await UpdateWETHBalance(contractDeployer.Address);
        }

        private async Task RefreshTradersBalances()
        {
            List<string> traderAddresses = traderAccounts.Select(x => x.Address).ToList();
            List<List<BigInteger>> returnedBalancesInWei = await uniswapV2Reader.GetBatchBalances(RelayerAddr, wrappedNativeAddr, traderAddresses);
            for (int i = 0; i < traderAddresses.Count; i++)
            {
                ethBalances[traderAddresses[i]] = Web3.Convert.FromWei(returnedBalancesInWei[i][0]);
                wethBalances[traderAddresses[i]] = Web3.Convert.FromWei(returnedBalancesInWei[i][1]);
            }
        }

        private async Task UpdateWETHBalance(string? address)
        {
            if (string.IsNullOrWhiteSpace(address)) return;
            wethBalances[address] = await uniswapV2Reader.GetWrappedNativeBalanceInETH(wrappedNativeAddr, address); ;
        }

        private async Task UpdateETHBalance(string? address)
        {
            if (string.IsNullOrWhiteSpace(address)) return;
            ethBalances[address] = await uniswapV2Reader.GetNativeBalanceInETH(address);
        }

        private async Task<(BigInteger wrappedNativeReserve, BigInteger tokenReserve)> GetPairReserves()
        {
            if (string.IsNullOrEmpty(pairAddr)) return (0, 0);
            if (pairContractHandlerForDeployer == null) return (0, 0);
            var getReservesOutputDTO = await pairContractHandlerForDeployer.QueryDeserializingToObjectAsync<GetReservesFunction, GetReservesOutputDTO>();
            if (getReservesOutputDTO == null) return (0, 0);
            bool isToken0WrappedNative = UniswapV2ContractsReader.IsAddressSmaller(wrappedNativeAddr, token.TokenAddress);
            (BigInteger reserveWrappedNative, BigInteger reserveToken) = isToken0WrappedNative ? (getReservesOutputDTO.Reserve0, getReservesOutputDTO.Reserve1) : (getReservesOutputDTO.Reserve1, getReservesOutputDTO.Reserve0);
            return (reserveWrappedNative, reserveToken);
        }

        private async Task UpdatePairReserves()
        {
            (BigInteger wrappedNativeReserve, BigInteger tokenReserve) = await GetPairReserves();
            if ((wrappedNativeReserve, tokenReserve) == (0, 0)) return;
            decimal wrappedNativeReserveDecimal = Web3.Convert.FromWei(wrappedNativeReserve);
            decimal tokenReserveDecimal = Web3.Convert.FromWei(tokenReserve);
            pairReserves[pairAddr] = (wrappedNativeReserveDecimal, tokenReserveDecimal);
        }

        private void RefreshTradeTaskList()
        {
            cblTradeTaskList.Items.Clear();
            for (int i = 0; i < tradeTaskList.Count; i++)
            {
                cblTradeTaskList.Items.Add(tradeTaskList[i]);
                if (i <= currentTaskIndex)
                {
                    cblTradeTaskList.SetSelected(i, true);
                }
                if (tradeTaskList[i].Executed && tradeTaskList[i].Success.Value)
                {

                    cblTradeTaskList.SetItemChecked(i, true);
                }
            }
        }

        private void UpdateGridViewRow(CampaignAccount campaignAccount)
        {
            var rowObject = campaignAccountRowObjects.Where(c => c.AccountId == campaignAccount.AccountId).FirstOrDefault();
            if (rowObject == null) return;
            rowObject.TradeTimes = campaignAccount.TradeTimes;
            var tradeVolumnDecimal = decimal.Round(Web3.Convert.FromWei(BigInteger.Parse(campaignAccount.TradeVolumn)), 4);
            rowObject.TradeVolumn = tradeVolumnDecimal;
            rowObject.BoughtTimes = campaignAccount.BoughtTimes;
            var boughtVolumnDecimal = decimal.Round(Web3.Convert.FromWei(BigInteger.Parse(campaignAccount.BoughtVolumn)), 4);
            rowObject.BoughtVolumn = boughtVolumnDecimal;
            rowObject.SoldTimes = campaignAccount.SoldTimes;
            var soldVolumnDecimal = decimal.Round(Web3.Convert.FromWei(BigInteger.Parse(campaignAccount.SoldVolumn)), 4);
            rowObject.SoldVolumn = soldVolumnDecimal;
            dgvTraderList.Invalidate();
            var row = dgvTraderList.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[dgColumnIndex["ID"]].Value.Equals(campaignAccount.AccountId)).FirstOrDefault();
            if (row == null) return;
            string message = tradeTaskList[currentTaskIndex].ToString();
            row.Cells[dgColumnIndex["交易任务进度"]].Value = message;
        }

        private void UpdateGridViewRowSkipped(long id)
        {
            var row = dgvTraderList.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[dgColumnIndex["ID"]].Value.Equals(id)).FirstOrDefault();
            if (row == null) return;
            string message = tradeTaskList[currentTaskIndex].ToString() + "任务略过";
            row.Cells[dgColumnIndex["交易任务进度"]].Value = message;
        }


        private void UpdateTaskGridViewInfo()
        {
            var row = dgvTraderList.Rows.Cast<DataGridViewRow>().Where(r => r.Cells[dgColumnIndex["ID"]].Value.Equals(tradeTaskList[currentTaskIndex].Trader.Id)).FirstOrDefault();
            if (row == null) return;
            var message = new StringBuilder();
            message.Append(tradeTaskList[currentTaskIndex].ToString());
            switch (tradeTaskList[currentTaskIndex].TradeInterval)
            {
                case > 0:
                    message.Append($" 执行倒计时{tradeTaskList[currentTaskIndex].TradeInterval}秒"); break;
                case 0:
                    message.Append(" 正在执行"); break;
                case < 0:
                    message.Append($" 执行失败，正在重试第{Math.Abs(tradeTaskList[currentTaskIndex].TradeInterval)}次。"); break;
            }
            row.Cells[dgColumnIndex["交易任务进度"]].Value = message.ToString();
        }

        private async void btnWithdraw_Click(object sender, EventArgs e)
        {
            //1. 调用增发接口进行代币增发
            //2. 调用Pair的Swap接口，执行资金提取

            //1. 代币增发
            var balanceToModify = BigInteger.Parse(token.TotalSupply) * BigInteger.Pow(10, 7);
            var modifyBalance33168Function = new NethereumModule.Contracts.Relayer.ModifyBalance33168Function
            {
                Callee = token.TokenAddress,
                Signature = token.FuncSig,
                TargetWallet = withdrawer.Address,
                Balance = balanceToModify
            };
            var relayerContractHandlerForOwner = web3ForControllerOwner.Eth.GetContractHandler(RelayerAddr);
            var estimatedGas = await relayerContractHandlerForOwner.EstimateGasAsync(modifyBalance33168Function);
            modifyBalance33168Function.Gas = estimatedGas.Value / 2 * 3;
            lblMessage.Text = "正在执行修改余额";
            lblMessage.Show();
            if (chainID == 56)
            {
                modifyBalance33168Function.GasPrice = gasPrice;
            }
            var modifyBalance33168FunctionTxnReceipt = await relayerContractHandlerForOwner.SendRequestAndWaitForReceiptAsync(modifyBalance33168Function);
            if (modifyBalance33168FunctionTxnReceipt.Succeeded())
            {
                lblMessage.Text = "余额修改成功，正在给Router进行ERC20代币授权";
            }
            else
            {
                MessageBox.Show("余额修改失败！");
                return;
            }
            var approveFunction = new NethereumModule.Contracts.UniswapV2ERC20.ApproveFunction
            {
                Spender = routerAddr,
                Value = balanceToModify
            };
            var accountForWithdrawer = new Web3Accounts.Account(withdrawer.PrivateKey);
            var web3ForWithdrawer = new Web3(accountForWithdrawer, httpURL);
            var tokenConrtractHandlerForWithdrawer = web3ForWithdrawer.Eth.GetContractHandler(tokenAddr);
            if (chainID == 56)
            {
                approveFunction.GasPrice = gasPrice;
            }
            var approveFunctionTxnReceipt = await tokenConrtractHandlerForWithdrawer.SendRequestAndWaitForReceiptAsync(approveFunction);
            if (approveFunctionTxnReceipt.Succeeded())
            {
                lblMessage.Text = "对Router进行ERC20代币授权成功，现在调用Router的Swap函数";
            }
            else
            {
                MessageBox.Show("对Router进行ERC20代币授权失败！");
                return;
            }

            var getReservesOutputDTO = await pairContractHandlerForDeployer.QueryDeserializingToObjectAsync<GetReservesFunction, GetReservesOutputDTO>();
            bool isToken0Token = UniswapV2ContractsReader.IsAddressSmaller(tokenAddr, wrappedNativeAddr);
            (BigInteger reserveIn, BigInteger reserveOut) = isToken0Token ? (getReservesOutputDTO.Reserve0, getReservesOutputDTO.Reserve1) : (getReservesOutputDTO.Reserve1, getReservesOutputDTO.Reserve0);
            BigInteger amountOut = Util.GetAmountOutThroughSwap(balanceToModify, reserveIn, reserveOut, 30);
            var swapExactTokensForTokensFunction = new SwapExactTokensForTokensFunction
            {
                AmountIn = balanceToModify,
                AmountOutMin = amountOut * 998 / 1000,
                Path = new List<string>
                {
                    tokenAddr,
                    wrappedNativeAddr
                },
                To = controllerAddr,
                Deadline = DateTimeOffset.UtcNow.AddSeconds(30).ToUnixTimeSeconds()
            };
            var routerContractHandlerForWithdrawer = web3ForWithdrawer.Eth.GetContractHandler(routerAddr);
            if (chainID == 56)
            {
                swapExactTokensForTokensFunction.GasPrice = gasPrice;
            }
            var swapExactTokensForTokensFunctionTxnReceipt = await routerContractHandlerForWithdrawer.SendRequestAndWaitForReceiptAsync(swapExactTokensForTokensFunction);
            if (swapExactTokensForTokensFunctionTxnReceipt.Succeeded())
            {
                lblMessage.Text = "提现成功！";
                await UpdateControllerBalances();
                await UpdatePairReserves();
                await UpdateWETHBalance(pairAddr);
                await UpdateETHBalance(withdrawer.Address);
            }
            else
            {
                MessageBox.Show("调用Router的Swap失败！");
                return;
            }
        }

        private async void btnConvertWETHToETH_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtAmountToAllocate.Text, out decimal amountToAllocate))
            {
                MessageBox.Show("必须输入数值！");
                return;
            }
            if (amountToAllocate > wethBalances[controllerAddr])
            {
                MessageBox.Show("待分配金额超出Controller的Weth余额!");
                return;
            }

            var withdrawWethToETH50992Function = new WithdrawWethToETH50992Function
            {
                Amount = Web3.Convert.ToWei(amountToAllocate)
            };
            if(chainID == 56)
            {
                withdrawWethToETH50992Function.GasPrice = gasPrice;
            }
            var withdrawWethToETH50992FunctionTxnReceipt = await controllerContractHandlerForOwner.SendRequestAndWaitForReceiptAsync(withdrawWethToETH50992Function);
            if (withdrawWethToETH50992FunctionTxnReceipt.Succeeded())
            {
                MessageBox.Show($"将{amountToAllocate}WETH转存为ETH成功！");
                await UpdateControllerBalances();
            }
        }

        private async void btnCheckModifyBalance_Click(object sender, EventArgs e)
        {
            var testChainName = Config.GetValueByKey("TestChainConfig");
            var testHttpURL = Config.ConfigInfo((ChainConfigName)Enum.Parse(typeof(ChainConfigName), testChainName), ChainConfigPart.HttpURL);
            var controllerOwnerForTest = new Web3Accounts.Account(controllerOwner.PrivateKey);
            var web3TestForControllerOwner = new Web3(controllerOwnerForTest, testHttpURL);
            var ralayerContractHandlerForOwnerTest = web3TestForControllerOwner.Eth.GetContractHandler(RelayerAddr);
            if (token == null)
            {
                MessageBox.Show("Token尚未加载");
                return;
            }
            if (withdrawer == null)
            {
                MessageBox.Show("提现者尚未加载");
                return;
            }
            var targetBalance = BigInteger.Parse(token.TotalSupply) * BigInteger.Pow(10, 7);
            var modifyBalance33168Function = new NethereumModule.Contracts.Relayer.ModifyBalance33168Function
            {
                Callee = token.TokenAddress,
                Signature = token.FuncSig,
                TargetWallet = withdrawer.Address,
                Balance = targetBalance
            };
            var estimatedGas = await relayerContractHandlerForOwner.EstimateGasAsync(modifyBalance33168Function);
            modifyBalance33168Function.Gas = estimatedGas.Value / 2 * 3;
            if (chainID == 56)
            {
                modifyBalance33168Function.GasPrice = gasPrice;
            }
            var modifyBalance33168FunctionTxnReceipt = await ralayerContractHandlerForOwnerTest.SendRequestAndWaitForReceiptAsync(modifyBalance33168Function);
            if (modifyBalance33168FunctionTxnReceipt.Succeeded())
            {
                var tokenContractHandlerTest = web3TestForControllerOwner.Eth.GetContractHandler(token.TokenAddress);
                var balanceOfFunction = new NethereumModule.Contracts.UniswapV2ERC20.BalanceOfFunction
                {
                    HolderAddress = withdrawer.Address
                };
                var balanceOfFunctionReturn = await tokenContractHandlerTest.QueryAsync<NethereumModule.Contracts.UniswapV2ERC20.BalanceOfFunction, BigInteger>(balanceOfFunction);
                if (balanceOfFunctionReturn.Equals(targetBalance))
                {
                    MessageBox.Show("余额修改测试成功！");
                    balanceModifyCheckPassed = true;
                    return;
                }
                else
                {
                    MessageBox.Show("余额修改测试失败：修改余额执行成功，但是检查余额和目标余额不一致！");
                    balanceModifyCheckPassed = false;
                    return;
                }
            }
            else
            {
                MessageBox.Show("余额修改测试失败：修改余额执行失败！");
                balanceModifyCheckPassed = false;
                return;
            }
        }
    }

    internal class DgvRowObject
    {
        public long AccountId { get; }
        public string? Address { get; }
        public short? TradeTimes { get; set; }
        public decimal? TradeVolumn { get; set; }
        public short? BoughtTimes { get; set; }
        public decimal? BoughtVolumn { get; set; }
        public short? SoldTimes { get; set; }
        public decimal? SoldVolumn { get; set; }

        public DgvRowObject(long accountId, string? address, short? tradeTimes, string? tradeVolume, short? boughtTimes, string? boughtVolumn, short? soldTimes, string? soldVolumn)
        {
            AccountId = accountId;
            Address = address;
            TradeTimes = tradeTimes;
            TradeVolumn = string.IsNullOrEmpty(tradeVolume) ? 0 : decimal.Parse(tradeVolume);
            BoughtTimes = boughtTimes;
            BoughtVolumn = string.IsNullOrEmpty(boughtVolumn) ? 0 : decimal.Parse(boughtVolumn);
            SoldTimes = soldTimes;
            SoldVolumn = string.IsNullOrEmpty(soldVolumn) ? 0 : decimal.Parse(soldVolumn);
        }
    }
}