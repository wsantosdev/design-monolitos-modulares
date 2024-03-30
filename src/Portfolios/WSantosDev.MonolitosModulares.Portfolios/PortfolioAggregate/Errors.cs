using WSantosDev.MonolitosModulares.Commons.Results;

namespace WSantosDev.MonolitosModulares.Portfolios
{
    public static class Errors
    {
        public static readonly InvalidAccountIdError InvalidAccountId;
        public static readonly InvalidSymbolError InvalidSymbol;
        public static readonly InvalidQuantityError InvalidQuantity;
        public static readonly InsuficientAssetsError InsuficientAssets;
    }

    public readonly struct InvalidAccountIdError : IError { }
    public readonly struct InvalidSymbolError : IError { }
    public readonly struct InvalidQuantityError : IError { }
    public readonly struct InsuficientAssetsError : IError { }
}
