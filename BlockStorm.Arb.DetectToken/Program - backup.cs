// See https://aka.ms/new-console-template for more information
using BlockStorm.Arb.Algorithm;
using BlockStorm.EFModels;
using BlockStorm.NethereumModule;
using BlockStorm.NethereumModule.Contracts.UniswapV2Router02;
using BlockStorm.Utils;
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

var pairsToDetect = new List<string>
{
    //"0x611CBD6744Fa310A1A75FAD66261b90305E998E1",
    "0x7F8967E37bEC10C50A294436AA4F0282E9c34143",
    "0xE3d3551BB608e7665472180a20280630D9E938aa",
    "0x57B8d6d008aE245610DbFca0BFc1F805D3d19C64",
    "0x1498bd576454159Bb81B5Ce532692a8752D163e8",
    "0x7D7E813082eF6c143277c71786e5bE626ec77b20",
    "0x3203d789bfdB222bfdd629B8de7C5Dc38e8241eC",
    "0x99e2Bd6f2Fd5086dc18f5b25a97770D1C407F812",
    "0xCb2286d9471cc185281c4f763d34A962ED212962",
    "0x59F96b8571E3B11f859A09Eaf5a790A138FC64D0",
    "0x95F8eA94C3b5aD4A30a2CCdD393641843E91FdE4",
    "0xCb2286d9471cc185281c4f763d34A962ED212962",
    "0x550301E3E18009E67A115f9fA067f9D7A62073df",
    "0xA2f5F7F7d8b3dEbFB3649f5Dc53c86B13cB3F6C9",
    "0x2cC846fFf0b08FB3bFfaD71f53a60B4b6E6d6482",
    "0x7c1C4a2Cf81D2fC83b89bfD34F4d2c7e90044b32",
    "0x2E72a375Af522b5EDD91cf50eDFf101ac1ef8Cc1",
    "0x6A8033E3c64cF50fB02D26D6FedDc0DB572a493a",
    "0xC05371628F77cd274D111EC9621B5D85f5945da2",
    "0x8D99e3c31b69E7B2E042728AE86913A208733a7a",
    "0x632846B457Ca00176bbb01D029F3da4BB3Ef6CFA",
    "0x6fce1254AbF7B3bFde1880acDC2Ee696c2F7F064",
    "0xcB525DA7F6e8E990Ba2C31ba09e1E3078FA0ACE2",
    "0x77552f5f1029C2759c6F250F64D7276BF53c6De1",
    "0x60EF1e0Bf9218Cdc1769a43c4B0B111ed38BB418",
    "0x5DC041b3F3c3b44971Af6b5149692B1e0563c21f",
    "0x84B731c44B773c29BBAFe33f95981B1b94F55B1c",
    "0x92FFe72EE8A6a3DF28d18d6CA01a8e17ADF608F0",
    "0x5660c518c5610493086A3BA550f7ad6EB7935d1E",
    "0xcFB8CF118B4F0ABB2E8CE6DBEb90D6bC0A62693D",
    "0xAbC7D601982B1fF279965A2d0Db19b39dB4F39CA",
    "0x9BF92f4e176C2AA3c439375AE49522a4367651C4",
    "0x4417d89CB2230B3a8f32180a3cBE0A48100e5491",
    "0xCB4F0868a2f5bE4246614B064CF5C631d74ff57a",
    "0x00170a7EDE49642cd6EbAF3BDd9e18B52f39E56B",
    "0x05F4E094c8F77D001a05dc7FFEf208CCbEA47079",
    "0x52c77b0CB827aFbAD022E6d6CAF2C44452eDbc39",
    "0x1058E2644E066031B7b4a76a4bB42208838eF938",
    "0x3Daf585e0922823f390c6120041aC4BE8EdAdbA7",
    "0x79Aec2604855D6EdDc26DD558eC9a91A630b2997"
};

string privateKey = "0xf23687d9e3e42e92e3548f823c143ec8b9a80e0be04dbb09ce62d443f13c31ad";
string walletAddress = "0x1d47229e42631C80B25899E04e70011a5908C95f";

//string uniswapDelegate = "0x15BB2cc3Ea43ab2658F7AaecEb78A9d3769BE3cb";

var tokensEnable = new Dictionary<string, bool>();
var topTokens = context.Tokens.Where(t => t.IsTopToken == true && t.ChainId == Convert.ToInt64(chainID)).ToList();
foreach (var token in topTokens)
{
    tokensEnable.Add(token.TokenAddress, true);
}

