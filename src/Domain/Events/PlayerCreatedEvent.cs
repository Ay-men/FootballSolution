namespace Domain.Events;

using Primitives;
using ValueObjects;

public sealed record PlayerCreatedEvent(
    PlayerId PlayerId) : DomainEvent;