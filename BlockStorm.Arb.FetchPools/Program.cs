using BlockStorm.EFModels;
using BlockStorm.Utils;
using BlockStorm.NethereumModule;
using System.Configuration;
using System.Numerics;
using EFCore.BulkExtensions;

var chainID = Config.ConfigInfo(null, ChainConfigPart.ChainID);
var httpURL = Config.ConfigInfo(null, ChainConfigPart.HttpURL);

using var context = new BlockchainContext();
var dexes = context.Dexes.Where(d => d.ChainId.ToString() == chainID).ToList();
if (dexes.Count == 0) return;

if (httpURL == null) return;
var uniswapV2Reader = new UniswapV2ContractsReader(httpURL);

foreach (var dex in dexes)
{
    while (true)
    {
        int? maxPairIndex = context.Pairs.Where(p => p.DexName == dex.DexName).Max(pair => pair.PairIndex);
        int storedPairsCount = (maxPairIndex is null) ? 0 : maxPairIndex.Value + 1;
        BigInteger onChainPairsCount = await uniswapV2Reader.GetAllPairsLength(dex.Factory);
        Output.WriteLine(DateTime.Now.ToString());
        Output.WriteLine($"Dex Name: {dex.DexName}. 当前本地已有池子{storedPairsCount}个;链上池子总数{onChainPairsCount}个");
        Output.WriteLine($"池子增量{onChainPairsCount - storedPairsCount}个");
        if (onChainPairsCount <= storedPairsCount)
        {
            Output.WriteLine("**********************************************************************************");
            break;
        }


        Output.WriteLine($"开始获取增量池子，同时继续获取新增币种......");
        List<Pair> pairsToAdd = new();
        List<Token> tokensToAdd = new();
        HashSet<string> tokensHash = new();
        (int left, int top) = Console.GetCursorPosition();
        for (int i = storedPairsCount; i < onChainPairsCount /*&& i< storedPairsCount+200*/; i++)
        {
            string pairAddress = await uniswapV2Reader.GetPairAddressByIndex(dex.Factory, i);
            var getReservesOutputDTO = await uniswapV2Reader.GetReserves(pairAddress);
            Pair pair = new()
            {
                PairIndex = i,
                PairAddress = pairAddress,
                ChainId = dex.ChainId,
                DexName = dex.DexName,
                Created = DateTime.Now,
                LastUpdate = DateTime.Now,
                Token0 = await uniswapV2Reader.GetToken0Address(pairAddress),
                Token1 = await uniswapV2Reader.GetToken1Address(pairAddress),
                Reserve0 = getReservesOutputDTO.Reserve0.ToString().Trim(),
                Reserve1 = getReservesOutputDTO.Reserve1.ToString().Trim(),
                BlockTimeLast = Convert.ToInt32(getReservesOutputDTO.BlockTimestampLast),
                Fee = dex.Fee,
                Token0In = true,
                Token1In = true
            };
            pairsToAdd.Add(pair);

            var token0 = context.Tokens.Find(pair.Token0, pair.ChainId);

            if (token0 == null && !tokensHash.Contains(pair.Token0)) //如果数据库没有这个token，且这个token不在待添加队列中，则添加这个token
            {
                token0 = await uniswapV2Reader.GetTokenModelByAddress(pair.Token0, dex.ChainId);
                tokensToAdd.Add(token0);
                tokensHash.Add(pair.Token0);
            }
            var token1 = context.Tokens.Find(pair.Token1, pair.ChainId);
            if (token1 == null && !tokensHash.Contains(pair.Token1))
            {
                token1 = await uniswapV2Reader.GetTokenModelByAddress(pair.Token1, dex.ChainId);
                tokensToAdd.Add(token1);
                tokensHash.Add(pair.Token1);
            }
            Console.Write($"已获取第{i - storedPairsCount + 1}个池子;有{tokensToAdd.Count}个新增币种");
            Console.SetCursorPosition(left, top);
        }
        Output.WriteLine("");
        Output.WriteLine($"向数据库存入{pairsToAdd.Count}个池子");
        context.BulkInsert<Pair>(pairsToAdd);
        Output.WriteLine($"向数据库存入{tokensToAdd.Count}个币种");
        context.BulkInsert<Token>(tokensToAdd);
        Output.WriteLine($"向数据库存入{pairsToAdd.Count}个池子和{tokensToAdd.Count}个币种成功!");
        Output.WriteLineSymbols('*', 120);
    }
}
Console.ReadLine();
