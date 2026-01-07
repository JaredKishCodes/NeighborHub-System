using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeighborHub.Domain.Enums;

namespace NeighborHub.Domain.Entities;
public class Bookings
{
    public int Id { get; set; }
    public int ItemId { get; set; }

    public int BorrowerId { get; set; }
    public int OwnerId { get; set; }
    public ItemStatus ItemStatus { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }


}
