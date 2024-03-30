using WSantosDev.MonolitosModulares.Commons.Modeling;

namespace WSantosDev.MonolitosModulares.Portfolios
{
    public sealed class PortfolioId : SimpleValue<Guid>
    {
        public static readonly PortfolioId Empty = new(Guid.Empty);

        public PortfolioId(Guid value) : base(value) { }

        public static PortfolioId New() =>
            new(Guid.NewGuid());

        public static implicit operator PortfolioId(Guid value) =>
            new(value);
    }
}