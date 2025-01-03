using Domain.Exceptions;

namespace Domain.ValueObjects;

using Primitives;

public class Height : ValueObject
{
  public decimal Value { get; }
  protected Height() { }

  private Height(decimal value)
  {
    if (value is < 1.40m or > 2.50m)
      throw new DomainException("Invalid height value");
    Value = value;
  }

  public static Height Create(decimal value) => new(value);

  public override IEnumerable<object> GetAtomicValues()
  {
    yield return Value;
  }
}