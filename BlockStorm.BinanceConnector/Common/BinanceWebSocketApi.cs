namespace BlockStorm.BinanceConnector.Common
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using Newtonsoft.Json;

    /// <summary>
    /// Binance WebSocket API wrapper.
    /// </summary>
    public class BinanceWebSocketApi : IDisposable
    {
        private string apiKey;
        private IBinanceWebSocketHandler handler;
        private IBinanceSignatureService signatureService;
        private List<Func<string, Task>> onMessageReceivedFunctions;
        private List<CancellationTokenRegistration> onMessageReceivedCancellationTokenRegistrations;
        private CancellationTokenSource loopCancellationTokenSource;
        private Uri url;
        private int receiveBufferSize;

        public BinanceWebSocketApi(IBinanceWebSocketHandler handler, string url, string apiKey, IBinanceSignatureService signatureService, int receiveBufferSize = 8192)
        {
            this.handler = handler;
            this.url = new Uri(url);
            this.apiKey = apiKey;
            this.signatureService = signatureService;
            this.receiveBufferSize = receiveBufferSize;
            onMessageReceivedFunctions = new List<Func<string, Task>>();
            onMessageReceivedCancellationTokenRegistrations = new List<CancellationTokenRegistration>();
        }

        public async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            if (handler.State != WebSocketState.Open)
            {
                loopCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                await handler.ConnectAsync(url, cancellationToken);
                await Task.Factory.StartNew(() => ReceiveLoop(loopCancellationTokenSource.Token, receiveBufferSize), loopCancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }

        public async Task DisconnectAsync(CancellationToken cancellationToken = default)
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

        public void OnMessageReceived(Func<string, Task> onMessageReceived, CancellationToken cancellationToken = default)
        {
            onMessageReceivedFunctions.Add(onMessageReceived);

            if (cancellationToken != CancellationToken.None)
            {
                var reg = cancellationToken.Register(() =>
                    onMessageReceivedFunctions.Remove(onMessageReceived));

                onMessageReceivedCancellationTokenRegistrations.Add(reg);
            }
        }

        public async Task SendApiAsync(string method, Dictionary<string, object> parameters = null, object requestId = null, CancellationToken cancellationToken = default)
        {
            parameters = ParamsWithApiKey(parameters);
            await SendAsync(method, parameters, requestId, cancellationToken);
        }

        public async Task SendSignedAsync(string method, Dictionary<string, object> parameters = null, object requestId = null, CancellationToken cancellationToken = default)
        {
            if (signatureService == null)
            {
                throw new ArgumentNullException("Initiate WebSocketApi with IBinanceSignatureService to perfom this request");
            }

            parameters = ParamsWithApiKey(parameters);
            parameters.Add("timestamp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            StringBuilder payloadBuilder = new StringBuilder();
            payloadBuilder = BuildPayload(parameters, payloadBuilder);
            string signature = signatureService.Sign(payloadBuilder.ToString());
            parameters.Add("signature", signature);
            await SendAsync(method, parameters, requestId, cancellationToken);
        }

        public async Task SendAsync(string method, Dictionary<string, object> parameters = null, object requestId = null, CancellationToken cancellationToken = default)
        {
            // Process requestId
            if (requestId is string && string.IsNullOrWhiteSpace((string)requestId) || requestId == null)
            {
                requestId = Guid.NewGuid().ToString();
            }
            else if (!(requestId is int || requestId is string || requestId == null))
            {
                throw new ArgumentException($"{requestId} must be of type int or string");
            }

            // Prepare request
            var jsonObject = new object();

            if (parameters is null)
            {
                jsonObject = new
                {
                    @id = requestId,
                    method,
                };
            }
            else
            {
                jsonObject = new
                {
                    @id = requestId,
                    method,
                    @params = ProcessRequestParams(parameters),
                };
            }

            string jsonRequest = JsonConvert.SerializeObject(jsonObject);

            byte[] byteArray = Encoding.ASCII.GetBytes(jsonRequest);

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

        private StringBuilder BuildPayload(Dictionary<string, object> parameters, StringBuilder builder)
        {
            foreach (KeyValuePair<string, object> param in parameters.OrderBy(p => p.Key))
            {
                string paramValue = Convert.ToString(param.Value, CultureInfo.InvariantCulture);
                if (!string.IsNullOrWhiteSpace(paramValue))
                {
                    if (builder.Length > 0)
                    {
                        builder.Append("&");
                    }

                    builder
                        .Append(param.Key)
                        .Append("=")
                        .Append(HttpUtility.UrlEncode(paramValue));
                }
            }

            return builder;
        }

        private Dictionary<string, object> ParamsWithApiKey(Dictionary<string, object> parameters = null)
        {
            if (apiKey == null)
            {
                throw new ArgumentNullException("Initiate WebSocketApi with apiKey to perfom this request");
            }

            if (parameters is null)
            {
                parameters = new Dictionary<string, object> { };
            }

            parameters.Add("apiKey", apiKey);
            return parameters;
        }

        private Dictionary<string, object> ProcessRequestParams(Dictionary<string, object> parameters)
        {
            var reqParameters = new Dictionary<string, object>();

            foreach (KeyValuePair<string, object> param in parameters)
            {
                if (param.Value != null && (param.Value is string || param.Value.GetType().IsArray))
                {
                    reqParameters.Add(param.Key, param.Value);
                }
                else
                {
                    if (param.Value != null)
                    {
                        string paramValue = Convert.ToString(param.Value);
                        reqParameters.Add(param.Key, paramValue);
                    }
                }
            }

            return reqParameters;
        }
    }
}