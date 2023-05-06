using WSantosDev.MonolitosModulares.Commons.Modeling;

namespace WSantosDev.MonolitosModulares.Shared
{
    public sealed class OrderStatus : SimpleValue<string>
    {
        public static readonly OrderStatus Invalid = new(nameof(Invalid));
        public static readonly OrderStatus New = new(nameof(New));
        public static readonly OrderStatus Filled = new(nameof(Filled));
        public static readonly OrderStatus Canceled = new(nameof(Canceled));

        private OrderStatus(string value) : base(value) { }

        public static implicit operator OrderStatus(string value) =>
            value switch
            {
                nameof(New) => New,
                nameof(Filled) => Filled,
                nameof(Canceled) => Canceled,
                _ => Invalid
            };

        public static implicit operator string(OrderStatus status) =>
            status == Invalid
                ? nameof(Invalid)
                : status.Value;
    }
}
