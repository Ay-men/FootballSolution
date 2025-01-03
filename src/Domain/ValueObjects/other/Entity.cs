using Domain.Events;

namespace Domain.Entities.Common
{
  public abstract class Entity<TId> : IEntity
  {
    public TId Id { get; protected set; }
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
      _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
      _domainEvents.Clear();
    }

    public override bool Equals(object? obj)
    {
      if (obj is not Entity<TId> other)
        return false;

      if (ReferenceEquals(this, other))
        return true;

      if (GetType() != other.GetType())
        return false;

      return Id!.Equals(other.Id);
    }

    public override int GetHashCode()
    {
      return Id!.GetHashCode();
    }

    public static bool operator ==(Entity<TId> left, Entity<TId> right)
    {
      if (left is null && right is null)
        return true;

      if (left is null || right is null)
        return false;

      return left.Equals(right);
    }

    public static bool operator !=(Entity<TId> left, Entity<TId> right)
    {
      return !(left == right);
    }
  }
}