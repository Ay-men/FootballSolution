using Domain.Entities.Common;
using Domain.Entities.ValueObjects;

namespace Domain.Events;

public class PlayerCreatedAsFreeAgentEvent : DomainEvent
{
  public PlayerId PlayerId { get; }

  public PlayerCreatedAsFreeAgentEvent(PlayerId playerId)
  {
    PlayerId = playerId;
  }
}