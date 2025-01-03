using Domain.Entities.Common;
using Domain.Entities.ValueObjects;

namespace Domain.Events;

public class PlayerCreatedEvent : DomainEvent
{
  public PlayerId PlayerId { get; }
  public TeamId? TeamId { get; }

  public PlayerCreatedEvent(PlayerId playerId, TeamId? teamId = null)
  {
    PlayerId = playerId;
    TeamId = teamId;
  }
}