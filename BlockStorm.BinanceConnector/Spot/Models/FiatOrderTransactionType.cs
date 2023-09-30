namespace BlockStorm.BinanceConnector.Spot.Models
{
    public struct FiatOrderTransactionType
    {
        private FiatOrderTransactionType(string value)
        {
            Value = value;
        }

        public static FiatOrderTransactionType DEPOSIT { get => new FiatOrderTransactionType("0"); }
        public static FiatOrderTransactionType WITHDRAW { get => new FiatOrderTransactionType("1"); }

        public string Value { get; private set; }

        public static implicit operator string(FiatOrderTransactionType enm) => enm.Value;

        public override string ToString() => Value.ToString();
    }
}