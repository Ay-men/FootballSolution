namespace Domain.Primitives;

using Interfaces;

public class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => [.. _domainEvents];
    private DateTime _createdAt;
    private DateTime? _updatedAt;

    protected AggregateRoot(TId id) : base(id)
    {
        _createdAt = DateTime.UtcNow;
    }
    
    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);
    
    protected void AddDomainEvent(IDomainEvent domainEvent) =>
        _domainEvents.Add(domainEvent);
    
    protected void Update()
    {
        _updatedAt = DateTime.UtcNow;
    }
}