namespace BlockStorm.BinanceConnector.Spot
{
    using System.Net.WebSockets;
    using BlockStorm.BinanceConnector.Common;
    using BlockStorm.BinanceConnector.Spot.WebSocketApiUtil;

    public class WebSocketApi : BinanceWebSocketApi
    {
        private const string DEFAULT_WEBSOCKET_API_BASE_URL = "wss://ws-api.binance.com:443/ws-api/v3";
        private WebSocketApiGeneral general;
        private WebSocketApiMarket market;
        private WebSocketApiAccountTrade accountTrade;
        private WebSocketApiUserDataStream userDataStream;

        public WebSocketApi(string baseUrl = DEFAULT_WEBSOCKET_API_BASE_URL, string apiKey = null, IBinanceSignatureService signatureService = null)
        : base(new BinanceWebSocketHandler(new ClientWebSocket()), baseUrl, apiKey, signatureService: signatureService)
        {
            general = new WebSocketApiGeneral(this);
            market = new WebSocketApiMarket(this);
            accountTrade = new WebSocketApiAccountTrade(this);
            userDataStream = new WebSocketApiUserDataStream(this);
        }

        public WebSocketApiGeneral General
        {
            get
            {
                return general;
            }
        }

        public WebSocketApiMarket Market
        {
            get
            {
                return market;
            }
        }

        public WebSocketApiAccountTrade AccountTrade
        {
            get
            {
                return accountTrade;
            }
        }

        public WebSocketApiUserDataStream UserDataStream
        {
            get
            {
                return userDataStream;
            }
        }
    }
}
