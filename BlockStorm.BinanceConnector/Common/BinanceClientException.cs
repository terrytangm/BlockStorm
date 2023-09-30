namespace BlockStorm.BinanceConnector.Common
{
    using System;

    /// <summary>
    /// Binance exception class for any errors throw as a result of the misuse of the API or the library.
    /// </summary>
    public class BinanceClientException : BinanceHttpException
    {
        public BinanceClientException()
        : base()
        {
        }

        public BinanceClientException(string message, int code)
        : base(message)
        {
            Code = code;
            Message = message;
        }

        public BinanceClientException(string message, int code, Exception innerException)
        : base(message, innerException)
        {
            Code = code;
            Message = message;
        }

        [Newtonsoft.Json.JsonProperty("code")]
        public int Code { get; set; }

        [Newtonsoft.Json.JsonProperty("msg")]
        public new string Message { get; protected set; }
    }
}