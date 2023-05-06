namespace WSantosDev.MonolitosModulares.Commons.Results
{
    public readonly struct Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsError => !IsSuccess;

        private readonly IError _error;
        public IError Error =>
            IsError
            ? _error
            : throw ResultExceptions.NoErrorAvailable;

        private readonly T _value;
        public T Value =>
            IsSuccess
            ? _value
            : throw ResultExceptions.NoValueAvailable;

        private Result(bool isSuccess, T value, IError error) =>
            (IsSuccess, _value, _error) = (isSuccess, value, error);

        public static Result<T> Success(T value) =>
            new(true, value, default(NoError));

        public static Result<T> Fail(IError error) =>
            new(false, default!, error);

        public static implicit operator Result(Result<T> result)
        {
            if (result.IsSuccess)
                return Result.Success;

            return Result.Fail(result.Error);
        }

        public static implicit operator Result<T>(T value) =>
            Success(value);

        public static implicit operator bool(Result<T> result) =>
            result.IsSuccess;
    }
}
