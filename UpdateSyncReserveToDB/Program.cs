// See https://aka.ms/new-console-template for more information
using System.Numerics;
using System.Configuration;
using BlockStorm.EFModels;
using BlockStorm.Utils;
using BlockStorm.NethereumModule.Contracts.UniswapV2Pair;
using BlockStorm.NethereumModule.Subscriptions.LogSubscription;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Web3;
using System.Security.Policy;
using System.Runtime.InteropServices.WindowsRuntime;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using System.Web;

var webSocketURL = Config.ConfigInfo(null, ChainConfigPart.WebsocketURL);
var chainID = Config.ConfigInfo(null, ChainConfigPart.ChainID);

SyncReserveSubscription syncReserveSubscription = new(webSocketURL);
syncReserveSubscription.OnLogReceived += SyncReserveSubscription_OnLogReceivedAsync;
await syncReserveSubscription.GetSyncReserve_Observable_Subscription();
Console.CancelKeyPress += Console_CancelKeyPress;

void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
{
    syncReserveSubscription.Subscription.UnsubscribeAsync();
}

void SyncReserveSubscription_OnLogReceivedAsync(object? sender, LogReceivedEventArgs e)
{
    if (e == null){ return; }
    FilterLog log = e.ReceivedLog;
    if (log == null) { return; }
    try
    {
        // decode the log into a typed event log
        var decoded = Event<SyncEventDTO>.DecodeEvent(log);
        if (decoded != null)
        {
            Output.WriteLine(DateTime.Now.ToString());
            Output.WriteLine("Contract address: " + log.Address);
            Output.WriteLine("Reserve0:" + decoded.Event.Reserve0);
            Output.WriteLine("Reserve1:" + decoded.Event.Reserve1);
            bool updated;
            string dexName;
            try
            {
                (updated, dexName) = updateReserveAsync(log, decoded);
                short updatedRow = (short)(updated ? 1 : 0);
                InsertSyncReserveLogToDB(log, decoded, dexName, Convert.ToInt32(chainID));
                Output.WriteLine($"已更新{dexName} Pair表的{updatedRow}条数据");
                Output.WriteLine($"已插入1条SyncReserveLog");
            }
            catch (Exception ex) 
            {
                Output.WriteLine($"更新数据库出错，错误信息：{ex}");
            }
        }
        else
        {
            // the log may be an event which does not match the event
            // the name of the function may be the same
            // but the indexed event parameters may differ which prevents decoding
            Output.WriteLine("Found not Sync log");
        }
    }
    catch (Exception ex)
    {
        Output.WriteLine("Log Address: " + log.Address + " is not a standard Sync log: " + ex.Message);
    }
    finally
    {
        Output.WriteLine("**********************************************************************************");
    }
}

(bool, string) updateReserveAsync(FilterLog log, EventLog<SyncEventDTO> decoded)
{
    using var context = new BlockchainContext();
    var pair = context.Pairs.FirstOrDefault(pair => pair.PairAddress == log.Address);
    bool updated;
    string dexName;
    if (pair != null)
    {
        
        pair.Reserve0 = decoded.Event.Reserve0.ToString().Trim();
        pair.Reserve1 = decoded.Event.Reserve1.ToString().Trim();
        pair.LastUpdate=DateTime.Now;
        context.SaveChanges();
        updated = true;
        dexName = pair.DexName;
    }
    else
    {
        updated = false;
        dexName = string.Empty;
    }

    return (updated, dexName);
}

void InsertSyncReserveLogToDB(FilterLog log, EventLog<SyncEventDTO> decoded, string? dexName, int? chainID)
{
    using var context = new BlockchainContext();
    var syncReserveLog = new SyncReserveLog
    {
        PairAddress = log.Address,
        ChainId = chainID,
        Reserve0 = decoded.Event.Reserve0.ToString().Trim(),
        Reserve1 = decoded.Event.Reserve1.ToString().Trim(),
        TransactionHash = log.TransactionHash,
        BlockHash = log.BlockHash,
        BlockNumber = log.BlockNumber.ToLong(),
        DexName = dexName,
        Created = DateTime.Now
    };
    context.SyncReserveLogs.Add(syncReserveLog);
    context.SaveChanges();
}