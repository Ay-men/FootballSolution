namespace Application.Common.Interfaces;

using Domain.Entities;
using Domain.ValueObjects;
using Models;
using Paging;

public interface IPlayerRepository
{
    Task<Player?> GetPlayerByIdAsync(PlayerId id, CancellationToken cancellationToken = default);
    Task<List<Player>> GetByTeamAsync(TeamId teamId, CancellationToken cancellationToken = default);
    Task<PagedResult<PlayerResponse>> GetPlayersByTeamPagedAsync(
        TeamId teamId,
        int page,
        int pageSize,
        string? searchTerm,
        string? sortBy,
        bool sortDescending,
        CancellationToken cancellationToken = default);
    Task AddAsync(Player player, CancellationToken cancellationToken = default);
    Task UpdateAsync(Player player, CancellationToken cancellationToken = default);
}