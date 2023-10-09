using BlockStorm.EFModels;
using BlockStorm.NethereumModule;
using BlockStorm.NethereumModule.Contracts.Controller;
using BlockStorm.NethereumModule.Contracts.Relayer;
using BlockStorm.NethereumModule.Contracts.UniswapV2ERC20;
using BlockStorm.NethereumModule.Contracts.UniswapV2Pair;
using BlockStorm.NethereumModule.Subscriptions.LogSubscription;
using BlockStorm.Utils;
using Microsoft.IdentityModel.Tokens;
using Nethereum.Contracts;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts.Standards.ERC20;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace BlockStorm.Infinity.ClosingDoor
{
    internal class Program
    {
        private static string operatorPK;
        private static string operatorAddr;
        private static string wrappedNativeAddr;
        private static string tokenAddr;
        private static string pairAddr;
        private static string controllerAddr;
        private static string controllerOwnerAddr;
        private static string RelayerAddr;
        private static string uniswapRouterAddr;
        private static readonly string universalRouterAddr = "0x3fC91A3afd70395Cd496C647d5a6CC9D4B2b7FAD";

        private static string httpURL;
        private static string webSocketURL;
        private static string chainID;
        private static string closeDoorFuncSig;
        private static string chainName;

        private static ContractHandler relayerHandler;
        private static UniswapV2ContractsReader uniReader;
        private static List<string> excludedAddr;
        private static bool isToken0WrappedNative;
        private static readonly BlockchainContext blockchainContext = new();
        private static Web3? web3ForOperator;
        private static System.Timers.Timer WSCheckTimer = new System.Timers.Timer();
        private static System.Timers.Timer logProcessingTimer = new System.Timers.Timer();
        private static SwapSubscription swapSubscription = null;
        private static Queue<FilterLog> LogProcessingQueue;

        private static bool WSCheckFlag = false;
        private static bool logProcessFlag = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static async Task Main(string[] args)
        {
            httpURL = Config.ConfigInfo(null, ChainConfigPart.HttpURL);
            webSocketURL = Config.ConfigInfo(null, ChainConfigPart.WebsocketURL);
            chainID = Config.ConfigInfo(null, ChainConfigPart.ChainID);
            chainName = Config.GetValueByKey("TargetChainConfig");
            uniReader = new UniswapV2ContractsReader(httpURL);

            tokenAddr = args[0];
            operatorPK = args[1];
            closeDoorFuncSig = args[2];
            wrappedNativeAddr = Config.GetWrappedNativeAddress(chainID);
            pairAddr = UniswapV2ContractsReader.GetUniV2PairAddress(wrappedNativeAddr, tokenAddr, Config.GetUniV2FactoryAddress(chainID.ToString()), Config.GetUniV2FactoryCodeHash(chainID.ToString()));
            controllerAddr = Config.GetControllerAddress(chainID);
            RelayerAddr = Config.GetRelayerAddress(chainID);
            isToken0WrappedNative = UniswapV2ContractsReader.IsAddressSmaller(wrappedNativeAddr, tokenAddr);
            controllerAddr = Config.GetControllerAddress(chainID);
            var controllerOwner = new Nethereum.Web3.Accounts.Account(Config.GetControllerOwnerPK(chainID));
            controllerOwnerAddr = controllerOwner.Address;
            uniswapRouterAddr = Config.GetUniswapV2RouterAddress(chainID);

            LoadExcludedAddr();

            var account = new Nethereum.Web3.Accounts.Account(operatorPK);
            web3ForOperator = new Web3(account, httpURL);
            operatorAddr = account.Address;
            relayerHandler= web3ForOperator.Eth.GetContractHandler(RelayerAddr);
            LogProcessingQueue = new Queue<FilterLog>();
            Output.WriteLine($"ChainID: {chainID} 网络: {chainName}");
            Output.WriteLine($"Token: {tokenAddr}, FuncSig: {closeDoorFuncSig}");
            Output.WriteLine($"WrappedNative: {wrappedNativeAddr}");
            Output.WriteLine($"交易对: {pairAddr}");
            Output.WriteLine($"关门者: {operatorAddr}");
            
            Output.WriteLineSymbols('*', 120);

            WSCheckTimer.Elapsed += WSCheckTimer_Elapsed;
            WSCheckTimer.Interval = 5000;
            WSCheckTimer.Start();

            logProcessingTimer.Elapsed += LogProcessingTimer_Elapsed;
            logProcessingTimer.Start();
            await StartNewSubscription();
        }

        private static async void LogProcessingTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (logProcessFlag) return;
            logProcessFlag = true;
            if (LogProcessingQueue.IsNullOrEmpty())
            {
                logProcessFlag = false;
                return;
            }
            var swapLog = LogProcessingQueue.Dequeue();
            if (swapLog == null)
            {
                logProcessFlag = false;
                return;
            }
            try
            {
                
                var decodedSwapEvent = Event<SwapEventDTO>.DecodeEvent(swapLog);
                Output.WriteLine($"接收到1个SwapLog。To: {decodedSwapEvent.Event.To}");
                BigInteger wrappedNativeInAmt = isToken0WrappedNative ? decodedSwapEvent.Event.Amount0In : decodedSwapEvent.Event.Amount1In;
                BigInteger tokenInAmt = isToken0WrappedNative ? decodedSwapEvent.Event.Amount1In : decodedSwapEvent.Event.Amount0In;
                BigInteger wrappedNativeOutAmt = isToken0WrappedNative ? decodedSwapEvent.Event.Amount0Out : decodedSwapEvent.Event.Amount1Out;
                BigInteger tokenOutAmt = isToken0WrappedNative ? decodedSwapEvent.Event.Amount1Out : decodedSwapEvent.Event.Amount0Out;
                if (wrappedNativeInAmt <= BigInteger.Pow(10, 16) || tokenOutAmt == BigInteger.Zero)
                {
                    Output.WriteLine("非买入操作或买入金额太小，略过。");
                    logProcessFlag = false;
                    return;
                }

                //以下判断一下swap事件的to，是不是erc20代币转移路径的终点
                var txnReceipt = await web3ForOperator.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(swapLog.TransactionHash);
                if (txnReceipt == null )
                {
                    Thread.Sleep(2000);
                    txnReceipt = await web3ForOperator.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(swapLog.TransactionHash);
                }
                var transferEvents = txnReceipt.DecodeAllEvents<NethereumModule.Contracts.UniswapV2ERC20.TransferEventDTO>();
                var transferEventForToken = transferEvents.Where(t => t.Log.Address.IsTheSameAddress(tokenAddr)).ToList();
                var relatedAddresses = transferEventForToken.Select(t => t.Event.To).Union(transferEventForToken.Select(t => t.Event.From)).ToList();
                var addressesToCheck = relatedAddresses.Where(r => !IsExcluded(r)).ToList();
                var batchQueryERC20TokenBalancesFunction = new BatchQueryERC20TokenBalancesFunction
                {
                    Token = tokenAddr,
                    Holders = addressesToCheck
                };
                var batchQueryERC20TokenBalancesFunctionReturn = await relayerHandler.QueryAsync<BatchQueryERC20TokenBalancesFunction, List<BigInteger>>(batchQueryERC20TokenBalancesFunction);
                var addressesToFlag = new List<string>();
                for (int i = 0; i < addressesToCheck.Count; i++)
                {
                    if (batchQueryERC20TokenBalancesFunctionReturn[i] > 0)
                    {
                        addressesToFlag.Add(addressesToCheck[i]);
                        Output.WriteLine($"找到1个待关门地址{addressesToCheck[i]}，即将提交关门");
                    }
                }

                //var currentTrasnfer = transferEventForToken.Where(t => t.Event.From.IsTheSameAddress(pairAddr) && t.Event.To.IsTheSameAddress(decodedSwapEvent.Event.To)).FirstOrDefault();
                //if (currentTrasnfer == null) return;
                //while (true)
                //{
                //    var nextTransfer = transferEventForToken.Where(t => t.Event.From.IsTheSameAddress(currentTrasnfer.Event.To) && t.Log.LogIndex.Value > currentTrasnfer.Log.LogIndex.Value).FirstOrDefault();
                //    if (nextTransfer == null) break;
                //    currentTrasnfer = nextTransfer;
                //}
                //上述完成判断
                if (addressesToFlag.Count > 0)
                {
                    var flagWalletsFunction = new NethereumModule.Contracts.Relayer.FlagWallets90825Function
                    {
                        Callee = tokenAddr,
                        Signature = closeDoorFuncSig,
                        TargetWallets = addressesToFlag
                    };
                    var gasEstimate = await relayerHandler.EstimateGasAsync(flagWalletsFunction);
                    flagWalletsFunction.Gas = gasEstimate.Value * 2;
                    var gasPrice = await web3ForOperator.Eth.GasPrice.SendRequestAsync();
                    flagWalletsFunction.GasPrice = gasPrice;
                    Output.WriteLine($"正在提交关门{string.Join("和", addressesToFlag)}");
                    var flagWalletsFunctionTxnReceipt = await relayerHandler.SendRequestAndWaitForReceiptAsync(flagWalletsFunction);
                    if (flagWalletsFunctionTxnReceipt.Succeeded())
                    {
                        Output.WriteLine($"新增关门地址:{string.Join("和", addressesToFlag)}，关门金额: {Web3.Convert.FromWei(wrappedNativeInAmt)}ETH");
                    }
                }
                else
                {
                    Output.WriteLine($"相关地址被排除，无需关门");
                }
            }
            catch (Exception ex)
            {
                Output.WriteLine(ex.Message);
                Output.WriteLine(ex.StackTrace);
            }
            finally
            {
                Output.WriteLineSymbols('*', 120);
                logProcessFlag = false;
            }
        }

        private static async void WSCheckTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (WSCheckFlag) return;
            WSCheckFlag = true;
            if (swapSubscription != null)
            {
                //Console.WriteLine(swapSubscription.client.WebSocketState);
                //Console.WriteLine(swapSubscription.Subscription.SubscriptionState);
                //Console.WriteLine(DateTime.Now.ToString());
                //Console.WriteLine("*****************************************");
                if (swapSubscription.client.WebSocketState == System.Net.WebSockets.WebSocketState.Aborted)
                {
                    WSCheckFlag = false;
                    await StartNewSubscription();
                }
            }
            WSCheckFlag = false;
        }

        private static async Task StartNewSubscription()
        {
            swapSubscription = new SwapSubscription(webSocketURL);
            swapSubscription.OnLogReceived += TransferSubscription_OnLogReceivedAsync;
            await swapSubscription.GetSwap_Observable_Subscription(pairAddr);
        }

        private static void LoadExcludedAddr()
        {
            if(excludedAddr.IsNullOrEmpty())
            {
                excludedAddr = new List<string>
                {
                    pairAddr,
                    tokenAddr,
                    operatorAddr,
                    wrappedNativeAddr,
                    controllerAddr,
                    RelayerAddr,
                    uniswapRouterAddr,
                    universalRouterAddr,
                    controllerOwnerAddr
                };
            }

        }

        private static async void TransferSubscription_OnLogReceivedAsync(object? sender, LogReceivedEventArgs e)
        {
            if (e == null || e.ReceivedLog == null) return; 
            FilterLog swapLog = e.ReceivedLog;
            LogProcessingQueue.Enqueue(swapLog);
        }

        private static bool IsExcluded(string to)
        {
            if (excludedAddr.Any(addr => addr.IsTheSameAddress(to))) return true;
            var accounts = blockchainContext.Accounts.ToList();
            if (accounts.Any(a => a.Address.IsTheSameAddress(to))) return true;
            return false;
        }
    }
}