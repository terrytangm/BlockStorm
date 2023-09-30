using Nethereum.JsonRpc.Client;
using Nethereum.RPC;

namespace BlockStorm.Samples
{
    public class EthNewPendingFullTransactionSubscriptionRequestBuilder : RpcRequestBuilder
    {
        public EthNewPendingFullTransactionSubscriptionRequestBuilder() : base(ApiMethods.eth_subscribe.ToString())
        {
        }

        public override RpcRequest BuildRequest(object id = null)
        {
            if (id == null) id = Guid.NewGuid().ToString();
            return base.BuildRequest(id, "newPendingTransactions", true);
        }
    }
}