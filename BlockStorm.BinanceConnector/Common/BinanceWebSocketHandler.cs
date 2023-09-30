namespace BlockStorm.BinanceConnector.Common
{
    using System;
    using System.Net.WebSockets;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Binance humble object for <see cref="ClientWebSocket" />.
    /// </summary>
    public class BinanceWebSocketHandler : IBinanceWebSocketHandler
    {
        private static readonly string UserAgent = "binance-connector-dotnet/" + VersionInfo.GetVersion;

        private ClientWebSocket webSocket;

        public BinanceWebSocketHandler(ClientWebSocket clientWebSocket)
        {
            webSocket = clientWebSocket;
            webSocket.Options.SetRequestHeader("User-Agent", UserAgent);
        }

        public WebSocketState State
        {
            get
            {
                return webSocket.State;
            }
        }

        public async Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
        {
            await webSocket.ConnectAsync(uri, cancellationToken);
        }

        public async Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            await webSocket.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
        }

        public async Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            await webSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
        }

        public async Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
        {
            return await webSocket.ReceiveAsync(buffer, cancellationToken);
        }

        public async Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
        {
            await webSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
        }

        public void Dispose()
        {
            webSocket.Dispose();
        }
    }
}