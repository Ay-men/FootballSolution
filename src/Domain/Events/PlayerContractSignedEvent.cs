namespace Domain.Events;

using Primitives;
using ValueObjects;

public sealed record PlayerContractSignedEvent(
    PlayerId PlayerId,
    TeamId TeamId,
    ContractDetails Details) : DomainEvent;
