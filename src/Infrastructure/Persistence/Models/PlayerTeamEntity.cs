namespace Infrastructure.Persistence.Models;

public class PlayerTeamEntity
{
    public PlayerTeamEntity(){}
    public Guid Id { get; init; }
    public Guid PlayerId { get; init; }
    public Guid TeamId { get; init; }
    
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; set; } 
    public decimal Salary { get; init; }
    public string SalaryCurrency { get; init; } = default!;
    public decimal? TransferFee { get; init; }
    public string? TransferFeeCurrency { get; init; }
    
    public int Appearances { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }
    
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; set; }
    
    public virtual PlayerEntity PlayerEntity { get; init; } = default!;
    public virtual TeamEntity TeamEntity { get; init; } = default!;
}