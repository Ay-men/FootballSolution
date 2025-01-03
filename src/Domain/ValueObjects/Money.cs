namespace Domain.ValueObjects;

using System.Text.Json.Serialization;
using Exceptions;
using Primitives;

public class Money : ValueObject
{
    protected Money()
    {
    }
    public static Money Create(decimal amount, string currency)  
        => new Money(amount, currency);
    
    [JsonConstructor]
    private Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new DomainException("Amount cannot be negative");
        
        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; init; }
    public string Currency { get; init; }


    public Money Subtract(Money other)
    {
        if (other.Currency != Currency)
            throw new DomainException("Cannot subtract different currencies");

        return Create(Amount - other.Amount, Currency);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Amount;
        yield return Currency;
    }

    public Money Add(Money other)
    {
        if (other.Currency != Currency)
            throw new DomainException("Cannot add different currencies");

        return Create(Amount + other.Amount, Currency);
    }
}