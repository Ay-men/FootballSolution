namespace Domain.Events;

using Primitives;
using ValueObjects;

public sealed record TeamCreatedEvent(
    TeamId TeamId) : DomainEvent;