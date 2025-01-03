namespace Domain.Events;

using Primitives;
using ValueObjects;

public sealed record TeamBudgetUpdatedEvent(
    TeamId TeamId,
    Money NewBudget) : DomainEvent;