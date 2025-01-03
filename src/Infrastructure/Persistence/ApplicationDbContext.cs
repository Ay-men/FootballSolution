namespace Infrastructure.Persistence;

using Configurations;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

public class ApplicationDbContext : DbContext
{
    public DbSet<PlayerEntity> Players { get; set; }
    public DbSet<TeamEntity> Teams { get; set; }
    public DbSet<PlayerTeamEntity> PlayerTeams { get; set; }
    private readonly IDomainEventDispatcher _eventDispatcher;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IDomainEventDispatcher eventDispatcher)
        : base(options)
    {
        _eventDispatcher = eventDispatcher;
    }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PlayerConfiguration());
        modelBuilder.ApplyConfiguration(new PlayerTeamConfiguration());
        modelBuilder.ApplyConfiguration(new TeamConfiguration());

    }
    
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities();
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    private void UpdateAuditableEntities()
    {
        var entries = ChangeTracker
            .Entries<IAuditableEntity>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(nameof(IAuditableEntity.CreatedOnUtc))
                    .CurrentValue = DateTime.UtcNow;
            }

            entry.Property(nameof(IAuditableEntity.ModifiedOnUtc))
                .CurrentValue = DateTime.UtcNow;
        }
    }
}