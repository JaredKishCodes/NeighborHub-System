using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NeighborHub.Domain.Entities;

namespace NeighborHub.Infrastructure.Persistence.Configuration;
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable(nameof(Booking));

        // 1. Primary Key
        builder.HasKey(b => b.Id);
        
        builder.HasOne(b => b.Borrower)
            .WithMany(u => u.BorrowedBookings)
            .HasForeignKey(b => b.BorrowerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
