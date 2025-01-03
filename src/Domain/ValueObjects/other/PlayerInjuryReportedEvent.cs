using Domain.Entities.Common;
using Domain.Entities.ValueObjects;

namespace Domain.Events;

public class PlayerInjuryReportedEvent : DomainEvent
{
  public PlayerId PlayerId { get; }
  public string InjuryDetails { get; }

  public PlayerInjuryReportedEvent(PlayerId playerId, string details)
  {
    PlayerId = playerId;
    InjuryDetails = details;
  }
}

public class PlayerInjuryClearedEvent : DomainEvent
{
  public PlayerId PlayerId { get; }

  public PlayerInjuryClearedEvent(PlayerId playerId)
  {
    PlayerId = playerId;
  }
}

public class TeamTitleWonEvent : DomainEvent
{
  public TeamId TeamId { get; }
  public bool IsLeague { get; }

  public TeamTitleWonEvent(TeamId teamId, bool isLeague)
  {
    TeamId = teamId;
    IsLeague = isLeague;
  }
}