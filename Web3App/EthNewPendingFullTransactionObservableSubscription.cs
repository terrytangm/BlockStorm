using System.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.Client.Streaming;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Subscriptions;
using Nethereum.RPC.Reactive.RpcStreaming;

namespace BlockStorm.Samples
{

    public class EthNewPendingFullTransactionObservableSubscription : RpcStreamingSubscriptionObservableHandler<Transaction>
    {
        private EthNewPendingFullTransactionSubscriptionRequestBuilder _builder;

        public EthNewPendingFullTransactionObservableSubscription(IStreamingClient client) : base(client, new EthUnsubscribeRequestBuilder())
        {
            _builder = new EthNewPendingFullTransactionSubscriptionRequestBuilder();
        }

        public Task SubscribeAsync(object id = null)
        {
            return base.SubscribeAsync(BuildRequest(id));
        }

        public RpcRequest BuildRequest(object id)
        {
            return _builder.BuildRequest(id);
        }
    }
}
