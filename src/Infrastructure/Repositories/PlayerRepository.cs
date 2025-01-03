namespace Infrastructure.Repositories;

using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Paging;
using Domain.Entities;
using Domain.Exceptions;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Models;
using AutoMapper;

public class PlayerRepository : IPlayerRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PlayerRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Player?> GetPlayerByIdAsync(PlayerId id, CancellationToken cancellationToken = default)
    {
        var playerEntity = await _context.Players
            .Include(p => p.TeamAssociations)
            .ThenInclude(pt => pt.TeamEntity)
            .FirstOrDefaultAsync(p => p.Id == id.Value, cancellationToken);

        if (playerEntity != null)
        {
            return _mapper.Map<Player>(playerEntity);
        }

        playerEntity = await _context.Players
            .FirstOrDefaultAsync(p => p.Id == id.Value, cancellationToken);
// && !p.TeamAssociations.Any()
        return playerEntity != null ? _mapper.Map<Player>(playerEntity) : null;
    }
    public async Task<List<Player>> GetByTeamAsync(TeamId teamId, CancellationToken cancellationToken = default)
    {
        var playerEntities = await _context.Players
            .Include(p => p.TeamAssociations)
            .Where(p => p.TeamAssociations.Any(ta => 
                ta.TeamId == teamId.Value && 
                ta.StartDate <= DateTime.UtcNow && 
                (!ta.EndDate.HasValue || ta.EndDate > DateTime.UtcNow)))
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<Player>>(playerEntities);
    }

    public async Task<PagedResult<PlayerResponse>> GetPlayersByTeamPagedAsync(
        TeamId teamId,
        int page,
        int pageSize,
        string? searchTerm,
        string? sortBy,
        bool sortDescending,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Players
            .Include(p => p.TeamAssociations)
            .Where(p => p.TeamAssociations.Any(ta => 
                ta.TeamId == teamId.Value && 
                ta.StartDate <= DateTime.UtcNow && 
                (!ta.EndDate.HasValue || ta.EndDate > DateTime.UtcNow)));

        // Apply search
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p => 
                p.FirstName.Contains(searchTerm) || 
                p.LastName.Contains(searchTerm) || 
                p.Nationality.Contains(searchTerm));
        }

        // Apply sorting
        query = ApplySorting(query, sortBy, sortDescending);

        // // Get total count
        // var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination
        var players = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<PlayerResponse>
        {
            Results =_mapper.Map<List<PlayerResponse>>(players), 
            PageSize = pageSize
        };
    }

    private static IQueryable<PlayerEntity> ApplySorting(
        IQueryable<PlayerEntity> query,
        string? sortBy,
        bool sortDescending)
    {
        query = sortBy?.ToLower() switch
        {
            "name" => sortDescending 
                ? query.OrderByDescending(p => p.LastName).ThenByDescending(p => p.FirstName)
                : query.OrderBy(p => p.LastName).ThenBy(p => p.FirstName),
            "nationality" => sortDescending 
                ? query.OrderByDescending(p => p.Nationality)
                : query.OrderBy(p => p.Nationality),
            "position" => sortDescending 
                ? query.OrderByDescending(p => p.Position)
                : query.OrderBy(p => p.Position),
            "marketvalue" => sortDescending 
                ? query.OrderByDescending(p => p.MarketValue)
                : query.OrderBy(p => p.MarketValue),
            _ => query.OrderBy(p => p.LastName)
        };

        return query;
    }

    public async Task AddAsync(Player player, CancellationToken cancellationToken = default)
    {
        var playerEntity = _mapper.Map<PlayerEntity>(player);
        await _context.Players.AddAsync(playerEntity, cancellationToken);
    }

    public async Task UpdateAsync(Player player, CancellationToken cancellationToken = default)
    {
        var existingEntity = await _context.Players
            .Include(p => p.TeamAssociations)
            .FirstOrDefaultAsync(p => p.Id == player.Id.Value, cancellationToken);

        if (existingEntity == null)
            throw new NotFoundException($"Player with ID {player.Id} not found");

        _mapper.Map(player, existingEntity);
    }
}
