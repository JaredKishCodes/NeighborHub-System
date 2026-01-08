using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeighborHub.Domain.Entities;

namespace NeighborHub.Domain.Interface;
public interface IBookingRepository
{
    Task<List<Booking>> GetMyBorrowingAsync(int userId);
    Task<List<Booking>> GetMyLendingAsync(int userId);
    Task<Booking> GetBookingByIdAsync(int bookingId);
    Task<Booking> CreateBooking(Booking booking);
    Task<Booking> UpdateBooking( Booking booking);
    Task<bool> DeleteBooking(int bookingId);
}
