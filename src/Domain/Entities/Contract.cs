namespace Domain.Entities;

using Domain.Primitives;
using Domain.ValueObjects;
using Domain.Exceptions;

public class Contract : Entity<Guid>
{
    private PlayerId _playerId;
    private TeamId _teamId;
    private ContractDetails _details;
    private Contract(Guid id, PlayerId playerId, TeamId teamId, ContractDetails details) : base(id)
    {
        _playerId = playerId;
        _teamId = teamId;
        _details = details;
    }
    
    public bool IsActive()
    {
        return
            _details.StartDate <= DateTime.UtcNow &&
            (!_details.EndDate.HasValue || _details.EndDate.Value > DateTime.UtcNow);
    }
    public void Terminate(DateTime terminationDate)
    {
        if (terminationDate <= _details.StartDate)
            throw new DomainException("Termination date must be after contract start date");
    }
    protected Contract(Guid id):base(id)
    {
    }
    
    public static Contract Create(
        Player player, 
        Team team, 
        ContractDetails details)
    {
        return new Contract(
            Guid.NewGuid(),
            player.Id,
            team.Id,
            details);
    }
    
    public PlayerId GetPlayerId() => _playerId;
    public TeamId GetTeamId() => _teamId;
    public decimal GetSalary() => _details.Salary.Amount;
    public string GetSalaryCurrency() => _details.Salary.Currency;
    public DateTime GetStartDate() => _details.StartDate;
    public DateTime? GetEndDate() => _details.EndDate;
}