using WSantosDev.MonolitosModulares.Commons.Modeling;
using WSantosDev.MonolitosModulares.Commons.Results;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Orders
{
    public sealed class Trade
    {
        public static readonly Trade None = new(Quantity.Zero, Money.Zero);

        public Quantity Quantity { get; }
        public Money AveragePrice { get; }

        private Trade(Quantity quantity, Money averagePrice)
        {
            Quantity = quantity;
            AveragePrice = averagePrice;
        }

        internal static Result<Trade> Create(Quantity quantity, Money averagePrice)
        {
            return new Trade(quantity, averagePrice);
        }
    }
}