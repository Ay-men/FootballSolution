namespace Domain.ValueObjects;

using Primitives;

public class MarketValue : ValueObject
{
    public Money Value { get; }
    public DateTime ValidFrom { get; }
    
    private MarketValue(Money amount)
    {
        Value = amount ?? throw new ArgumentNullException(nameof(amount));
        ValidFrom = DateTime.UtcNow;
    }
    
    public static MarketValue Create(Money amount) => new(amount);
    
    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
        yield return ValidFrom;
    }
}