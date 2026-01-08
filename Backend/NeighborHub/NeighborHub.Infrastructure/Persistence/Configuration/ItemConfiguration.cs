using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NeighborHub.Domain.Entities;

namespace NeighborHub.Infrastructure.Persistence.Configuration;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        // 1. Primary Key
        builder.HasKey(r => r.Id);

        // 2. Property Configurations
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Description)
            .HasMaxLength(500);

        builder.Property(r => r.Category)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.OwnerId)
            .IsRequired();

        // 4. Set Default Values
        builder.Property(r => r.ItemStatus);
            

        builder.Property(r => r.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()"); // Uses SQL Server's clock
    }
}
