namespace BlockStorm.BinanceConnector.Spot.Models
{
    public struct CrossMarginTransferType
    {
        private CrossMarginTransferType(string value)
        {
            Value = value;
        }

        public static CrossMarginTransferType ROLL_IN { get => new CrossMarginTransferType("ROLL_IN"); }
        public static CrossMarginTransferType ROLL_OUT { get => new CrossMarginTransferType("ROLL_OUT"); }

        public string Value { get; private set; }

        public static implicit operator string(CrossMarginTransferType enm) => enm.Value;

        public override string ToString() => Value.ToString();
    }
}