using System.Threading.Tasks;
using Nethereum.Geth.RPC.Debug;
using Nethereum.Geth.RPC.Debug.DTOs;
using Nethereum.JsonRpc.Client;
using Newtonsoft.Json.Linq;

namespace BlockStorm.Samples
{
    /// <Summary>
    ///     The traceTransaction debugging method will attempt to run the transaction in the exact same manner as it was
    ///     executed on the network. It will replay any transaction that may have been executed prior to this one before it
    ///     will finally attempt to execute the transaction that corresponds to the given hash.
    ///     In addition to the hash of the transaction you may give it a secondary optional argument, which specifies the
    ///     options for this specific call. The possible options are:
    ///     disableStorage: BOOL. Setting this to true will disable storage capture (default = false).
    ///     disableMemory: BOOL. Setting this to true will disable memory capture (default = false).
    ///     disableStack: BOOL. Setting this to true will disable stack capture (default = false).
    ///     fullStorage: BOOL. Setting this to true will return you, for each opcode, the full storage, including everything
    ///     which hasn't changed. This is a slow process and is therefor defaulted to false. By default it will only ever give
    ///     you the changed storage values.
    /// </Summary>
    public class TraceCall : RpcRequestResponseHandler<JObject>, IDebugTraceTransaction
    {
        public TraceCall(IClient client) : base(client, "trace_call")
        {
        }

        public RpcRequest BuildRequest(string txnHash, TraceTransactionOptions options, object id = null)
        {
            return BuildRequest(id, txnHash, options);
        }

        public Task<JObject> SendRequestAsync(string txnHash, TraceTransactionOptions options, object id = null)
        {
            return SendRequestAsync(id, txnHash, options);
        }
    }
}