using WSantosDev.MonolitosModulares.Commons.Results;

namespace WSantosDev.MonolitosModulares.Orders
{
    internal static class Errors
    {
        public static readonly InvalidSideError InvalidSide;
        public static readonly InvalidQuantityError InvalidQuantity;
        public static readonly InvalidSymbolError InvalidSymbol;
        public static readonly InvalidPriceError InvalidPrice;

        public static readonly AlreadyCanceledError AlreadyCanceled;
        public static readonly AlreadyFilledError AlreadyFilled;
    }

    public readonly struct InvalidSideError : IError { }
    public readonly struct InvalidQuantityError : IError { }
    public readonly struct InvalidSymbolError : IError { }
    public readonly struct InvalidPriceError : IError { }

    public readonly struct AlreadyCanceledError : IError { }
    public readonly struct AlreadyFilledError : IError { }
}
