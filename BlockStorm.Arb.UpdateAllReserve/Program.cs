// See https://aka.ms/new-console-template for more information
using BlockStorm.EFModels;
using BlockStorm.NethereumModule;
using BlockStorm.Utils;
using System.Configuration;
using System.Linq;

var httpURL = Config.ConfigInfo(null, ChainConfigPart.HttpURL);
var chainID = Config.ConfigInfo(null, ChainConfigPart.ChainID);

using var blockChainContext = new BlockchainContext();
var dexes = blockChainContext.Dexes.Where(d => d.ChainId.ToString().Trim() == chainID).ToList();
if (dexes.Count == 0) return;
var uniswapV2Reader = new UniswapV2ContractsReader(httpURL);
int updatedPairCount = 0;
Output.WriteLine($"开始逐一扫描每个Dex的所有Pair的Reserve......");
foreach (var dex in dexes)
{
    int? maxPairIndex = blockChainContext.Pairs.Where(p => p.DexName == dex.DexName).Max(pair => pair.PairIndex);
    int storedPairsCount = (maxPairIndex is null) ? 0 : maxPairIndex.Value + 1;
    Output.WriteLine($"开始扫描{dex.DexName}的交易对......");
    for (int i = 0; i < storedPairsCount; i++)
    {
        Pair pair = blockChainContext.Pairs.First<Pair>(p => p.PairIndex == i && p.DexName == dex.DexName);
        if (pair is null) continue;
        var getReservesOutputDTO = await uniswapV2Reader.GetReserves(pair.PairAddress);
        if (pair.Reserve0.Trim() != getReservesOutputDTO.Reserve0.ToString().Trim() ||
            pair.Reserve1.Trim() != getReservesOutputDTO.Reserve1.ToString().Trim())
        {
            pair.Reserve0 = getReservesOutputDTO.Reserve0.ToString().Trim();
            pair.Reserve1 = getReservesOutputDTO.Reserve1.ToString().Trim();
            pair.BlockTimeLast = Convert.ToInt32(getReservesOutputDTO.BlockTimestampLast);
            blockChainContext.Pairs.Update(pair);
            blockChainContext.SaveChanges();
            updatedPairCount++;
            Output.WriteLine($"已更新{pair.PairAddress}的Reserve。目前共更新{updatedPairCount}个pair的Reserve。");
            Output.WriteLine("****************************************************************************************************");
        }
        if ((i + 1) % 10000 == 0)
        {
            Output.WriteLine($"{DateTime.Now}：已扫描{dex.DexName}的{i+1}个pair的Reserve。");
        }  
    }
}
