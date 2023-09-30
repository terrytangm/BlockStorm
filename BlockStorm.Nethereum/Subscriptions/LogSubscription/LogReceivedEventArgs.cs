using Nethereum.RPC.Eth.DTOs;

namespace BlockStorm.NethereumModule.Subscriptions.LogSubscription
{
    public class LogReceivedEventArgs: EventArgs
    {
        public FilterLog ReceivedLog { get; set; }
        public LogReceivedEventArgs(FilterLog receivedLog) 
        {
            this.ReceivedLog = receivedLog;
        }
    }
}
