namespace Infrastructure.Repositories;

using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Models;


public class TeamRepository : ITeamRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public TeamRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Team?> GetByIdAsync(TeamId id, CancellationToken cancellationToken = default)
    {
        var teamEntity = await _context.Teams
            .Include(t => t.PlayerAssociations)
            .ThenInclude(pt => pt.PlayerEntity)
            .FirstOrDefaultAsync(t => t.Id == id.Value, cancellationToken);
        
        return teamEntity != null ? _mapper.Map<Team>(teamEntity) : null;
    }

    public async Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Teams
            .AnyAsync(t => t.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task AddAsync(Team team, CancellationToken cancellationToken = default)
    {
        var teamEntity = _mapper.Map<TeamEntity>(team);
        await _context.Teams.AddAsync(teamEntity, cancellationToken);
    }

    public async Task UpdateAsync(Team team, CancellationToken cancellationToken = default)
    {
        var existingEntity = await _context.Teams
            .Include(t => t.PlayerAssociations)
            .FirstOrDefaultAsync(t => t.Id == team.Id.Value, cancellationToken);

        if (existingEntity == null)
            throw new NotFoundException($"Team with ID {team.Id} not found");

        _mapper.Map(team, existingEntity);
    }
}
