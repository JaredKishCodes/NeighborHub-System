using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeighborHub.Domain.Entities;
using NeighborHub.Domain.Enums;

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

        builder.HasOne(x => x.Owner)
            .WithMany(x => x.OwnedItems)
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasData(
           new Item
           {
               Id = 4,
               Name = "Hammer",
               Description = "very good hammer, slightly used.",
               Category = "Tools",
               OwnerId = 2,
               ItemStatus = ItemStatus.available,
               CreatedAt = new DateTime(2026, 5, 1, 0, 0, 0, DateTimeKind.Utc),
               ImageUrl = "/item-images/hammer.jpg"
           },
           new Item
           {
               Id = 2,
               Name = "Mountain Bike",
               Description = "Adult size, 21-speed mountain bike.",
               Category = "Sports",
               OwnerId = 2,
               ItemStatus = ItemStatus.available,
               CreatedAt = new DateTime(2026, 5, 2, 0, 0, 0, DateTimeKind.Utc),
               ImageUrl = "/item-images/mountainbike.jpg"
           },
           new Item
           {
               Id = 3,
               Name = "Drill Set",
               Description = "Cordless drill with two batteries.",
               Category = "Tools",
               OwnerId = 3,
               ItemStatus = ItemStatus.available,
               CreatedAt = new DateTime(2026, 5, 3, 0, 0, 0, DateTimeKind.Utc),
               ImageUrl = "/item-images/drillset.jpg"
           }
       );
    }
}
