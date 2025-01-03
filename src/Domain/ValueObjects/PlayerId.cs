namespace Domain.ValueObjects;

using Exceptions;
using Primitives;

public class PlayerId : ValueObject
{
    protected PlayerId()
    {
    }

    private PlayerId(Guid value)
    {
        if (value == Guid.Empty)
            throw new DomainException("PlayerId cannot be empty");
        Value = value;
    }

    public Guid Value { get; }

    public static PlayerId CreateUnique()
    {
        return new PlayerId(Guid.NewGuid());
    }

    public static PlayerId Create(Guid id)
    {
        return new PlayerId(id);
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value.ToString();
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}