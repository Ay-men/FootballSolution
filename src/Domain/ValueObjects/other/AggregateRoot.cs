namespace Domain.Entities.Common;

public abstract class AggregateRoot<TId> : IAggregateRoot where TId : class
{
    private readonly List<IDomainEvent> _domainEvents = new();

    private DateTime _createdAt;
    private DateTime? _updatedAt;

    protected AggregateRoot()
    {
        _createdAt = DateTime.UtcNow;
    }

    public TId Id { get; protected set; }
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void Update()
    {
        _updatedAt = DateTime.UtcNow;
    }
}