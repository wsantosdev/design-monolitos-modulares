namespace WSantosDev.MonolitosModulares.Commons.Results
{
    public readonly struct Result
    {
        public static readonly Result Success = new(true, default(NoError));

        public bool IsSuccess { get; }
        public bool IsError => !IsSuccess;

        private readonly IError _error;
        public IError Error =>
            IsError
            ? _error
            : throw ResultExceptions.NoErrorAvailable;

        private Result(bool isSuccess, IError error) =>
            (IsSuccess, _error) = (isSuccess, error);

        public static Result Fail(IError error) =>
            new(false, error);

        public static implicit operator bool(Result result)
            => result.IsSuccess;
    }
}
