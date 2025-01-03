namespace Domain.Events;

using Primitives;
using ValueObjects;

public sealed record PlayerMarketValueUpdatedEvent(
    PlayerId PlayerId,
    MarketValue NewValue) : DomainEvent;