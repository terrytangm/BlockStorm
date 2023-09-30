namespace BlockStorm.BinanceConnector.Common
{
    /// <summary>
    /// Interface for signing payloads.
    /// </summary>
    public interface IBinanceSignatureService
    {
        string Sign(string payload);
    }
}