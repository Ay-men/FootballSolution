namespace Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class TeamConfiguration : IEntityTypeConfiguration<TeamEntity>
    {
        public void Configure(EntityTypeBuilder<TeamEntity> builder)
        {
            builder.ToTable("Teams");
        
            builder.HasKey(t => t.Id);
        
            builder.Property(t => t.Name)
                .HasMaxLength(100)
                .IsRequired();
            
            builder.Property(t => t.Budget)
                .HasPrecision(18, 2);

            // Added indexes for common queries
            builder.HasIndex(t => t.Name).IsUnique();
            builder.HasIndex(t => new { t.Country, t.City });
        }
}