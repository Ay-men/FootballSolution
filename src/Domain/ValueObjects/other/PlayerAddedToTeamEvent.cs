using Domain.Entities.Common;
using Domain.Entities.ValueObjects;

namespace Domain.Events;

public class PlayerAddedToTeamEvent : DomainEvent
{
  public TeamId TeamId { get; }
  public PlayerId PlayerId { get; }

  public PlayerAddedToTeamEvent(TeamId teamId, PlayerId playerId)
  {
    TeamId = teamId;
    PlayerId = playerId;
  }

}