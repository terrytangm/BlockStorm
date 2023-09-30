using System.Threading.Tasks;
using Nethereum.Geth.RPC.Debug.DTOs;
using Nethereum.JsonRpc.Client;
using Newtonsoft.Json.Linq;

namespace BlockStorm.Samples
{
    /// <Summary>

    /// </Summary>
    public class TraceTransaction : RpcRequestResponseHandler<JArray>, ITraceTransaction
    {
        public TraceTransaction(IClient client) : base(client, "trace_transaction")
        {
        }

        public RpcRequest BuildRequest(string txnHash, object id = null)
        {
            return BuildRequest(id, txnHash);
        }

        public Task<JArray> SendRequestAsync(string txnHash, object id = null)
        {
            return SendRequestAsync(id, txnHash);
        }
    }

    public interface ITraceTransaction
    {
        RpcRequest BuildRequest(string txnHash, object id = null);

        Task<JArray> SendRequestAsync(string txnHash, object id = null);
    }
}