using WSantosDev.MonolitosModulares.Commons.Modeling;
using WSantosDev.MonolitosModulares.Commons.Results;
using WSantosDev.MonolitosModulares.Shared;

namespace WSantosDev.MonolitosModulares.Portfolios
{
    public sealed class Entry
    {
        public static readonly Entry Empty = new(Symbol.Empty, quantity: Quantity.Zero, 
                                                 blockedQuantity: Quantity.Zero);

        public Symbol Symbol { get; }
        public Quantity Quantity { get; private set; }
        public Quantity BlockedQuantity { get; private set; }

        private Entry(Symbol symbol, Quantity quantity, Quantity blockedQuantity)
        {
            Symbol = symbol;
            Quantity = quantity;
            BlockedQuantity = blockedQuantity;
        }

        internal static Result<Entry> Create(Symbol symbol, Quantity quantity)
        {
            if (symbol == Symbol.Empty)
                return Result<Entry>.Fail(Errors.InvalidSymbol);
            if(quantity == Quantity.Zero)
                return Result<Entry>.Fail(Errors.InvalidQuantity);

            return new Entry(symbol, quantity, Quantity.Zero);
        }

        internal Result Add(Quantity quantity)
        {
            if (quantity == Quantity.Zero)
                return Result.Fail(Errors.InvalidQuantity);

            Quantity += quantity;
            return Result.Success;
        }

        internal Result Subtract(Quantity quantity) 
        {
            if (quantity == Quantity.Zero)
                return Result.Fail(Errors.InvalidQuantity);

            if (Quantity - quantity < Quantity.Zero)
                return Result.Fail(Errors.InsuficientAssets);

            Quantity -= quantity;
            return Result.Success;
        }
    }
}
