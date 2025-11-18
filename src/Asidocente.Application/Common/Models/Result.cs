namespace Asidocente.Application.Common.Models;

/// <summary>
/// Generic result pattern for operation outcomes
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public string[] Errors { get; }

    protected Result(bool isSuccess, string[] errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Success()
    {
        return new Result(true, Array.Empty<string>());
    }

    public static Result Failure(params string[] errors)
    {
        return new Result(false, errors);
    }
}

/// <summary>
/// Generic result pattern with return value
/// </summary>
public class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, string[] errors)
        : base(isSuccess, errors)
    {
        Value = value;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, Array.Empty<string>());
    }

    public new static Result<T> Failure(params string[] errors)
    {
        return new Result<T>(false, default, errors);
    }
}
