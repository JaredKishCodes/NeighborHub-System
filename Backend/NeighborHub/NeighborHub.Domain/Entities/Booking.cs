using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeighborHub.Domain.Enums;

namespace NeighborHub.Domain.Entities;
public class Booking
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public int BorrowerId { get; set; }
    public BookingStatus BookingStatus { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public Item Item { get; set; }
    public User Borrower { get; set; }
  


}
