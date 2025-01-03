namespace Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class PlayerTeamConfiguration: IEntityTypeConfiguration<PlayerTeamEntity>
{
    public void Configure(EntityTypeBuilder<PlayerTeamEntity> builder)
    {
        builder.ToTable("PlayerTeamAssociations");
        
        builder.HasKey(pt => pt.Id);
        
        builder.Property(pt => pt.Salary)
            .HasPrecision(18, 2);
            
        builder.Property(pt => pt.TransferFee)
            .HasPrecision(18, 2);

        // Relationships
        builder.HasOne(pt => pt.PlayerEntity)
            .WithMany()
            .HasForeignKey(pt => pt.PlayerId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(pt => pt.TeamEntity)
            .WithMany()
            .HasForeignKey(pt => pt.TeamId)
            .OnDelete(DeleteBehavior.Restrict);

        // Added indexes for common queries
        builder.HasIndex(pt => new { pt.PlayerId, pt.StartDate });
        builder.HasIndex(pt => new { pt.TeamId, pt.StartDate });
        builder.HasIndex(pt => pt.EndDate);
    }
    
}