// See https://aka.ms/new-console-template for more information
using BlockStorm.Arb.Algorithm;
using BlockStorm.EFModels;
using BlockStorm.NethereumModule;
using BlockStorm.NethereumModule.Contracts.LoopArbitrage;
using BlockStorm.NethereumModule.Contracts.UniswapV2Pair;
using BlockStorm.Utils;
using Microsoft.EntityFrameworkCore;
using Nethereum.Contracts;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Contracts.Standards.ERC20.TokenList;
using Nethereum.Geth.RPC.Debug;
using Nethereum.Geth.RPC.Debug.DTOs;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.Web3;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Eac;
using System.Collections;
using System.Configuration;
using System.Numerics;
using System.Security.Policy;
using System.Text;

string privateKey = "0xaa43766d2f8131c33a563c6ed4dcb054df9cf866fa8d4ac3c4b804793ebecd5a";
string walletAddress = "0xF5d8bb4EBA463643C53E1C3A3A120c46ed16702c";
string arbitrageContract = "0xd0E3b9bc415aB6Dd285C9Ee72DcAeaFB952F963F";
string WETH = "0xC02aaA39b223FE8D0A0e5C4F27eAD9083C756Cc2";
var targetTopTokens = new List<string>
{
    WETH,
   //"0xdAC17F958D2ee523a2206206994597C13D831ec7"//USDT
    //"0xA0b86991c6218b36c1d19D4a2e9Eb0cE3606eB48" //USDC
    //"0x6B175474E89094C44Da98b954EedeAC495271d0F", //DAI
    //"0x2260FAC5E5542a773Aa44fBCfeDf7C193bc2C599", //WBTC
    //"0x514910771AF9Ca656af840dff83E8264EcF986CA" //Link
    //"0x1f9840a85d5aF5bf1D1762F925BDADdC4201F984" //UNI
    //"0x95aD61b0a150d79219dCF64E1E6Cc01f0B64C4cE" //Shib
};

var dexesForFlashloan = new List<string>
{
    "Uniswap V2",
    "Pancake",
    "RadioShark",
    "PepeDex",
    "Xchange",
    "Taal"
};

var httpURL = Config.ConfigInfo(null, ChainConfigPart.HttpURL);
var chainID = Config.ConfigInfo(null, ChainConfigPart.ChainID);
var account = new Nethereum.Web3.Accounts.Account(privateKey);
var web3 = new Web3(account, httpURL);
var client = new RpcClient(new Uri(httpURL));
var debugTraceCall = new DebugTraceCall(client);
JObject? debugResult = null;

var arbContract = web3.Eth.GetContractHandler(arbitrageContract);
var blockChainContext = new BlockchainContext();
var ethPriceUSDT = blockChainContext.Tokens.Find(WETH, long.Parse(chainID)).PriceUsdt;
//long blockNumber = blockChainContext.SyncReserveLogs.Max(s => s.BlockNumber).GetValueOrDefault(0);
//var pairAddresses = blockChainContext.SyncReserveLogs.Where(s => s.BlockNumber == blockNumber).Select(rs => rs.PairAddress).Distinct().ToList();
//var routeIDs = blockChainContext.RouteNodes.Where(r => pairAddresses.Contains(r.Pair)).Select(rn => rn.Id);
var routes = blockChainContext.Routes.Where(r => r.Enabled == true && targetTopTokens.Contains(r.TokenIn)).ToList();
Util.ListRandom<Route>(routes);

Output.WriteLine($"有{routes.Count}个待测试的兑换路径");

BigInteger optimalInput, optimalOutput, optimalProfit, balanceBefore, balanceAfter, gasInTargetTokenWei;
BigInteger simulatedGas, simulatedReturn;
decimal simulatedGasInEth, simulatedProfit, simulatedProfitUSDT, simulatedGasUSDT, finalProfitUSDT, gasInTargetTokenWeiDecimal;



