using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeighborHub.Application.DTOs.Booking;
public class BookingResponseDto
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public int BorrowerId { get; set; }
    public string BookingStatus { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
