using Domain.Entities;
using Domain.Entities.ValueObjects;

namespace Domain.Interfaces;

public interface ITransferRepository
{
  Task<TransferRecord> GetTransferByIdAsync(Guid id);
  Task<IEnumerable<TransferRecord>> GetTransferHistoryAsync(
      PlayerId? playerId = null,
      TeamId? teamId = null,
      DateTime? fromDate = null,
      DateTime? toDate = null);
  Task AddTransferAsync(TransferRecord transfer);
  Task SeedTransfersAsync();
}