for (int i = 0; i < routes.Count; i++)
{
    Output.WriteLine($"兑换路径ID: {routes[i].Id} 当前进度{i}/{routes.Count}");
    var swapFees = new List<BigInteger>();
    var pairAddressList = new List<string>();
    var pairList = new List<FilteredPair>();
    var tokenList = new List<string>
    {
        routes[i].TokenIn
    };
    var routeNodes = blockChainContext.RouteNodes.Where(rn => rn.RouteId == routes[i].Id).OrderBy(rn => rn.PairRank).ToList();
    for (int j = 0; j < routeNodes.Count; j++)
    {
        var pair = blockChainContext.Pairs.Find(routeNodes[j].Pair);
        pairList.Add(Pair.ToFilteredPair(pair));
        swapFees.Add((BigInteger)pair.Fee);
        pairAddressList.Add(pair.PairAddress);
        tokenList.Add(routeNodes[j].TokenOut);
        //display&log
        var stringBuilder = new StringBuilder();
        stringBuilder.Append($"交易所: {pair.DexName} 交易池：{pair.PairAddress} 币种: ");
        stringBuilder.Append($"{routeNodes[j].TokenIn} => {routeNodes[j].TokenOut}");
        Output.WriteLine(stringBuilder.ToString());
    }
    (BigInteger Ea, BigInteger Eb) = FilterPairs.GetEaEb(pairList, tokenList);
    var targetToken = blockChainContext.Tokens.Find(routes[i].TokenIn, Convert.ToInt64(chainID));
    Output.WriteLine($"Ea: {Ea}");
    Output.WriteLine($"Eb: {Eb}");
    if (Ea == 0 || Eb == 0)
    {
        Output.WriteLine("此路径流动性不满足条件");
        Output.WriteLineSymbols('*', 120);
        continue;
    }
    if (Eb * (10000 - swapFees[0]) > Ea * 10000)
    {
        (optimalInput, optimalOutput, optimalProfit) = FilterPairs.GetOptimalInput(Ea, Eb, (short)swapFees[0]);
        if (optimalProfit < Web3.Convert.ToWei("0.0025"))
        {
            Output.WriteLine("计算无套利机会");
            Output.WriteLineSymbols('*', 120);
            continue;
        }
        Output.WriteLine("计算有套利机会");
    }
    else
    {
        Output.WriteLine("计算无套利机会");
        Output.WriteLineSymbols('*', 120);
        continue;
        //optimalInput = BigInteger.Parse(targetToken.LowestReserve);
        //optimalOutput = Util.GetAmountOutThroughSwap(optimalInput, Ea, Eb, (short)swapFees[0]);
        //optimalProfit = optimalOutput - optimalInput;
    }
    var tokenInContract = web3.Eth.GetContractHandler(routes[i].TokenIn);
    var balanceOfFunction = new BalanceOfFunction
    {
        HolderAddress = arbitrageContract
    };
    balanceBefore = await tokenInContract.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction);
    if(optimalInput > balanceBefore)
    {
        optimalInput = balanceBefore;
        optimalOutput = Util.GetAmountOutThroughSwap(optimalInput, Ea, Eb, (short)swapFees[0]);
        optimalProfit = optimalOutput - optimalInput;
    }
    Output.WriteLine($"输入金额:{Util.GetAmountDisplayWithDecimals(optimalInput, (short)targetToken.Decimals)} {targetToken.Symbol}");
    Output.WriteLine($"输出金额:{Util.GetAmountDisplayWithDecimals(optimalOutput, (short)targetToken.Decimals)} {targetToken.Symbol}");
    Output.WriteLine($"计算利润:{Util.GetAmountDisplayWithDecimals(optimalProfit, (short)targetToken.Decimals)} {targetToken.Symbol}");


    var amountOutMin = 1; //optimalInput + optimalProfit * 50 / 100;
    //直接调用套利函数
    var loopArbitrageV1Function = new LoopArbitrageV1DirectCallFunction
    {
        AmountIn = optimalInput,
        AmountOutMin = amountOutMin,
        Token = routes[i].TokenIn,
        SwapFees = swapFees,
        Pairs = pairAddressList
    };

    var callInput = loopArbitrageV1Function.CreateCallInput<LoopArbitrageV1DirectCallFunction>(arbitrageContract);
    callInput.From = walletAddress;
    callInput.To = arbitrageContract;
    var tco = new TraceCallOptions();
    var currentBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
    try
    {
        debugResult = await debugTraceCall.SendRequestAsync(callInput, currentBlockNumber.Value.ToString(), tco);
    }
    catch (Exception ex)
    {
        Output.WriteLine(ex.Message);
    }
    if (debugResult["failed"].ToString().ToLower() == "true")
    {
        Output.WriteLine("模拟运行出错");
        Output.WriteLineSymbols('*', 120);
        continue;
    }
    var gas = debugResult["gas"].ToString();
    var returnValue = debugResult["returnValue"].ToString();
    simulatedGas = BigInteger.Parse("0" + gas.Substring(2), System.Globalization.NumberStyles.HexNumber);
    simulatedReturn = BigInteger.Parse(returnValue.Substring(2), System.Globalization.NumberStyles.AllowHexSpecifier);
    var gasPriceRaw = await web3.Eth.GasPrice.SendRequestAsync();
    var gasPriceGwei = Web3.Convert.FromWei(gasPriceRaw, Web3.Convert.GetEthUnitValue(Nethereum.Util.UnitConversion.EthUnit.Gwei));
    Output.WriteLine("");
    Output.WriteLine($"模拟输出:{Util.GetAmountDisplayWithDecimals(simulatedReturn, (short)targetToken.Decimals)} {targetToken.Symbol}");
    simulatedProfit = Convert.ToDecimal(Util.GetAmountDisplayWithDecimals(simulatedReturn - optimalInput, (short)targetToken.Decimals));
    simulatedProfitUSDT = simulatedProfit * targetToken.PriceUsdt.Value;
    Output.WriteLine($"模拟利润:{simulatedProfit} {targetToken.Symbol} 模拟利润-U {simulatedProfitUSDT}USDT");
    simulatedGasInEth = Web3.Convert.FromWei(simulatedGas * gasPriceRaw);
    Output.WriteLine($"模拟Gas数量: {simulatedGas}，Gas价格{gasPriceGwei}Gwei 模拟Gas额: {simulatedGasInEth}ETH");
    simulatedGasUSDT = ethPriceUSDT.Value * simulatedGasInEth;
    Output.WriteLine($"模拟GAS转化为U: {simulatedGasUSDT}USDT");
    finalProfitUSDT = simulatedProfitUSDT - simulatedGasUSDT;
    Output.WriteLine("");
    Output.WriteLine($"最终利润: {finalProfitUSDT:0.##}USDT");



    if (finalProfitUSDT < 0)
    {
        Output.WriteLineSymbols('*', 120);
        continue;
    }
    Output.WriteLine("");
    Output.WriteLine("有盈利空间，实行生产环境套利。");
    gasInTargetTokenWeiDecimal = simulatedGasUSDT / targetToken.PriceUsdt.Value * Convert.ToDecimal(Math.Pow(10, (double)targetToken.Decimals));
    gasInTargetTokenWei = (BigInteger)gasInTargetTokenWeiDecimal;

    try
    {
        loopArbitrageV1Function.AmountOutMin = optimalInput;// + gasInTargetTokenWei;
        var loopArbitrageV1FunctionTxnReceipt = await arbContract.SendRequestAndWaitForReceiptAsync(loopArbitrageV1Function);
        balanceAfter = await tokenInContract.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction);
        Output.WriteLine($"实际执行利润:{Util.GetAmountDisplayWithDecimals(balanceAfter - balanceBefore, (short)targetToken.Decimals)} {targetToken.Symbol}");
        var spentGas = loopArbitrageV1FunctionTxnReceipt.GasUsed.Value * loopArbitrageV1FunctionTxnReceipt.EffectiveGasPrice.Value;
        Output.WriteLine($"实际消耗Gas:{Util.GetAmountDisplayWithDecimals(spentGas, 18)} ETH");
    }
    catch (Exception ex)
    {
        Output.WriteLine(ex.Message);
    }
    /*闪电贷
    var pairToLoan = blockChainContext.Pairs.Where(p => ((p.Token0 == targetToken.TokenAddress && p.Reserve0.Length > optimalInput.ToString().Trim().Length)
                                                                                     || (p.Token1 == targetToken.TokenAddress && p.Reserve1.Length > optimalInput.ToString().Trim().Length))
                                                                                     && (!pairAddressList.Contains(p.PairAddress)) && dexesForFlashloan.Contains(p.DexName)).OrderBy(p => p.Fee).First();


    if (pairToLoan != null)
    {
        var flashloanUniswapV2Function = new FlashloanUniswapV2Function
        {
            TokenBorrow = targetToken.TokenAddress,
            PairBorrow = pairToLoan.PairAddress,
            LoanFee = (BigInteger)pairToLoan.Fee,
            Amount = optimalInput,
            AmountOutMin = amountOutMin,
            SwapFees = swapFees,
            Pairs = pairAddressList,
            DexNameForLoan = pairToLoan.DexName
            
        };

        var tokenInContract = web3.Eth.GetContractHandler(routes[i].TokenIn);
        var balanceOfFunction = new BalanceOfFunction
        {
            HolderAddress = arbitrageContract
        }; 
        try
        {
            var balanceBefore = await tokenInContract.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction);
            //var gasInWei = await web3Util.GetLoopArbitrageGas(arbitrageContract, tokenIn, pairToLoan.PairAddress, (BigInteger)pairToLoan.Fee, optimalInput, amountOutMin, swapFees, pairAddressList);
            //Output.WriteLine($"预估Gas: {Util.GetAmountDisplayWithDecimals(gasInWei, 18)} ETH");
            var flashloanUniswapV2FunctionTxnReceipt = await arbContract.SendRequestAndWaitForReceiptAsync(flashloanUniswapV2Function);
            var balanceAfter = await tokenInContract.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction);
            Output.WriteLine($"实际执行利润:{Util.GetAmountDisplayWithDecimals(balanceAfter - balanceBefore, (short)targetToken.Decimals)} {targetToken.Symbol}");
            var spentGas = flashloanUniswapV2FunctionTxnReceipt.GasUsed.Value * flashloanUniswapV2FunctionTxnReceipt.EffectiveGasPrice.Value;
            Output.WriteLine($"实际消耗Gas:{Util.GetAmountDisplayWithDecimals(spentGas, 18)} ETH");
        }
        catch (Exception ex)
        {
            Output.WriteLine(ex.Message);
        }
    }
    else
    {
        Output.WriteLine("没有找到可以提供闪电贷的交易池!");
    }*/
    Output.WriteLineSymbols('*', 120);
}//end for(every completed path)

Console.ReadLine();
