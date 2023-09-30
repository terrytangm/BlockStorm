using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Web3;
using Nethereum.JsonRpc.Client.Streaming;
using Nethereum.JsonRpc.WebSocketStreamingClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Subscriptions;
using Nethereum.RPC.Reactive.Eth.Subscriptions;
using Newtonsoft.Json;
using System;
using System.Numerics;
//using System.Reactive.Linq;
using System.Threading.Tasks;
using Nethereum.RPC.Reactive.Polling;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Geth.RPC.Debug;
using Nethereum.JsonRpc.Client;
using NBitcoin.RPC;
using System.IO;
using Nethereum.Geth.RPC.Debug.DTOs;

namespace BlockStorm.Samples
{
    public class Subscriptions
    {
        private static string ipc_path = "E:\\ETHNode\\Nethermind\\ipc.cfg";
        private static string ethMain_LocalHost_http = "http://localhost:8545";
        private static string ethMain_LocalHost_ws = "ws://localhost:8545";
        private static string ethMain_Alchemy_wss = "wss://eth-mainnet.g.alchemy.com/v2/gNTwg3OHHDuFfYoMvGR6OmsOU7qIIr87";
        private static string ethMain_Infura_wss = "wss://mainnet.infura.io/ws/v3/94a3a444da5d433eb10b29536057e6c9";
        private static string ethMain_Alchemy_http = "https://eth-mainnet.g.alchemy.com/v2/gNTwg3OHHDuFfYoMvGR6OmsOU7qIIr87";
        private static string ethMain_Infura_http = "https://mainnet.infura.io/v3/94a3a444da5d433eb10b29536057e6c9";
        private static string ethMain_Quicknode_wss = "wss://rough-clean-wildflower.quiknode.pro/961aac14702a59d6b5e54f753d10fde9849483d0/";
        public static async Task NewBlockHeader_With_Observable_Subscription()
        {
            using (var client = new StreamingWebSocketClient(ethMain_Infura_wss))
            {
                // create the subscription
                // (it won't start receiving data until Subscribe is called)
                var subscription = new EthNewBlockHeadersObservableSubscription(client);

                // attach a handler for when the subscription is first created (optional)
                // this will occur once after Subscribe has been called
                subscription.GetSubscribeResponseAsObservable().Subscribe(subscriptionId =>
                    Console.WriteLine("Block Header subscription Id: " + subscriptionId));

                DateTime? lastBlockNotification = null;
                double secondsSinceLastBlock = 0;

                // attach a handler for each block
                // put your logic here
                subscription.GetSubscriptionDataResponsesAsObservable().Subscribe(block =>
                {
                    secondsSinceLastBlock = lastBlockNotification == null ? 0 : (int)DateTime.Now.Subtract(lastBlockNotification.Value).TotalSeconds;
                    lastBlockNotification = DateTime.Now;
                    var utcTimestamp = DateTimeOffset.FromUnixTimeSeconds((long)block.Timestamp.Value);
                    Console.WriteLine($"New Block. Number: {block.Number.Value}, Timestamp UTC: {JsonConvert.SerializeObject(utcTimestamp)}, Seconds since last block received: {secondsSinceLastBlock} ");
                });

                bool subscribed = true;

                // handle unsubscription
                // optional - but may be important depending on your use case
                subscription.GetUnsubscribeResponseAsObservable().Subscribe(response =>
                {
                    subscribed = false;
                    Console.WriteLine("Block Header unsubscribe result: " + response);
                });

                // open the websocket connection
                await client.StartAsync();

                // start the subscription
                // this will only block long enough to register the subscription with the client
                // once running - it won't block whilst waiting for blocks
                // blocks will be delivered to our handler on another thread
                await subscription.SubscribeAsync();

                // run for a minute before unsubscribing
                await Task.Delay(TimeSpan.FromMinutes(1));

                // unsubscribe
                await subscription.UnsubscribeAsync();

                //allow time to unsubscribe
                while (subscribed) await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        public static async Task NewBlockHeader_With_Subscription()
        {
            using (var client = new StreamingWebSocketClient(ethMain_Infura_wss))
            {
                // create a subscription 
                // it won't do anything just yet though
                var subscription = new EthNewBlockHeadersSubscription(client);

                // attach our handler for new block header data
                subscription.SubscriptionDataResponse += (sender, e) =>
                {
                    var utcTimestamp = DateTimeOffset.FromUnixTimeSeconds((long)e.Response.Timestamp.Value);
                    Console.WriteLine($"New Block: Number: {e.Response.Number.Value}, Timestamp: {JsonConvert.SerializeObject(utcTimestamp)}");
                };

                // open the web socket connection
                await client.StartAsync();

                // subscribe to new block headers
                // blocks will be received on another thread
                // therefore this doesn't block the current thread
                await subscription.SubscribeAsync();

                //allow some time before we close the connection and end the subscription
                await Task.Delay(TimeSpan.FromMinutes(1));

                // the connection closing will end the subscription
            }
        }
        public static async Task GetPendingTransactionHash(string ws_Url)
        {
            using var client = new StreamingWebSocketClient(ws_Url);
            // create the subscription
            // it won't start receiving data until Subscribe is called on it
            var subscription = new EthNewPendingTransactionObservableSubscription(client);
            // attach a handler subscription created event (optional)
            // this will only occur once when Subscribe has been called
            subscription.GetSubscribeResponseAsObservable().Subscribe(subscriptionId =>
                Console.WriteLine("Pending transactions subscription Id: " + subscriptionId));
            // attach a handler for each pending transaction
            StreamWriter sw = new StreamWriter($"{DateTime.Now.ToString("HH-mm-ss")}.txt");
            subscription.GetSubscriptionDataResponsesAsObservable().Subscribe(transactionHash =>
            {
                sw.WriteLine($"{transactionHash},{DateTime.Now.ToString("hh-mm-ss")}");
            });
            bool subscribed = true;

            //handle unsubscription
            //optional - but may be important depending on your use case
            subscription.GetUnsubscribeResponseAsObservable().Subscribe(response =>
            {
                subscribed = false;
                Console.WriteLine("Pending transactions unsubscribe result: " + response);
                sw.Close();
            });

            //open the websocket connection
            await client.StartAsync();

            // start listening for pending transactions
            // this will only block long enough to register the subscription with the client
            // it won't block whilst waiting for transactions
            // transactions will be delivered to our handlers on another thread
            await subscription.SubscribeAsync();

            // run for minute
            // transactions should appear on another thread
            await Task.Delay(TimeSpan.FromMinutes(1));

            // unsubscribe
            await subscription.UnsubscribeAsync();

            // wait for unsubscribe 
            while (subscribed)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        /// <summary>
        /// 获取pending中的交易的完整信息
        /// </summary>
        /// <param name="ws_Url">websocket的URL</param>
        /// <returns></returns>
        public static async Task NewPendingFullTransactions(string ws_Url)
        {
            //var web3 = new Nethereum.Web3.Web3(ethMain_Infura_http);
            using var client = new StreamingWebSocketClient(ws_Url);
            //var rpcclient = new RpcClient(new Uri(ethMain_Quicknode_wss));
            // create the subscription
            // it won't start receiving data until Subscribe is called on it
            var subscription = new EthNewPendingFullTransactionObservableSubscription(client);

            // attach a handler subscription created event (optional)
            // this will only occur once when Subscribe has been called
            subscription.GetSubscribeResponseAsObservable().Subscribe(subscriptionId =>
                Console.WriteLine("Pending transactions subscription Id: " + subscriptionId));

            // attach a handler for each pending transaction
            // put your logic here
            subscription.GetSubscriptionDataResponsesAsObservable().Subscribe(tx =>
            {
                if (tx.Input.StartsWith("0x7ff36ab5"))
                {
                    Console.WriteLine($"Pending Transaction Hash:{tx.TransactionHash}");
                    Console.WriteLine("Calling swapExactETHForTokens(uint256 amountOutMin, address[] path, address to, uint256 deadline)");
                    Console.WriteLine("block Number:" + tx.BlockNumber);
                    Console.WriteLine("Block Hash:" + tx.BlockHash);
                    Console.WriteLine("From:" + tx.From);
                    Console.WriteLine("To:" + tx.To);
                    Console.WriteLine("Value:" + Web3.Convert.FromWei(tx.Value) + " ETH");
                    Console.WriteLine("Input Data:" + tx.Input);
                    Console.WriteLine("*************************************************************************");
                }
                else if (tx.Input.StartsWith("0x791ac947"))
                {
                    Console.WriteLine($"Pending Transaction Hash:{tx.TransactionHash}");
                    Console.WriteLine("Calling swapExactTokensForETHSupportingFeeOnTransferTokens(uint256 amountIn, uint256 amountOutMin, address[] path, address to, uint256 deadline)");
                    Console.WriteLine("block Number:" + tx.BlockNumber);
                    Console.WriteLine("Block Hash:" + tx.BlockHash);
                    Console.WriteLine("From:" + tx.From);
                    Console.WriteLine("To:" + tx.To);
                    Console.WriteLine("Value:" + Web3.Convert.FromWei(tx.Value) + " ETH");
                    Console.WriteLine("Input Data:" + tx.Input);
                    Console.WriteLine("*************************************************************************");
                }
                else if (tx.Input.StartsWith("0x38ed1739"))
                {
                    Console.WriteLine($"Pending Transaction Hash:{tx.TransactionHash}");
                    Console.WriteLine("Calling swapExactTokensForTokens(uint256 amountIn, uint256 amountOutMin, address[] path, address to, uint256 deadline)");
                    Console.WriteLine("block Number:" + tx.BlockNumber);
                    Console.WriteLine("Block Hash:" + tx.BlockHash);
                    Console.WriteLine("From:" + tx.From);
                    Console.WriteLine("To:" + tx.To);
                    Console.WriteLine("Value:" + Web3.Convert.FromWei(tx.Value) + " ETH");
                    Console.WriteLine("Input Data:" + tx.Input);
                    Console.WriteLine("*************************************************************************");
                }
                else if (tx.Input.StartsWith("0x18cbafe5"))
                {
                    Console.WriteLine($"Pending Transaction Hash:{tx.TransactionHash}");
                    Console.WriteLine("Calling swapExactTokensForETH(uint256 amountIn, uint256 amountOutMin, address[] path, address to, uint256 deadline)");
                    Console.WriteLine("block Number:" + tx.BlockNumber);
                    Console.WriteLine("Block Hash:" + tx.BlockHash);
                    Console.WriteLine("From:" + tx.From);
                    Console.WriteLine("To:" + tx.To);
                    Console.WriteLine("Value:" + Web3.Convert.FromWei(tx.Value) + " ETH");
                    Console.WriteLine("Input Data:" + tx.Input);
                    Console.WriteLine("*************************************************************************");
                }
                //await GetTransactionDebugTrace(tx.TransactionHash, ethMain_Quicknode_wss, rpcclient, sw);
            }
                );

            /*
            subscription.GetSubscriptionDataResponsesAsObservable().Subscribe(async response =>
            {
                Console.WriteLine("New pending transaction: " + response);
                var result = await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(response);
            if (result != null)
            {
                Console.WriteLine("Transaction Hash:" + result.TransactionHash);
                Console.WriteLine("block Number:" + result.BlockNumber);
                Console.WriteLine("Block Hash:" + result.BlockHash);
                Console.WriteLine("From:" + result.From);
                Console.WriteLine("To:" + result.To);
                Console.WriteLine("Value:" + Nethereum.Web3.Web3.Convert.FromWei(result.Value) + "ETH");
                Console.WriteLine("Input Data:" + result.Input);
            }
            else
            {
                Console.WriteLine("Transaction Hash " + response +" returns null!");
                }

            });
            */
            bool subscribed = true;

            //handle unsubscription
            //optional - but may be important depending on your use case
            subscription.GetUnsubscribeResponseAsObservable().Subscribe(response =>
            {
                subscribed = false;
                Console.WriteLine("Pending transactions unsubscribe result: " + response);
            });

            //open the websocket connection
            await client.StartAsync();

            // start listening for pending transactions
            // this will only block long enough to register the subscription with the client
            // it won't block whilst waiting for transactions
            // transactions will be delivered to our handlers on another thread
            await subscription.SubscribeAsync();

            // run for minute
            // transactions should appear on another thread
            await Task.Delay(TimeSpan.FromMinutes(5));

            // unsubscribe
            await subscription.UnsubscribeAsync();

            // wait for unsubscribe 
            while (subscribed)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        static async Task GetTransactionDebugTrace(string transactionHash, string ws_Url, RpcClient client, StreamWriter sw)
        {

            var debugTrace = new TraceTransaction(client);
            //var options = new TraceTransactionOptions();
            var result = await debugTrace.SendRequestAsync(transactionHash);
            //StreamWriter sw = new StreamWriter($"C:\\Users\\Terry\\Documents\\{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.txt");
            if (result != null)
            {
                sw.WriteLine($"{transactionHash} has the following traces:");
                foreach (var tx in result)
                {
                    sw.WriteLine(tx);
                }
                sw.WriteLine("*************************************************************************");
            }
            else
            {
                sw.WriteLine($"{transactionHash} has NO traces!");
                sw.WriteLine("*************************************************************************");
            }
            //sw.Close();
        }

        public static async Task GetLogs_Observable_Subscription()
        {
            using (var client = new StreamingWebSocketClient("wss://mainnet.infura.io/ws"))
            {
                // create the subscription
                // nothing will happen just yet though
                var subscription = new EthLogsObservableSubscription(client);

                // attach our handler for each log
                subscription.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
                {
                    Console.WriteLine("Log Address:" + log.Address);
                });

                // create the web socket connection
                await client.StartAsync();

                // begin receiving subscription data
                // data will be received on another thread
                await subscription.SubscribeAsync();

                // allow to run for a minute
                await Task.Delay(TimeSpan.FromMinutes(1));

                // unsubscribe
                await subscription.UnsubscribeAsync();

                // allow some time to unsubscribe
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        public static async Task GetLogsTokenTransfer_Observable_Subscription()
        {
            // ** SEE THE TransferEventDTO class below **

            using (var client = new StreamingWebSocketClient(ethMain_LocalHost_ws))
            {
                // create a log filter specific to Transfers
                // this filter will match any Transfer (matching the signature) regardless of address
                var filterTransfers = Event<TransferEventDTO>.GetEventABI().CreateFilterInput();

                // create the subscription
                // it won't do anything yet
                var subscription = new EthLogsObservableSubscription(client);

                // attach a handler for Transfer event logs
                subscription.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
                {
                    try
                    {
                        // decode the log into a typed event log
                        var decoded = Event<TransferEventDTO>.DecodeEvent(log);
                        if (decoded != null)
                        {
                            Console.WriteLine("Contract address: " + log.Address + " Log Transfer from:" + decoded.Event.From);
                        }
                        else
                        {
                            // the log may be an event which does not match the event
                            // the name of the function may be the same
                            // but the indexed event parameters may differ which prevents decoding
                            Console.WriteLine("Found not ERC20 standard transfer log");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Log Address: " + log.Address + " is not a standard ERC20 transfer log:", ex.Message);
                    }
                });

                // open the web socket connection
                await client.StartAsync();

                // begin receiving subscription data
                // data will be received on a background thread
                await subscription.SubscribeAsync(filterTransfers);

                // run for a while
                await Task.Delay(TimeSpan.FromMinutes(1));

                // unsubscribe
                await subscription.UnsubscribeAsync();

                // allow time to unsubscribe
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        public static async Task GetSyncReserve_Observable_Subscription()
        {
            using var client = new StreamingWebSocketClient(ethMain_LocalHost_ws);
            // create a log filter specific to Transfers
            // this filter will match any Transfer (matching the signature) regardless of address
            var filterTransfers = Event<SyncEventDTO>.GetEventABI().CreateFilterInput();

            // create the subscription
            // it won't do anything yet
            var subscription = new EthLogsObservableSubscription(client);

            subscription.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                try
                {
                    // decode the log into a typed event log
                    var decoded = Event<SyncEventDTO>.DecodeEvent(log);
                    if (decoded != null)
                    {
                        Console.WriteLine("Contract address: " + log.Address);
                        Console.WriteLine("Reserve0:" + decoded.Event.Reserve0);
                        Console.WriteLine("Reserve1:" + decoded.Event.Reserve1);
                    }
                    else
                    {
                        // the log may be an event which does not match the event
                        // the name of the function may be the same
                        // but the indexed event parameters may differ which prevents decoding
                        Console.WriteLine("Found not Sync log");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Log Address: " + log.Address + " is not a standard Sync log:", ex.Message);
                }
            });

            // open the web socket connection
            await client.StartAsync();

            // begin receiving subscription data
            // data will be received on a background thread
            await subscription.SubscribeAsync(filterTransfers);

            // run for a while
            await Task.Delay(TimeSpan.FromMinutes(1));

            // unsubscribe
            await subscription.UnsubscribeAsync();

            // allow time to unsubscribe
            await Task.Delay(TimeSpan.FromSeconds(5));
        }


        // This class describes the Transfer event
        // It allows untyped logs to be decoded into typed representations
        // This allows the event parameters to be decoded
        // It also provides a basis for creating filters which are used to retrieve matching logs 
        // It can be created by hand but often this class is code generated 
        // It is marked as partial to allow you to extend it without breaking everytime the code is regenerated
        // .nethereum-events-gettingstarted/
        public partial class TransferEventDTO : TransferEventDTOBase { }

        [Event("Transfer")]
        public class TransferEventDTOBase : IEventDTO
        {
            [Parameter("address", "_from", 1, true)]
            public virtual string? From { get; set; }
            [Parameter("address", "_to", 2, true)]
            public virtual string? To { get; set; }
            [Parameter("uint256", "_value", 3, false)]
            public virtual BigInteger Value { get; set; }
        }

        public partial class SyncEventDTO : SyncEventDTOBase { }

        [Event("Sync")]
        public class SyncEventDTOBase : IEventDTO
        {
            [Parameter("uint112", "_reserve0", 1)]
            public virtual BigInteger Reserve0 { get; set; }
            [Parameter("uint112", "_reserve1", 2)]
            public virtual BigInteger Reserve1 { get; set; }
        }
    }
}