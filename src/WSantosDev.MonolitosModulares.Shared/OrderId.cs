using WSantosDev.MonolitosModulares.Commons.Modeling;

namespace WSantosDev.MonolitosModulares.Shared
{
    public sealed class OrderId : SimpleValue<Guid>
    {
        public static readonly OrderId Empty = new(Guid.Empty);

        private OrderId(Guid value) : base(value) { }

        public static OrderId New() => 
            new (Guid.NewGuid());

        public static implicit operator OrderId(Guid value) =>
            new(value);
    }
}