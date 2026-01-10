using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeighborHub.Application.DTOs.Booking;
public class CreateBookingDto
{
    public int ItemId { get; set; }
    public int BorrowerId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
