using Domain.Entities.Common;
using Domain.Entities.ValueObjects;

namespace Domain.Events;

public class PlayerTransferredEvent : DomainEvent
{
  public PlayerId PlayerId { get; }
  public TeamId FromTeamId { get; }
  public TeamId ToTeamId { get; }
  public Money TransferFee { get; }
  public DateTime TransferDate { get; }

  public PlayerTransferredEvent(
      PlayerId playerId,
      TeamId fromTeamId,
      TeamId toTeamId,
      Money transferFee,
      DateTime transferDate)
  {
    PlayerId = playerId;
    FromTeamId = fromTeamId;
    ToTeamId = toTeamId;
    TransferFee = transferFee;
    TransferDate = transferDate;
  }
}