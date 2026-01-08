using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeighborHub.Domain.Entities;

public class User
{
    public int Id { get; set; } 
    public string IdentityId { get; set; } 
    public string FullName { get; set; }

    // Navigation for NeighborHub logic
    public ICollection<Booking> BorrowedBookings { get; set; } = new List<Booking>();
}
