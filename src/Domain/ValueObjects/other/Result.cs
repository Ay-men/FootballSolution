namespace Domain.Entities.Common;

using System.Text.Json.Serialization;

public class Result<T>
{
    [JsonConstructor]
    private Result(bool isSuccess, T value, string error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, string.Empty);
    }


    public static Result<T> Failure(string error)
    {
        return new Result<T>(false, default!, error);
    }
}