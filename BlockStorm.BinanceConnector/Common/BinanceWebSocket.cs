namespace BlockStorm.BinanceConnector.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Binance web socket wrapper.
    /// </summary>
    public class BinanceWebSocket : IDisposable
    {
        private IBinanceWebSocketHandler handler;
        private List<Func<string, Task>> onMessageReceivedFunctions;
        private List<CancellationTokenRegistration> onMessageReceivedCancellationTokenRegistrations;
        private CancellationTokenSource loopCancellationTokenSource;
        private Uri url;
        private int receiveBufferSize;

        public BinanceWebSocket(IBinanceWebSocketHandler handler, string url, int receiveBufferSize = 8192)
        {
            this.handler = handler;
            this.url = new Uri(url);
            this.receiveBufferSize = receiveBufferSize;
            onMessageReceivedFunctions = new List<Func<string, Task>>();
            onMessageReceivedCancellationTokenRegistrations = new List<CancellationTokenRegistration>();
        }

        public async Task ConnectAsync(CancellationToken cancellationToken)
        {
            if (handler.State != WebSocketState.Open)
            {
                loopCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                await handler.ConnectAsync(url, cancellationToken);
                await Task.Factory.StartNew(() => ReceiveLoop(loopCancellationTokenSource.Token, receiveBufferSize), loopCancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }

        public async Task DisconnectAsync(CancellationToken cancellationToken)
        {
            if (loopCancellationTokenSource != null)
            {
                loopCancellationTokenSource.Cancel();
            }

            if (handler.State == WebSocketState.Open)
            {
                await handler.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, null, cancellationToken);
                await handler.CloseAsync(WebSocketCloseStatus.NormalClosure, null, cancellationToken);
            }
        }

        public void OnMessageReceived(Func<string, Task> onMessageReceived, CancellationToken cancellationToken)
        {
            onMessageReceivedFunctions.Add(onMessageReceived);

            if (cancellationToken != CancellationToken.None)
            {
                var reg = cancellationToken.Register(() =>
                    onMessageReceivedFunctions.Remove(onMessageReceived));

                onMessageReceivedCancellationTokenRegistrations.Add(reg);
            }
        }

        public async Task SendAsync(string message, CancellationToken cancellationToken)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(message);

            await handler.SendAsync(new ArraySegment<byte>(byteArray), WebSocketMessageType.Text, true, cancellationToken);
        }

        public void Dispose()
        {
            DisconnectAsync(CancellationToken.None).Wait();

            handler.Dispose();

            onMessageReceivedCancellationTokenRegistrations.ForEach(ct => ct.Dispose());

            loopCancellationTokenSource.Dispose();
        }

        private async Task ReceiveLoop(CancellationToken cancellationToken, int receiveBufferSize = 8192)
        {
            WebSocketReceiveResult receiveResult = null;
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var buffer = new ArraySegment<byte>(new byte[receiveBufferSize]);
                    receiveResult = await handler.ReceiveAsync(buffer, cancellationToken);

                    if (receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }

                    string content = Encoding.UTF8.GetString(buffer.ToArray(), buffer.Offset, buffer.Count);
                    onMessageReceivedFunctions.ForEach(omrf => omrf(content));
                }
            }
            catch (TaskCanceledException)
            {
                await DisconnectAsync(CancellationToken.None);
            }
        }
    }
}