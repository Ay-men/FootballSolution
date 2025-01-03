using Domain.Entities.Common;
using Domain.Entities.ValueObjects;

namespace Domain.Events;

public class TeamCreatedEvent : DomainEvent
{
  public TeamId TeamId { get; }

  public TeamCreatedEvent(TeamId teamId)
  {
    TeamId = teamId;
  }
}