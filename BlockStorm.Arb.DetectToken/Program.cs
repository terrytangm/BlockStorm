// See https://aka.ms/new-console-template for more information
using BlockStorm.Arb.Algorithm;
using BlockStorm.EFModels;
using BlockStorm.NethereumModule;
using BlockStorm.NethereumModule.Contracts.UniswapV2Router02;
using BlockStorm.Utils;
using EFCore.BulkExtensions;
using System.Numerics;

var httpURL = Config.ConfigInfo(null, ChainConfigPart.HttpURL);
var chainID = Config.ConfigInfo(null, ChainConfigPart.ChainID);
var context = new BlockchainContext();

var targetTopTokens = new List<string>
{
    "0xC02aaA39b223FE8D0A0e5C4F27eAD9083C756Cc2", //WETH
    "0xdAC17F958D2ee523a2206206994597C13D831ec7", //USDT
    "0xA0b86991c6218b36c1d19D4a2e9Eb0cE3606eB48", //USDC
    "0x6B175474E89094C44Da98b954EedeAC495271d0F" //DAI
};

var pairs= context.FilteredPairs.Where(p => p.ChainId == Convert.ToInt64(chainID) && (targetTopTokens.Contains(p.Token0) || targetTopTokens.Contains(p.Token1))).ToList();
var pairsToDetect = pairs.Where(p => !targetTopTokens.Contains(p.Token0) || !targetTopTokens.Contains(p.Token1)).ToList();

string privateKey = "0xac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80";
string walletAddress = "0xf39Fd6e51aad88F6F4ce6aB8827279cffFb92266";

int checkedPair=0;
string token0InResult = null;
string token1InResult = null;
Output.WriteLine($"------共有{pairsToDetect.Count}个交易池等待检测------");
foreach (var pairToDetect in pairsToDetect)
{
    if (targetTopTokens.Contains(pairToDetect.Token0))
    {
        (token0InResult, token1InResult) = await DetectPairAsync(pairToDetect, PairDirection.Token0InToken1Out);
    }
    else if (targetTopTokens.Contains(pairToDetect.Token1))
    {
        (token0InResult, token1InResult) = await DetectPairAsync(pairToDetect, PairDirection.Token1InToken0Out);
    }
    if (UpdatePair(pairToDetect, token0InResult, token1InResult))
    {
        var pairToUpdate = context.Pairs.FirstOrDefault(p => p.PairAddress == pairToDetect.PairAddress);
        if (pairToUpdate != null)
        {
            pairToUpdate.Token0In = pairToDetect.Token0In;
            pairToUpdate.Token1In = pairToDetect.Token1In;
            context.Update(pairToUpdate);
            context.SaveChanges();
        }
    }

    int updated = UpdateRoute(pairToDetect, PairDirection.Token0InToken1Out, (bool)pairToDetect.Token0In);
    Output.WriteLine($"已更新{updated}个0进1出方向的兑换路径");
    updated = UpdateRoute(pairToDetect, PairDirection.Token1InToken0Out, (bool)pairToDetect.Token1In);
    Output.WriteLine($"已更新{updated}个1进0出方向的兑换路径");
    Output.WriteLineSymbols('*', 120);
    Output.WriteLine($"------已检测{++checkedPair}/{pairsToDetect.Count}个交易池------");
}

bool UpdatePair(FilteredPair pairToDetect, string token0InResult, string token1InResult)
{
    bool needUpdate = false;
    if (!string.IsNullOrEmpty(token0InResult) && token0InResult == "success" && pairToDetect.Token0In != true)
    {
        pairToDetect.Token0In = true;
        needUpdate = true;
    }
    else if (!string.IsNullOrEmpty(token0InResult) && token0InResult.ToLower().Contains("smart contract error") && pairToDetect.Token0In != false)
    {
        pairToDetect.Token0In = false;
        needUpdate = true;
    }
    if (!string.IsNullOrEmpty(token1InResult) && token1InResult == "success" && pairToDetect.Token1In != true)
    {
        pairToDetect.Token1In = true;
        needUpdate = true;
    }
    else if (!string.IsNullOrEmpty(token1InResult) && token1InResult.ToLower().Contains("smart contract error") && pairToDetect.Token1In != false)
    {
        pairToDetect.Token1In = false;
        needUpdate = true;
    }
    return needUpdate;
}
int UpdateRoute(FilteredPair pairToDetect, PairDirection direction, bool value)
{
    int updated = 0;
    var context = new BlockchainContext();
    (string tokenInAddress, string tokenOutAddress) = direction == PairDirection.Token0InToken1Out ? (pairToDetect.Token0, pairToDetect.Token1) : (pairToDetect.Token1, pairToDetect.Token0);
    var routes = context.RouteNodes.Where(rn => rn.TokenIn == tokenInAddress && rn.TokenOut == tokenOutAddress).Select(rn => rn.Route).Distinct().ToList();
    if (value==false)
    {
        var routesToUpdate = routes.Where(r=>r.Enabled=true).ToList();
        foreach(var route in routesToUpdate)
        {
            route.Enabled = false;
        }
        updated = routesToUpdate.Count;
        if (updated > 0)
        {
            context.BulkUpdate<Route>(routesToUpdate);
            context.SaveChanges();
        }
    }
    else
    {
        var routesToUpdate = routes.Where(r => r.Enabled = false).ToList();
        foreach (var route in routesToUpdate)
        {
            bool enable = true;
            foreach(var routeNode in route.RouteNodes)
            {
                if (routeNode.Pair == pairToDetect.PairAddress) continue;
                var pair = context.Pairs.Find(routeNode.Pair);
                if (pair == null) continue;
                var pairEnabled = routeNode.TokenIn == pair.Token0 ? pair.Token0In : pair.Token1In;
                if(pairEnabled == false)
                {
                    enable = false;
                    break;
                }
            }
            if (enable)
            {
                route.Enabled = enable;
                updated++;
            }

        }
        if (updated > 0)
        {
            context.BulkUpdate<Route>(routesToUpdate);
            context.SaveChanges();
        }
    }
    return updated;

}

