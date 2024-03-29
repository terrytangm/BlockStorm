namespace BlockStorm.BinanceConnector.Spot.Models
{
    public struct LoanDirection
    {
        private LoanDirection(string value)
        {
            Value = value;
        }

        public static LoanDirection ADDITIONAL { get => new LoanDirection("ADDITIONAL"); }
        public static LoanDirection REDUCED { get => new LoanDirection("REDUCED"); }

        public string Value { get; private set; }

        public static implicit operator string(LoanDirection enm) => enm.Value;

        public override string ToString() => Value.ToString();
    }
}