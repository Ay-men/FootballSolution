namespace Domain.Common;

public sealed record Error
{
    public string Code { get; }
    public string Message { get; }

    private Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static readonly Error None = new(string.Empty, string.Empty);

    public static Error NotFound(string message) => new("NotFound", message);
    
    public static Error Validation(string message) => new("Validation", message);
    
    public static Error Conflict(string message) => new("Conflict", message);
}
