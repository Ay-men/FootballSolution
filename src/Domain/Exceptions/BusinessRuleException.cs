namespace Domain.Exceptions;

public class BusinessRuleException : DomainException
{
  public string Details { get; }

  public BusinessRuleException(string message)
      : base(message)
  {
  }

  public BusinessRuleException(string message, string details)
      : base(message)
  {
    Details = details;
  }
}
