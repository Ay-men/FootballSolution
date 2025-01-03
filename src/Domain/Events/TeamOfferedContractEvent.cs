namespace Domain.Events;

using Primitives;
using ValueObjects;

public sealed record TeamOfferedContractEvent(
    TeamId TeamId,
    PlayerId PlayerId,
    ContractDetails Details) : DomainEvent;