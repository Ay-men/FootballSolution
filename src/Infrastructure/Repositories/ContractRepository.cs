namespace Infrastructure.Repositories;

using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Models;

public class ContractRepository : IContractRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ContractRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Contract?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var contractEntity = await _context.PlayerTeams
            .Include(pt => pt.PlayerEntity)
            .Include(pt => pt.TeamEntity)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        var t = _mapper.Map<Contract>(contractEntity);
        return contractEntity != null ? _mapper.Map<Contract>(contractEntity) : null;
    }

    public async Task<IEnumerable<Contract>> GetActiveContractsForTeamAsync(
        TeamId teamId,
        CancellationToken cancellationToken = default)
    {
        var currentDate = DateTime.UtcNow;
        var contractEntities = await _context.PlayerTeams
            .Include(pt => pt.PlayerEntity)
            .Include(pt => pt.TeamEntity)
            .Where(pt => 
                pt.TeamId == teamId.Value &&
                pt.StartDate <= currentDate &&
                (!pt.EndDate.HasValue || pt.EndDate > currentDate))
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<Contract>>(contractEntities);
    }

    public async Task<IEnumerable<Contract>> GetContractHistoryAsync(
        PlayerId playerId,
        CancellationToken cancellationToken = default)
    {
        var contractEntities = await _context.PlayerTeams
            .Include(pt => pt.PlayerEntity)
            .Include(pt => pt.TeamEntity)
            .Where(pt => pt.PlayerId == playerId.Value)
            .OrderByDescending(pt => pt.StartDate)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<Contract>>(contractEntities);
    }

    public async Task AddAsync(Contract contract, CancellationToken cancellationToken = default)
    {
        var contractEntity = _mapper.Map<PlayerTeamEntity>(contract);
        
        // Set audit fields
        // contractEntity.CreatedOnUtc = DateTime.UtcNow;
        // contractEntity.ModifiedOnUtc = DateTime.UtcNow;

        await _context.PlayerTeams.AddAsync(contractEntity, cancellationToken);
    }

    public async Task UpdateAsync(Contract contract, CancellationToken cancellationToken = default)
    {
        var existingEntity = await _context.PlayerTeams
            .FirstOrDefaultAsync(pt => pt.Id == contract.Id, cancellationToken);

        if (existingEntity == null)
            throw new NotFoundException($"Contract with ID {contract.Id} not found");

        _mapper.Map(contract, existingEntity);
        
        // Update audit field
        // existingEntity.ModifiedOnUtc = DateTime.UtcNow;

        _context.PlayerTeams.Update(existingEntity);
    }

    public async Task<bool> ExistsAsync(
        PlayerId playerId,
        DateTime startDate,
        CancellationToken cancellationToken = default)
    {
        return await _context.PlayerTeams
            .AnyAsync(pt => 
                pt.PlayerId == playerId.Value &&
                pt.StartDate <= startDate &&
                (!pt.EndDate.HasValue || pt.EndDate > startDate), 
                cancellationToken);
    }

    public async Task<Contract?> GetActiveContractForPlayerAsync(
        PlayerId playerId,
        CancellationToken cancellationToken = default)
    {
        var currentDate = DateTime.UtcNow;
        var contractEntity = await _context.PlayerTeams
            .Include(pt => pt.PlayerEntity)
            .Include(pt => pt.TeamEntity)
            .FirstOrDefaultAsync(pt =>
                pt.PlayerId == playerId.Value &&
                pt.StartDate <= currentDate &&
                (!pt.EndDate.HasValue || pt.EndDate > currentDate),
                cancellationToken);

        return contractEntity != null ? _mapper.Map<Contract>(contractEntity) : null;
    }
    
}