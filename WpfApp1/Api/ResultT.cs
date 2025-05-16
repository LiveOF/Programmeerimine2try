namespace WpfApp1.Api
{
    public class Result<T> : Result
    {
        private readonly T? _value;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException("Cannot access value of a failed result");
                return _value!;
            }
        }

        protected internal Result(bool isSuccess, string? error, T? value) : base(isSuccess, error)
        {
            _value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(true, null, value);
        public new static Result<T> Failure(string error) => new Result<T>(false, error, default);

        public static implicit operator Result<T>(T value) => Success(value);

        public static Result<T> From(Func<T> func, string errorMessage)
        {
            try
            {
                return Success(func());
            }
            catch (Exception ex)
            {
                return Failure($"{errorMessage}: {ex.Message}");
            }
        }
    }
}