namespace Domain.ValueObjects;

using System.Text.Json.Serialization;
using Domain.Exceptions;
using Primitives;

public class ContractDetails : ValueObject
{
  public DateTime StartDate { get; init; }
  public DateTime? EndDate { get; init; }
  public Money Salary { get; init; }

  protected ContractDetails() { }

  [JsonConstructor]
  public ContractDetails(DateTime startDate, Money salary, DateTime? endDate = null)
  {
    if (endDate.HasValue && endDate.Value <= startDate)
      throw new DomainException("Contract end date must be after start date");
    if (DateTime.Today >= startDate)
      throw new DomainException("Contract start date must be in the future");
    
    if (endDate.HasValue && endDate.Value <= startDate)
        throw new DomainException("Contract end date must be after start date");

    if (startDate > DateTime.UtcNow)
    {
        if (DateTime.UtcNow.AddYears(5) < startDate)
            throw new DomainException("Contract start date cannot be more than 5 years in the future");
    }
    else 
    {
        if (startDate < DateTime.UtcNow.AddYears(-50))
            throw new DomainException("Historical contracts cannot be older than 50 years");
    }

    if (endDate.HasValue && endDate.Value > DateTime.UtcNow.AddYears(10))
        throw new DomainException("Contract end date cannot be more than 10 years in the future");

    StartDate = startDate;
    EndDate = endDate;
    Salary = salary;
  }
  
  public override IEnumerable<object> GetAtomicValues()
  {
      yield return StartDate;
      yield return EndDate ?? DateTime.MaxValue;
      yield return Salary;
  }
}