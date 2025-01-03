using Domain.Entities;
using Domain.Entities.ValueObjects;

namespace Domain.Interfaces;

public interface ITeamRepository
{
  Task<Team> GetTeamDetailsById(TeamId id);
  Task<IEnumerable<Team>> GetAllAsync();
  Task<IEnumerable<Player>> GetActiveSquadAsync(TeamId teamId);
  Task<Team> GetTeamDetailsByIdWithTrackingAsync(TeamId id);
  Task AddAsync(Team team);
  Task UpdateAsync(Team team);
}