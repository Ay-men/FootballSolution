namespace Infrastructure.Persistence.Models;

using System.Collections.ObjectModel;

public record TeamEntity
{
    public TeamEntity(){}

    public Guid Id { get; set; }
    
    // Core identity - init-only
    public string Name { get; init; } = default!;
    public string? City { get; init; }
    public string? Country { get; init; }
    public int? FoundedYear { get; init; }
    
    // Changeable properties
    public string? Stadium { get; set; }
    public decimal Budget { get; set; }
    public string Currency { get; set; } = default!;
    public int LeagueTitles { get; set; }
    public int CupTitles { get; set; }
    
    // Audit fields
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties - virtual for EF lazy loading
    public virtual ICollection<PlayerTeamEntity> PlayerAssociations { get; init; } = 
        new Collection<PlayerTeamEntity>();
}