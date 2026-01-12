using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeighborHub.Domain.Enums;

namespace NeighborHub.Application.DTOs.Booking;
public class UpdateBookingDto
{
    public int Id { get; set; }
    public BookingStatus BookingStatus { get; set; }
    public DateTime EndDate { get; set; }
}
