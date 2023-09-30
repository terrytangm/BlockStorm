using BlockStorm.Arb.Algorithm;
using BlockStorm.EFModels;
using BlockStorm.Utils;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

var chainID = Config.ConfigInfo(null, ChainConfigPart.ChainID);
var context = new BlockchainContext();
var filteredPairs = context.FilteredPairs.Where(fp => fp.ChainId == Convert.ToInt64(chainID)).ToList();
var tokenInList = context.Tokens.Where(t => t.IsTopToken == true && t.ChainId == Convert.ToInt64(chainID)).ToList();
Output.WriteLine($"有{filteredPairs.Count}个交易对进入分析范围");
Output.WriteLineSymbols('*', 100);
foreach (var tokenIn in tokenInList)
{
    Output.WriteLine($"开始分析{tokenIn.TokenAddress}({tokenIn.Symbol})的兑换路径");
    var completedPaths = new List<CompletedPath>();
    FilterPairs.GetPaths(filteredPairs, tokenIn.TokenAddress, tokenIn.TokenAddress, 3, null, null, completedPaths);
    Output.WriteLine($"分析出{completedPaths.Count}个兑换路径");
    int newRouteCount = 0;
    List<string> addedHashes = new();
    foreach (var completedPath in completedPaths)
    {
        string routeHash = GetRouteHash(completedPath.tokens, completedPath.pairs);
        if (context.Routes.Where(r => r.RouteHash.Equals(routeHash.Trim())).Any() || addedHashes.Contains(routeHash)) continue;
        context.Routes.Add(GetRouteViaPath(completedPath, routeHash, chainID));
        addedHashes.Add(routeHash);
        newRouteCount++;
    }
    Output.WriteLine($"共有{newRouteCount}个新增兑换路径，现在向数据库添加");
    context.SaveChanges();
    Output.WriteLine($"向数据库添加{newRouteCount}个兑换路径成功");
    Output.WriteLineSymbols('*', 120);
}

Route GetRouteViaPath(CompletedPath completedPath, string routeHash, string chainID)
{
    if (completedPath.pairs == null || completedPath.pairs.Count == 0)
    {
        throw new ArgumentNullException(nameof(completedPath.pairs));
    }
    var route = new Route
    {
        Enabled = true,
        Created = DateTime.Now,
        LastUpdate = DateTime.Now,
        Hop = (short)completedPath.pairs.Count,
        TokenIn = completedPath.tokens[0],
        TokenOut = completedPath.tokens[0],
        RouteHash = routeHash,
        ChainId = Convert.ToInt64(chainID),
        OptimalInput = completedPath.optimalInput.ToString().Trim(),
        OptimalProfit = completedPath.optimalProfit.ToString().Trim()
    };
    bool routeEnabled = true;
    for (int i = 0; i < completedPath.pairs.Count; i++)
    {
        var routeNode = new RouteNode
        {
            Created = DateTime.Now,
            Route = route,
            PairRank = (short)i,
            TokenIn = completedPath.tokens[i],
            TokenOut = completedPath.tokens[i + 1],
            Pair = completedPath.pairs[i].PairAddress
        };
        bool nodeEnabled = (bool)(completedPath.pairs[i].Token0 == routeNode.TokenIn ? completedPath.pairs[i].Token0In : completedPath.pairs[i].Token1In);
        if(nodeEnabled == false)
        {
            routeEnabled = false;
        }
        route.RouteNodes.Add(routeNode);
    }
    route.Enabled = routeEnabled;
    return route;
}

string GetRouteHash(List<string> tokens, List<FilteredPair> pairs)
{
    if (tokens == null || tokens.Count == 0) return string.Empty;
    var sb = new StringBuilder();
    for (int i = 0; i < tokens.Count; i++)
    {
        sb.Append(tokens[i]);
    }
    for (int i = 0; i < pairs.Count; i++)
    {
        sb.Append(pairs[i].PairAddress);
    }
    return Crypto.ComputeSHA256Hash(sb.ToString());
}
