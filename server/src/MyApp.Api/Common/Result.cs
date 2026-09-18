namespace MyApp.Common;

public enum ErrorType
{
    None = 0,
    Validation = 1,
    Unauthorized = 2,
    Forbidden = 3,
    NotFound = 4,
    Conflict = 5,
    Lockout = 6,
    RateLimit = 7,
    Internal = 8,
}

public sealed record Error(string Code, string Message, ErrorType Type)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);

    public static Error Validation(string code, string message) => new(code, message, ErrorType.Validation);

    public static Error Unauthorized(string message) => new("unauthorized", message, ErrorType.Unauthorized);

    public static Error Forbidden(string message) => new("forbidden", message, ErrorType.Forbidden);

    public static Error NotFound(string message) => new("not_found", message, ErrorType.NotFound);

    public static Error Conflict(string message) => new("conflict", message, ErrorType.Conflict);

    public static Error Lockout(string message) => new("lockout", message, ErrorType.Lockout);

    public static Error RateLimit(string message) => new("rate_limit", message, ErrorType.RateLimit);

    public static Error Internal(string message) => new("internal_error", message, ErrorType.Internal);
}

public sealed class Result
{
    private Result(bool isSuccess, Error? error = null)
    {
        IsSuccess = isSuccess;
        Error = error ?? Error.None;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true);

    public static Result Failure(Error error) => new(false, error);

    public static implicit operator Result(Error error) => Failure(error);
}

public sealed class Result<T>
{
    private Result(bool isSuccess, T? value = default, Error? error = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error ?? Error.None;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public T? Value { get; }

    public Error Error { get; }

    public static Result<T> Success(T value) => new(true, value);

    public static Result<T> Failure(Error error) => new(false, default, error);

    public static implicit operator Result<T>(T value) => Success(value);

    public static implicit operator Result<T>(Error error) => Failure(error);
}
