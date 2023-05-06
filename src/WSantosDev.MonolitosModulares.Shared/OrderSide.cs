using WSantosDev.MonolitosModulares.Commons.Modeling;

namespace WSantosDev.MonolitosModulares.Shared
{
    public sealed class OrderSide : SimpleValue<string>
    {
        public static readonly OrderSide Invalid = new(nameof(Invalid));
        public static readonly OrderSide Buy = new(nameof(Buy));
        public static readonly OrderSide Sell = new(nameof(Sell));

        private OrderSide(string value) : base(value) { }

        public static implicit operator OrderSide(string value) =>
            value switch
            {
                nameof(Buy) => Buy,
                nameof(Sell) => Sell,
                _ => Invalid
            };

        public static implicit operator string(OrderSide side) =>
            side.Value;

        public static bool operator ==(string left, OrderSide right) =>
            left == right.Value;

        public static bool operator !=(string left, OrderSide right) =>
            !(left == right);

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not string)
                return false;

            return base.Equals(obj);
        }

        public override int GetHashCode() 
            => base.GetHashCode();
    }
}
