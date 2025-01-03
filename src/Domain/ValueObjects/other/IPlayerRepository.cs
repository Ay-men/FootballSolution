using Domain.Entities;
using Domain.Entities.ValueObjects;

namespace Domain.Interfaces;

public interface IPlayerRepository
{
  public Task CreatePlayer(Player player);
  Task<Player> GetByIdAsync(PlayerId id);
  Task<(List<Player> Items, int TotalCount)> GetPlayersByTeamAsync(TeamId value, int pageNumber, int pageSize, string? searchTerm, string? sortBy, bool sortDescending);
  Task UpdateAsync(Player player);
  Task<Player> GetByIdWithTrackingAsync(PlayerId id);

}