using WSantosDev.MonolitosModulares.Commons.Results;

namespace WSantosDev.MonolitosModulares.Accounts
{
    public static class Errors
    {
        public static readonly EmptyIdError EmptyId;
        public static readonly InvalidAmountError InvalidAmount;
        public static readonly InsuficientFundsError InsuficientFunds;
    }

    public readonly struct EmptyIdError : IError { }
    public readonly struct InvalidAmountError : IError { }
    public readonly struct InsuficientFundsError : IError { }
}