//var routesToDetect = context.Routes.Where(t => t.TokenIn == "0xA0b86991c6218b36c1d19D4a2e9Eb0cE3606eB48" && t.OptimalProfit.Length >= 7).ToList();
int checkedPair=0;
//foreach (var routeToDetect in routesToDetect)
//{
    //var pairAddresses = context.RouteNodes.Where(r => r.RouteId == routeToDetect.Id).Select(rn => rn.Pair).ToList();
    //if (pairAddresses == null) continue;
    foreach (var pairAddress in pairsToDetect)
    {
        var pair = context.Pairs.Where(p => p.PairAddress == pairAddress).FirstOrDefault();
        if (pair == null) continue;
        if (!tokensEnable.ContainsKey(pair.Token0))
        {
            tokensEnable.Add(pair.Token0, await DetectTokenAsync(pair.Token0));
        }
        if (!tokensEnable.ContainsKey(pair.Token1))
        {
            tokensEnable.Add(pair.Token1, await DetectTokenAsync(pair.Token1));
        }
    }
    Output.WriteLine($"------已检测{++checkedPair}/{pairsToDetect.Count}个交易池------");
//}

async Task<bool> DetectTokenAsync(string tokenToDetectAddress)
{
    Output.WriteLine($"目前检测代币{tokenToDetectAddress}");
    FilteredPair pairToDetect = null;
    if (tokenToDetectAddress == "0xd25a99AA41177394AA4eAaae69b3FE6477a6eE25")
    {
        pairToDetect = context.FilteredPairs.Where(fp => fp.PairAddress == "0x7F8967E37bEC10C50A294436AA4F0282E9c34143").FirstOrDefault();
    }
    else
    {
        foreach (string token in targetTopTokens)
        {
            var pairTemp = context.FilteredPairs.Where(p => p.ChainId == Convert.ToInt64(chainID)
                                                                      && (p.Token0 == token && p.Token1 == tokenToDetectAddress || p.Token1 == token && p.Token0 == tokenToDetectAddress)).FirstOrDefault();
            if (pairTemp != null)
            {
                pairToDetect = pairTemp;
                break;
            }
        }
    }
    if (pairToDetect == null)
    {
        Output.WriteLine($"找不到测试对，先返回true");
        Output.WriteLineSymbols('*', 120);
        return true;
    }
    var uniswapV2ContractsWriter = new UniswapV2ContractsWriter(httpURL, privateKey);
    var uniswapV2ContractsReader = new UniswapV2ContractsReader(httpURL);
    var reservesDTO = await uniswapV2ContractsReader.GetReserves(pairToDetect.PairAddress);
    pairToDetect.Reserve0 = reservesDTO.Reserve0.ToString().Trim();
    pairToDetect.Reserve1 = reservesDTO.Reserve1.ToString().Trim();

    Output.WriteLine($"针对交易池{pairToDetect.PairAddress}展开测试");
    Output.WriteLine($"Token0: {pairToDetect.Token0}  Reserve0: {pairToDetect.Reserve0}");
    Output.WriteLine($"Token1: {pairToDetect.Token1}  Reserve1: {pairToDetect.Reserve1}");



    // tokenInAddress: 入金币种，也是稳定币(USDT, USDC, DAI)，或者WETH
    var tokenInAddress = pairToDetect.Token0 == tokenToDetectAddress ? pairToDetect.Token1 : pairToDetect.Token0;
    var tokenIn = context.Tokens.Where(t => t.TokenAddress == tokenInAddress && t.ChainId == Convert.ToInt64(chainID)).First() ?? throw new Exception("Cannot Find TokenIn");
    //amountIn 入金数额
    BigInteger amountIn; 
    if (tokenIn.Symbol == "WETH")
    {
        amountIn = BigInteger.Pow(10, 16);//0.01WETH
        Output.WriteLine($"入金0.01WETH");
    }
    else
    {
        amountIn = BigInteger.Pow(10, (int)tokenIn.Decimals+1); //10刀
        Output.WriteLine($"入金10刀");
    }


    bool enabled;
    try
    {
        await uniswapV2ContractsWriter.Transfer(tokenInAddress, pairToDetect.PairAddress, amountIn);
        (BigInteger reserveIn, BigInteger reserveOut) = tokenInAddress == pairToDetect.Token0 ? (BigInteger.Parse(pairToDetect.Reserve0), BigInteger.Parse(pairToDetect.Reserve1)) : (BigInteger.Parse(pairToDetect.Reserve1), BigInteger.Parse(pairToDetect.Reserve0));
        BigInteger currentBalance = await uniswapV2ContractsReader.GetTokenBalanceOf(tokenInAddress, pairToDetect.PairAddress);
        BigInteger actualIn = currentBalance - reserveIn;
        Output.WriteLine($"交易池实际到账数量{actualIn}");
        BigInteger amountOut = Util.GetAmountOutThroughSwap(actualIn, reserveIn, reserveOut, pairToDetect.Fee);
        Output.WriteLine($"调用交易池的Swap获取代币{tokenToDetectAddress}数量为{amountOut}的输出");
        (BigInteger amount0Out, BigInteger amount1Out) = tokenInAddress == pairToDetect.Token0 ? (BigInteger.Zero, amountOut) : (amountOut, BigInteger.Zero);
        BigInteger BalanceBefore = await uniswapV2ContractsReader.GetTokenBalanceOf(tokenToDetectAddress, walletAddress);
        await uniswapV2ContractsWriter.Swap(pairToDetect.PairAddress, amount0Out, amount1Out, walletAddress, Array.Empty<byte>());
        //await uniswapV2ContractsWriter.SwapThroughDelegate(uniswapDelegate, pairToDetect.PairAddress, amount0Out, amount1Out, walletAddress, Array.Empty<byte>());
        BigInteger BalanceAfter = await uniswapV2ContractsReader.GetTokenBalanceOf(tokenToDetectAddress, walletAddress);
        Output.WriteLine($"实际获得了代币{tokenToDetectAddress}数量为{BalanceAfter- BalanceBefore}的输出金额");

        Output.WriteLineSymbols('#', 20);
        reservesDTO = await uniswapV2ContractsReader.GetReserves(pairToDetect.PairAddress);
        pairToDetect.Reserve0 = reservesDTO.Reserve0.ToString().Trim();
        pairToDetect.Reserve1 = reservesDTO.Reserve1.ToString().Trim();
        Output.WriteLine($"当前交易池{pairToDetect.PairAddress}的Reserve0: {pairToDetect.Reserve0} Reserve1: {pairToDetect.Reserve1}");
        Output.WriteLineSymbols('#', 20);


        Output.WriteLine($"接下去进行反向操作");
        Output.WriteLine($"向Pair入金代币{tokenToDetectAddress} 数量 {BalanceAfter}");
        await uniswapV2ContractsWriter.Transfer(tokenToDetectAddress, pairToDetect.PairAddress, BalanceAfter);
        reservesDTO = await uniswapV2ContractsReader.GetReserves(pairToDetect.PairAddress);
        pairToDetect.Reserve0 = reservesDTO.Reserve0.ToString().Trim();
        pairToDetect.Reserve1 = reservesDTO.Reserve1.ToString().Trim();
        Output.WriteLine("");
        Output.WriteLine($"当前交易池{pairToDetect.PairAddress}的Reserve0: {pairToDetect.Reserve0} Reserve1: {pairToDetect.Reserve1}");
        Output.WriteLine("");
        currentBalance = await uniswapV2ContractsReader.GetTokenBalanceOf(tokenToDetectAddress, pairToDetect.PairAddress);
        Output.WriteLine($"当前pair持有的{tokenToDetectAddress}代币余额为{currentBalance}");
        (reserveIn, reserveOut) = tokenInAddress == pairToDetect.Token0 ? (BigInteger.Parse(pairToDetect.Reserve1), BigInteger.Parse(pairToDetect.Reserve0)) : (BigInteger.Parse(pairToDetect.Reserve0), BigInteger.Parse(pairToDetect.Reserve1));
        actualIn = currentBalance - reserveIn;
        Output.WriteLine($"交易池实际到账代币{tokenToDetectAddress} 数量 {actualIn}");
        amountOut = Util.GetAmountOutThroughSwap(actualIn, reserveIn, reserveOut, pairToDetect.Fee);
        Output.WriteLine($"调用交易池的Swap获取代币{tokenInAddress}数量为{amountOut}的输出");
        (amount0Out, amount1Out) = tokenInAddress != pairToDetect.Token0 ? (BigInteger.Zero, amountOut) : (amountOut, BigInteger.Zero);
        BalanceBefore = await uniswapV2ContractsReader.GetTokenBalanceOf(tokenInAddress, walletAddress);
        await uniswapV2ContractsWriter.Swap(pairToDetect.PairAddress, amount0Out, amount1Out, walletAddress, Array.Empty<byte>());
        //await uniswapV2ContractsWriter.SwapThroughDelegate(uniswapDelegate,pairToDetect.PairAddress, amount0Out, amount1Out, walletAddress, Array.Empty<byte>());
        BalanceAfter = await uniswapV2ContractsReader.GetTokenBalanceOf(tokenInAddress, walletAddress);
        Output.WriteLine($"实际获得了代币{tokenInAddress}数量为{BalanceAfter - BalanceBefore}的输出金额");
        enabled = true;
    }
    catch (Exception ex)
    {
        Output.WriteLine(ex.Message);
        enabled= false;
    }
    Output.WriteLineSymbols('*', 120);
    return enabled;
}