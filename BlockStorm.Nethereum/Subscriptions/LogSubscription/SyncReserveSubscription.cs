using Nethereum.Contracts;
using Nethereum.RPC.Reactive.Eth.Subscriptions;
using Nethereum.JsonRpc.WebSocketStreamingClient;
using BlockStorm.NethereumModule.Contracts.UniswapV2Pair;
using Org.BouncyCastle.Asn1.Mozilla;
using System.Diagnostics.Tracing;

namespace BlockStorm.NethereumModule.Subscriptions.LogSubscription
{
    public class SyncReserveSubscription
    {
        public event EventHandler<LogReceivedEventArgs>? OnLogReceived;
        private readonly StreamingWebSocketClient client;
        public EthLogsObservableSubscription Subscription { get; }
        public SyncReserveSubscription(string? webSocketURL)
        {
            client = new StreamingWebSocketClient(webSocketURL);
            Subscription = new EthLogsObservableSubscription(client);
        }

        public async Task GetSyncReserve_Observable_Subscription()
        {
            // create a log filter specific to Transfers
            // this filter will match any Transfer (matching the signature) regardless of address
            var filterTransfers = Event<SyncEventDTO>.GetEventABI().CreateFilterInput();

            Subscription.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                LogReceivedEventArgs e = new(log);
                OnLogReceived?.Invoke(this, e);
            });

            // open the web socket connection
            await client.StartAsync();

            // begin receiving subscription data
            // data will be received on a background thread
            await Subscription.SubscribeAsync(filterTransfers);

            // run for a while
            await Task.Delay(TimeSpan.FromMilliseconds(-1));
        }
    }
}
