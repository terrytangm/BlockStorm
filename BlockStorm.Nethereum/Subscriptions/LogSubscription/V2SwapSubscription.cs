﻿using BlockStorm.NethereumModule.Contracts.UniswapV2Pair;
using Nethereum.Contracts;
using Nethereum.JsonRpc.WebSocketStreamingClient;
using Nethereum.RPC.Reactive.Eth.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockStorm.NethereumModule.Subscriptions.LogSubscription
{
    public class SwapSubscription
    {
        public event EventHandler<LogReceivedEventArgs>? OnLogReceived;
        public readonly StreamingWebSocketClient client;
        public EthLogsObservableSubscription Subscription { get; }
        public SwapSubscription(string? webSocketURL)
        {
            client = new StreamingWebSocketClient(webSocketURL);
            Subscription = new EthLogsObservableSubscription(client);
            
        }

        public async Task GetSwap_Observable_Subscription(string contractAddress)
        {
            var filterTransfers = Event<SwapEventDTO>.GetEventABI().CreateFilterInput(contractAddress);
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
