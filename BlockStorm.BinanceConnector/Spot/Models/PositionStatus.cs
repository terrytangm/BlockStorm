namespace BlockStorm.BinanceConnector.Spot.Models
{
    public struct PositionStatus
    {
        private PositionStatus(string value)
        {
            Value = value;
        }

        public static PositionStatus HOLDING { get => new PositionStatus("HOLDING"); }
        public static PositionStatus REDEEMED { get => new PositionStatus("REDEEMED"); }

        public string Value { get; private set; }

        public static implicit operator string(PositionStatus enm) => enm.Value;

        public override string ToString() => Value.ToString();
    }
}