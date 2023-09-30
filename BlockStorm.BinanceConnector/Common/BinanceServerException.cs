namespace BlockStorm.BinanceConnector.Common
{
    using System;

    /// <summary>
    /// Binance exception class for any errors throw as a result internal server issues.
    /// </summary>
    public class BinanceServerException : BinanceHttpException
    {
        public BinanceServerException()
        : base()
        {
        }

        public BinanceServerException(string message)
        : base(message)
        {
            Message = message;
        }

        public BinanceServerException(string message, Exception innerException)
        : base(message, innerException)
        {
            Message = message;
        }

        public new string Message { get; protected set; }
    }
}