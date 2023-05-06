using WSantosDev.MonolitosModulares.Commons.Results;

namespace WSantosDev.MonolitosModulares.Exchange
{
    public static class Errors
    {
        public static readonly AlreadyCanceledError AlreadyCanceled;
        public static readonly AlreadyFilledError AlreadyFilled;
    }

    public readonly struct AlreadyCanceledError : IError { }
    public readonly struct AlreadyFilledError : IError { }
}
