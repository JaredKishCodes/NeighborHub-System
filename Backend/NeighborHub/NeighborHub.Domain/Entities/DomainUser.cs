using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeighborHub.Domain.Entities;

public class DomainUser
{
    public int? Id { get; set; } 
    public string? IdentityId { get; set; } 
    public string? FullName { get; set; }

    // Navigation for NeighborHub logic
    public ICollection<Item>? OwnedItems { get; set; } = new List<Item>();
    public ICollection<Booking>? BorrowedBookings { get; set; } = new List<Booking>();

    public static implicit operator DomainUser(DomainUser v) => throw new NotImplementedException();
}
