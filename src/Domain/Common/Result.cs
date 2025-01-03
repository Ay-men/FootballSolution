namespace Domain.Common;

public class Result
{
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException();
            
        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException();
            
        IsSuccess = isSuccess;
        Error = error;
    }
    
    public bool IsSuccess { get; }
    public Error Error { get; }
    
    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;
    private readonly Error _error;

    protected Result(TValue? value, bool isSuccess, Error error) 
        : base(isSuccess, error)
    {
        _value = value;
    }
    
    public TValue Value => IsSuccess 
        ? _value! 
        : throw new InvalidOperationException("Cannot access value of a failed result.");
        
    public static Result<TValue> Success(TValue value) => 
        new(value, true, Error.None);
        
    public static Result<TValue> Failure(Error error) => 
        new(default, false, error);
}