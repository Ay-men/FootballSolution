namespace Infrastructure.Persistence.Models;

using System.Collections.ObjectModel;

public record PlayerEntity//: DataEntityBase<Guid>
{
    public PlayerEntity(){}

    public Guid Id { get; init; }
    
    // Core data that rarely changes - init-only
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public DateTime DateOfBirth { get; init; }
    public string Nationality { get; init; } = default!;
    public string PassportNumber { get; init; } = default!;
    
    // Data that might change - normal setters
    public decimal Height { get; set; }
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public int Position { get; set; }
    public int PreferredFoot { get; set; }
    public decimal MarketValue { get; set; }
    public bool IsInjured { get; set; }
    public string? InjuryDetails { get; set; }
    
    // Statistics - normal setters as they update frequently
    public int TotalAppearances { get; set; }
    public int TotalGoals { get; set; }
    public int TotalAssists { get; set; }
    
    // Audit fields - init-only for creation, setter for updates
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation property - virtual for EF lazy loading
    public virtual ICollection<PlayerTeamEntity> TeamAssociations { get; init; } = 
        new Collection<PlayerTeamEntity>();
}