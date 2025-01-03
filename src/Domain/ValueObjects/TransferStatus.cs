using Domain.Exceptions;

namespace Domain.ValueObjects;

using Primitives;

public class TransferStatus : ValueObject
{
  public string Value { get; }

  private TransferStatus(string value)
  {
    Value = value;
  }

  public static TransferStatus FreeAgent => new("FreeAgent");
  public static TransferStatus Contracted => new("Contracted");

  public static TransferStatus FromValue(string value)
  {
    return value switch
    {
      "FreeAgent" => FreeAgent,
      "Contracted" => Contracted,
      _ => throw new DomainException($"Invalid transfer status: {value}")
    };
  }

  public override IEnumerable<object> GetAtomicValues()
  {
    yield return Value;
  }
}