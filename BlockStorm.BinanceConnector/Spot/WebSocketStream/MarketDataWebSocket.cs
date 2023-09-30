namespace BlockStorm.BinanceConnector.Spot.WebSocketStream
{
    using System.Net.WebSockets;
    using BlockStorm.BinanceConnector.Common;

    public class MarketDataWebSocket : BinanceWebSocket
    {
        private const string DEFAULT_USER_DATA_WEBSOCKET_BASE_URL = "wss://stream.binance.com:9443";

        public MarketDataWebSocket(string stream, string baseUrl = DEFAULT_USER_DATA_WEBSOCKET_BASE_URL)
        : base(new BinanceWebSocketHandler(new ClientWebSocket()), baseUrl + "/ws/" + stream)
        {
        }

        public MarketDataWebSocket(string stream, IBinanceWebSocketHandler handler, string baseUrl = DEFAULT_USER_DATA_WEBSOCKET_BASE_URL)
        : base(handler, baseUrl + "/ws/" + stream)
        {
        }

        public MarketDataWebSocket(string[] streams, string baseUrl = DEFAULT_USER_DATA_WEBSOCKET_BASE_URL)
        : base(new BinanceWebSocketHandler(new ClientWebSocket()), baseUrl + "/stream?streams=" + string.Join("/", streams))
        {
        }

        public MarketDataWebSocket(string[] streams, IBinanceWebSocketHandler handler, string baseUrl = DEFAULT_USER_DATA_WEBSOCKET_BASE_URL)
        : base(handler, baseUrl + "/stream?streams=" + string.Join("/", streams))
        {
        }
    }
}