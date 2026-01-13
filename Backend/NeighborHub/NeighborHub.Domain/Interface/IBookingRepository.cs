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
    Task<Booking> CreateBookingAsync(Booking booking);
    Task<Booking> UpdateBookingAsync( Booking booking);
    Task<bool> DeleteBookingAsync(int bookingId);

    Task<bool> HasOverlapAsync(int itemId, DateTime start, DateTime end);
}
