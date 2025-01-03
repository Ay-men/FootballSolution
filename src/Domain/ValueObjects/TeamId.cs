namespace Domain.ValueObjects;

using Domain.Exceptions;
using Primitives;

public class TeamId : ValueObject
{
  public Guid Value { get; }
  protected TeamId() { }

  public TeamId(Guid value)
  {
    if (value == Guid.Empty)
      throw new DomainException("TeamId cannot be empty");
    Value = value;
  }

  public static TeamId CreateUnique() => new(Guid.NewGuid());
  public static TeamId Create(Guid id) => new(id);

  public override IEnumerable<object> GetAtomicValues()
  {
    yield return Value;
  }

  public override string ToString() => Value.ToString();
}