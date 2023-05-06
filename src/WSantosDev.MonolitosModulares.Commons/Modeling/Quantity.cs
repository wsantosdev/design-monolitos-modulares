namespace WSantosDev.MonolitosModulares.Commons.Modeling
{
    public sealed class Quantity : SimpleValue<int>
    {
        public static readonly Quantity Zero = new (0);

        private Quantity(int value) : base(value)
        {
            if(value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
        }

        public static implicit operator Quantity(int value) =>
            new(value);

        public static implicit operator int(Quantity quantity) =>
            quantity.Value;

        public static Quantity operator -(Quantity left, Quantity right)
        {
            if (left.Value - right.Value < 0)
                throw new ArithmeticException("Quantity cannot be lower than zero.");

            return (Quantity)(left.Value - right.Value);
        }

        public static Quantity operator +(Quantity left, Quantity right) =>
            checked((Quantity)(left.Value + right.Value));

        public static bool operator >(Quantity left, Quantity right) =>
            left.Value > right.Value;

        public static bool operator <(Quantity left, Quantity right) =>
            left.Value < right.Value;
    }
}
