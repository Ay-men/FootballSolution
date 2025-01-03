namespace Application.Common.Interfaces;

using Domain.Entities;
using Domain.ValueObjects;

public interface IContractRepository
{
    Task<Contract?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Contract>> GetActiveContractsForTeamAsync(TeamId teamId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Contract>> GetContractHistoryAsync(PlayerId playerId, CancellationToken cancellationToken = default);
    Task<Contract?> GetActiveContractForPlayerAsync(PlayerId playerId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(PlayerId playerId, DateTime startDate, CancellationToken cancellationToken = default);
    Task AddAsync(Contract contract, CancellationToken cancellationToken = default);
    Task UpdateAsync(Contract contract, CancellationToken cancellationToken = default);
}