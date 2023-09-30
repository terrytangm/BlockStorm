// See https://aka.ms/new-console-template for more information
using BlockStorm.EFModels;
using BlockStorm.NethereumModule;
using BlockStorm.Utils;
using Nethereum.Model;
using System.Configuration;

string httpURL = Config.ConfigInfo(null, ChainConfigPart.HttpURL);
var chainID = Config.ConfigInfo(null, ChainConfigPart.ChainID);

using var context = new BlockchainContext();
var syncLogsWithNoDexName = context.SyncReserveLogs.Where(s => string.IsNullOrEmpty(s.DexName) && s.ChainId.ToString().Trim() == chainID).ToList();



var contractReader = new UniswapV2ContractsReader(httpURL);
var dexNameAndPairAddress = new List<string>();

foreach (var log in syncLogsWithNoDexName)
{
    if (!string.IsNullOrEmpty(log.PairAddress))
    {
        string tokenName = await contractReader.GetTokenName(log.PairAddress);
        dexNameAndPairAddress.Add($"{tokenName} {log.PairAddress} {log.Created.Value.ToString("yy-MM-dd HH:mm:ss")}");
    }
    
}
if (!dexNameAndPairAddress.Any()) return;
dexNameAndPairAddress.Sort((x, y) => x.CompareTo(y));
foreach (var log in dexNameAndPairAddress)
{
    Output.WriteLine(log);
}

