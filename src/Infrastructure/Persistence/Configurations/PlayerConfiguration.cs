namespace Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class PlayerConfiguration: IEntityTypeConfiguration<PlayerEntity>
{
    public void Configure(EntityTypeBuilder<PlayerEntity> builder)
    {
        builder.ToTable("Players");
        
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.FirstName)
            .HasMaxLength(100)
            .IsRequired();
            
        builder.Property(p => p.LastName)
            .HasMaxLength(100)
            .IsRequired();
            
        builder.Property(p => p.MarketValue)
            .HasPrecision(18, 2);
            
        builder.Property(p => p.Height)
            .HasPrecision(3, 2);

        // Added indexes for common queries
        builder.HasIndex(p => new { p.FirstName, p.LastName });
        builder.HasIndex(p => p.Nationality);
        builder.HasIndex(p => p.Position);
    }
}