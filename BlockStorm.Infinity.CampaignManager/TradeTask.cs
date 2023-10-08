using BlockStorm.EFModels;
using BlockStorm.NethereumModule;
using BlockStorm.NethereumModule.Contracts.UniswapV2ERC20;
using BlockStorm.NethereumModule.Contracts.UniswapV2Pair;
using BlockStorm.NethereumModule.Contracts.UniswapV2Router02;
using BlockStorm.Utils;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts.Standards.ERC20.TokenList;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace BlockStorm.Infinity.CampaignManager
{
    internal class TradeTask
    {
        private static readonly string chainID = Config.ConfigInfo(null, ChainConfigPart.ChainID);
        private static readonly string httpURL = Config.ConfigInfo(null, ChainConfigPart.HttpURL);
        private static readonly string routerAddr = Config.GetUniswapV2RouterAddress(chainID);
        private static readonly string wrappedNativeAddr = Config.GetWrappedNativeAddress(chainID);
        private string pairAddr;

        public string TradeToken { get; }
        public EFModels.Account Trader { get; }
        public TradeTaskType TaskType { get; }

        public int? TradeRatio { get; set; }

        public BigInteger? TradeAmount { get; set; }

        public bool Executed { get; set; }

        public bool? Success { get; set; }

        public DateTime? ExecutionTime { get; set; }

        public int TradeInterval { get; set; }

        public TradeTask(string tradeToken, EFModels.Account trader, TradeTaskType taskType, int interval)
        {
            TradeToken = tradeToken;
            Trader = trader;
            TaskType = taskType;
            Executed = false;
            Success=null; 
            ExecutionTime = null;
            TradeInterval = interval;
            pairAddr = UniswapV2ContractsReader.GetUniV2PairAddress(wrappedNativeAddr, tradeToken, Config.GetUniV2FactoryAddress(chainID.ToString()), Config.GetUniV2FactoryCodeHash(chainID.ToString()));

        }

        internal static TradeTaskType PickTaskType(int index, int buyRatio)
        {
            if (index < 0) throw new ArgumentOutOfRangeException("index");
            if (buyRatio <= 0 || buyRatio > 100) throw new ArgumentOutOfRangeException(nameof(buyRatio));
            if (index <= 1) return TradeTaskType.buy; //头两笔100%买单
            if (index == 2) buyRatio += (100 - buyRatio) / 2; //第三笔买单概率上浮50%
            var rn = new Random();
            int luckDraw = 0;
            for (int i = 0; i < index; i++)
            {
                luckDraw = rn.Next(1,100);
            }
            if (luckDraw <= buyRatio) 
                return TradeTaskType.buy;
            else 
                return TradeTaskType.sell;
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append(TaskType == TradeTaskType.buy ? "买" : "卖");
            str.Append($" | Account ID: {Trader.Id}");
            str.Append($" | Ratio: {TradeRatio}% | ");
            str.Append(Executed ? "已执行" : "未执行");
            if (Success != null)
                str.Append($" | 执行结果: {(Success.Value ? "成功" : "失败")}");
            if (ExecutionTime != null)
                str.Append($" | 执行时间: {ExecutionTime.Value.ToString("MM-dd HH-mm-ss")}");
            return str.ToString();
        }

        internal async Task<(bool, BigInteger)> ExcuteTaskAsync(CancellationToken cancelToken, BigInteger gasPrice)
        {
            var account = new Nethereum.Web3.Accounts.Account(Trader.PrivateKey);
            var web3ForTrader = new Web3(account, httpURL);
            var wrappedNativeContractHandlerForTrader = web3ForTrader.Eth.GetContractHandler(wrappedNativeAddr);
            var tokenContractHandlerForTrader = web3ForTrader.Eth.GetContractHandler(TradeToken);
            var pairContractHandlerForTrader = web3ForTrader.Eth.GetContractHandler(pairAddr);
            var routerContractHandlerForTrader = web3ForTrader.Eth.GetContractHandler(routerAddr);
            if (TaskType == TradeTaskType.buy) //用Weth买Token
            {
                //需要先approve
                var approveFunctionForWrappedNative = new NethereumModule.Contracts.UniswapV2ERC20.ApproveFunction
                {
                    Spender = routerAddr,
                    Value = TradeAmount.Value,
                    GasPrice = gasPrice
               
                };
                var approveFunctionTxnReceipt = await wrappedNativeContractHandlerForTrader.SendRequestAndWaitForReceiptAsync(approveFunctionForWrappedNative, cancelToken);
                

                if (approveFunctionTxnReceipt.Failed())
                {
                    Success = false;
                    ExecutionTime = DateTime.Now;
                    Executed = true;
                    return (false, 0);
                }

                var getReservesOutputDTO = await pairContractHandlerForTrader.QueryDeserializingToObjectAsync<GetReservesFunction, GetReservesOutputDTO>();
                bool isToken0WrappedNative = UniswapV2ContractsReader.IsAddressSmaller(wrappedNativeAddr, TradeToken);
                (BigInteger reserveIn, BigInteger reserveOut) = isToken0WrappedNative ? (getReservesOutputDTO.Reserve0, getReservesOutputDTO.Reserve1) : (getReservesOutputDTO.Reserve1, getReservesOutputDTO.Reserve0);
                BigInteger amountOut = Util.GetAmountOutThroughSwap(TradeAmount.Value, reserveIn, reserveOut, 30);           
                var swapExactTokensForTokensFunction = new SwapExactTokensForTokensFunction
                {
                    AmountIn = TradeAmount.Value,
                    AmountOutMin = amountOut * 998 / 1000,
                    Path = new List<string>
                    {
                        wrappedNativeAddr,
                        TradeToken
                    },
                    To = Trader.Address,
                    Deadline = DateTimeOffset.UtcNow.AddSeconds(30).ToUnixTimeSeconds(),
                    GasPrice = gasPrice
                };
                var swapExactTokensForTokensFunctionTxnReceipt = await routerContractHandlerForTrader.SendRequestAndWaitForReceiptAsync(swapExactTokensForTokensFunction, cancelToken);
                if(swapExactTokensForTokensFunctionTxnReceipt.Succeeded())
                {
                    Executed = true;
                    ExecutionTime = DateTime.Now;
                    Success = true;
                    return (true, TradeAmount.Value);
                }
                else
                {
                    Success = false;
                    ExecutionTime = DateTime.Now;
                    Executed = true;
                    return (false, 0);
                }
            }
            else //卖token换回WETH
            {
                var balanceOfFunctionForToken = new NethereumModule.Contracts.UniswapV2ERC20.BalanceOfFunction
                {
                    HolderAddress = Trader.Address
                };
                var balanceOfFunctionForTokenReturn = await tokenContractHandlerForTrader.QueryAsync<NethereumModule.Contracts.UniswapV2ERC20.BalanceOfFunction, BigInteger>(balanceOfFunctionForToken);
                BigInteger amountIn = balanceOfFunctionForTokenReturn * TradeRatio.Value / 100;
                //需要先approve
                var approveFunctionForToken = new NethereumModule.Contracts.UniswapV2ERC20.ApproveFunction
                {
                    Spender = routerAddr,
                    Value = amountIn,
                    GasPrice = gasPrice
                };
                var approveFunctionTxnReceipt = await tokenContractHandlerForTrader.SendRequestAndWaitForReceiptAsync(approveFunctionForToken, cancelToken);
                if (approveFunctionTxnReceipt.Failed())
                {
                    Success = false;
                    ExecutionTime = DateTime.Now;
                    Executed = true;
                    return (false, 0);
                }

                var getReservesOutputDTO = await pairContractHandlerForTrader.QueryDeserializingToObjectAsync<GetReservesFunction, GetReservesOutputDTO>();
                bool isToken0Token = UniswapV2ContractsReader.IsAddressSmaller(TradeToken, wrappedNativeAddr);
                (BigInteger reserveIn, BigInteger reserveOut) = isToken0Token ? (getReservesOutputDTO.Reserve0, getReservesOutputDTO.Reserve1) : (getReservesOutputDTO.Reserve1, getReservesOutputDTO.Reserve0);
                BigInteger amountOut = Util.GetAmountOutThroughSwap(amountIn, reserveIn, reserveOut, 30);

                var swapExactTokensForTokensFunction = new SwapExactTokensForTokensFunction
                {
                    AmountIn = amountIn,
                    AmountOutMin = amountOut * 998 / 1000,
                    Path = new List<string>
                    {
                        TradeToken,
                        wrappedNativeAddr
                    },
                    To = Trader.Address,
                    Deadline = DateTimeOffset.UtcNow.AddSeconds(30).ToUnixTimeSeconds(),
                    GasPrice = gasPrice
                };
                var swapExactTokensForTokensFunctionTxnReceipt = await routerContractHandlerForTrader.SendRequestAndWaitForReceiptAsync(swapExactTokensForTokensFunction, cancelToken);
                if (swapExactTokensForTokensFunctionTxnReceipt.Succeeded())
                {
                    Executed = true;
                    ExecutionTime = DateTime.Now;
                    Success = true;
                    return (true, amountOut);
                }
                else
                {
                    Success = false;
                    ExecutionTime = DateTime.Now;
                    Executed = true;
                    return (false, 0);
                }
            }
        }
    }

    public enum TradeTaskType
    {
        buy,
        sell
    }
}
