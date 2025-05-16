namespace WpfApp1.Api
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Error { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string? error)
        {
            if (isSuccess && !string.IsNullOrEmpty(error))
                throw new InvalidOperationException("Success result cannot have an error message");
            if (!isSuccess && string.IsNullOrEmpty(error))
                throw new InvalidOperationException("Failure result must have an error message");

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new Result(true, null);
        public static Result Failure(string error) => new Result(false, error);
        public static Result<T> Success<T>(T value) => Result<T>.Success(value);
        public static Result<T> Failure<T>(string error) => Result<T>.Failure(error);

        public static Result From(Func<bool> func, string errorMessage)
        {
            try
            {
                return func() ? Success() : Failure(errorMessage);
            }
            catch (Exception ex)
            {
                return Failure($"{errorMessage}: {ex.Message}");
            }
        }
    }
}