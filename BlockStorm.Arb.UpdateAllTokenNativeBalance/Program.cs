// See https://aka.ms/new-console-template for more information
using BlockStorm.EFModels;
using BlockStorm.NethereumModule;
using BlockStorm.Utils;
using EFCore.BulkExtensions;
using System.Configuration;


var chainID = Config.ConfigInfo(null, ChainConfigPart.ChainID);
var httpURL = Config.ConfigInfo(null, ChainConfigPart.HttpURL);

using var blockChainContext = new BlockchainContext();
var tokens = blockChainContext.Tokens.Where(t => t.ChainId.ToString().Trim() == chainID).ToList();
if (tokens.Count == 0) return;
var web3EthUtil = new Web3ETHUtil(httpURL, string.Empty);
var tokensToUpdate = new List<Token>();
(int left, int top) = Console.GetCursorPosition();
foreach (var token in tokens)
{
    var nativeBalance = await web3EthUtil.GetNativeNokenBalance(token.TokenAddress);
    if (token.NativeTokenBalance == null || token.NativeTokenBalance != nativeBalance.ToString().Trim())
    {
        token.NativeTokenBalance = nativeBalance.ToString().Trim();
        token.LastBalanceUpdate = DateTime.Now;
        tokensToUpdate.Add(token);
        Output.WriteLine($"已缓存{tokensToUpdate.Count}个Native Token Balance有更新的Token");
        Console.SetCursorPosition(left, top);
    }
}
Output.WriteLine($"批量向数据库更新Token的Balance......");
blockChainContext.BulkUpdate<Token>(tokensToUpdate);
Console.ReadLine();


