namespace BlockStorm.BinanceConnector.Spot.Models
{
    public struct FiatPaymentTransactionType
    {
        private FiatPaymentTransactionType(string value)
        {
            Value = value;
        }

        public static FiatPaymentTransactionType BUY { get => new FiatPaymentTransactionType("0"); }
        public static FiatPaymentTransactionType SELL { get => new FiatPaymentTransactionType("1"); }

        public string Value { get; private set; }

        public static implicit operator string(FiatPaymentTransactionType enm) => enm.Value;

        public override string ToString() => Value.ToString();
    }
}