async Task<(string token0InResult, string token1InResult)> DetectPairAsync(FilteredPair pairToDetect, PairDirection direction)
{
    var uniswapV2ContractsWriter = new UniswapV2ContractsWriter(httpURL, privateKey);
    var uniswapV2ContractsReader = new UniswapV2ContractsReader(httpURL);
    var reservesDTO = await uniswapV2ContractsReader.GetReserves(pairToDetect.PairAddress);
    if (reservesDTO != null)
    {
        pairToDetect.Reserve0 = reservesDTO.Reserve0.ToString().Trim();
        pairToDetect.Reserve1 = reservesDTO.Reserve1.ToString().Trim();
    }
    (string tokenInAddress, string tokenOutAddress) = direction == PairDirection.Token0InToken1Out ? (pairToDetect.Token0, pairToDetect.Token1) : (pairToDetect.Token1, pairToDetect.Token0);
    var tokenIn = context.Tokens.Where(t => t.TokenAddress == tokenInAddress && t.ChainId == Convert.ToInt64(chainID)).First() ?? throw new Exception("Cannot Find TokenIn");
    var tokenOut = context.Tokens.Where(t => t.TokenAddress == tokenOutAddress && t.ChainId == Convert.ToInt64(chainID)).First() ?? throw new Exception("Cannot Find TokenOut");

    Output.WriteLine($"针对交易池{pairToDetect.PairAddress}展开测试");
    Output.WriteLine($"Token0: {pairToDetect.Token0}  Reserve0: {pairToDetect.Reserve0}");
    Output.WriteLine($"Token1: {pairToDetect.Token1}  Reserve1: {pairToDetect.Reserve1}");

    BigInteger amountIn = tokenIn.Symbol == "WETH" ? amountIn = BigInteger.Pow(10, 16) : BigInteger.Pow(10, (int)tokenIn.Decimals + 1);
    Output.WriteLine($"入金{Util.GetAmountDisplayWithDecimals(amountIn,(short)tokenIn.Decimals)} {tokenIn.Symbol}");


    string token0InResult, token1InResult;
    BigInteger BalanceBefore, BalanceAfter, currentBalance, reserveIn, reserveOut, actualIn, amountOut, amount0Out, amount1Out;
    //1st round
    try
    {
        await uniswapV2ContractsWriter.Transfer(tokenInAddress, pairToDetect.PairAddress, amountIn);
        (reserveIn, reserveOut) = tokenInAddress == pairToDetect.Token0 ? (BigInteger.Parse(pairToDetect.Reserve0), BigInteger.Parse(pairToDetect.Reserve1)) : (BigInteger.Parse(pairToDetect.Reserve1), BigInteger.Parse(pairToDetect.Reserve0));
        currentBalance = await uniswapV2ContractsReader.GetTokenBalanceOf(tokenInAddress, pairToDetect.PairAddress);
        actualIn = currentBalance - reserveIn;
        Output.WriteLine($"交易池实际到账数量{Util.GetAmountDisplayWithDecimals(actualIn, (short)tokenIn.Decimals)} {tokenIn.Symbol}");
        amountOut = Util.GetAmountOutThroughSwap(actualIn, reserveIn, reserveOut, pairToDetect.Fee);
        Output.WriteLine($"调用交易池的Swap获取代币{tokenOut.Name}数量为{Util.GetAmountDisplayWithDecimals(amountOut, (short)tokenOut.Decimals)} {tokenOut.Symbol}的输出");
        (amount0Out, amount1Out) = tokenInAddress == pairToDetect.Token0 ? (BigInteger.Zero, amountOut) : (amountOut, BigInteger.Zero);
        BalanceBefore = await uniswapV2ContractsReader.GetTokenBalanceOf(tokenOutAddress, walletAddress);
        await uniswapV2ContractsWriter.Swap(pairToDetect.PairAddress, amount0Out, amount1Out, walletAddress, Array.Empty<byte>());
        BalanceAfter = await uniswapV2ContractsReader.GetTokenBalanceOf(tokenOutAddress, walletAddress);
        Output.WriteLine($"实际获得了代币{tokenOut.Name}数量为{Util.GetAmountDisplayWithDecimals(BalanceAfter - BalanceBefore, (short)tokenOut.Decimals)} {tokenOut.Symbol}的输出金额");
        (token0InResult, token1InResult) = direction == PairDirection.Token0InToken1Out ? ("success", string.Empty) : (string.Empty, "success");
    }
    catch (Exception ex)
    {
        Output.WriteLine(ex.Message);
        (token0InResult, token1InResult) = direction == PairDirection.Token0InToken1Out ? (ex.Message, string.Empty) : (string.Empty, ex.Message);
        return (token0InResult, token1InResult);
    }

    Output.WriteLineSymbols('#', 20);
    reservesDTO = await uniswapV2ContractsReader.GetReserves(pairToDetect.PairAddress);
    if (reservesDTO != null)
    {
        pairToDetect.Reserve0 = reservesDTO.Reserve0.ToString().Trim();
        pairToDetect.Reserve1 = reservesDTO.Reserve1.ToString().Trim();
    }
    Output.WriteLine($"当前交易池{pairToDetect.PairAddress}的Reserve0: {pairToDetect.Reserve0} Reserve1: {pairToDetect.Reserve1}");
    Output.WriteLineSymbols('#', 20);
    Output.WriteLine($"接下去进行反向操作");
    Output.WriteLine($"向Pair入金代币{tokenOutAddress} 数量 {Util.GetAmountDisplayWithDecimals(BalanceAfter, (short)tokenOut.Decimals)} {tokenOut.Symbol}");

    //2nd round, tokenout In
    try
    {
        await uniswapV2ContractsWriter.Transfer(tokenOutAddress, pairToDetect.PairAddress, BalanceAfter);
        reservesDTO = await uniswapV2ContractsReader.GetReserves(pairToDetect.PairAddress);
        pairToDetect.Reserve0 = reservesDTO.Reserve0.ToString().Trim();
        pairToDetect.Reserve1 = reservesDTO.Reserve1.ToString().Trim();
        Output.WriteLine("");
        Output.WriteLine($"当前交易池{pairToDetect.PairAddress}的Reserve0: {pairToDetect.Reserve0} Reserve1: {pairToDetect.Reserve1}");
        Output.WriteLine("");
        currentBalance = await uniswapV2ContractsReader.GetTokenBalanceOf(tokenOutAddress, pairToDetect.PairAddress);
        Output.WriteLine($"当前pair持有的{tokenOutAddress}代币余额为{Util.GetAmountDisplayWithDecimals(currentBalance, (short)tokenOut.Decimals)} {tokenOut.Symbol}");
        (reserveIn, reserveOut) = tokenInAddress == pairToDetect.Token0 ? (BigInteger.Parse(pairToDetect.Reserve1), BigInteger.Parse(pairToDetect.Reserve0)) : (BigInteger.Parse(pairToDetect.Reserve0), BigInteger.Parse(pairToDetect.Reserve1));
        actualIn = currentBalance - reserveIn;
        Output.WriteLine($"交易池实际到账代币{tokenOutAddress} 数量 {Util.GetAmountDisplayWithDecimals(actualIn, (short)tokenOut.Decimals)} {tokenOut.Symbol}");
        amountOut = Util.GetAmountOutThroughSwap(actualIn, reserveIn, reserveOut, pairToDetect.Fee);
        Output.WriteLine($"调用交易池的Swap获取代币{tokenInAddress}数量为{amountOut}的输出");
        (amount0Out, amount1Out) = tokenInAddress != pairToDetect.Token0 ? (BigInteger.Zero, amountOut) : (amountOut, BigInteger.Zero);
        BalanceBefore = await uniswapV2ContractsReader.GetTokenBalanceOf(tokenInAddress, walletAddress);
        await uniswapV2ContractsWriter.Swap(pairToDetect.PairAddress, amount0Out, amount1Out, walletAddress, Array.Empty<byte>());
        BalanceAfter = await uniswapV2ContractsReader.GetTokenBalanceOf(tokenInAddress, walletAddress);
        Output.WriteLine($"实际获得了代币{tokenInAddress}数量为{Util.GetAmountDisplayWithDecimals(BalanceAfter - BalanceBefore, (short)tokenIn.Decimals)} {tokenIn.Symbol}的输出金额");
        if (BalanceAfter - BalanceBefore - amountIn > 0)
        {
            Output.WriteLine($"产生了{Util.GetAmountDisplayWithDecimals(BalanceAfter - BalanceBefore - amountIn, (short)tokenIn.Decimals)} {tokenIn.Symbol}的利润");
        }
        (token0InResult, token1InResult) = direction == PairDirection.Token0InToken1Out ? (token0InResult, "success") : ("success", token1InResult);
    }
    catch (Exception ex) 
    {
        Output.WriteLine(ex.Message);
        (token0InResult, token1InResult) = direction == PairDirection.Token0InToken1Out ? (token0InResult, ex.Message) : (ex.Message, token1InResult);
        return (token0InResult, token1InResult);
    }
    return (token0InResult, token1InResult);
}