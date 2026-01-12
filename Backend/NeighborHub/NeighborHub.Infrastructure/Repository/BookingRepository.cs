using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NeighborHub.Domain.Entities;
using NeighborHub.Domain.Interface;
using NeighborHub.Infrastructure.Persistence;

namespace NeighborHub.Infrastructure.Repository;
public class BookingRepository(AppDbContext _context) : IBookingRepository
{
    public async Task<Booking> CreateBookingAsync(Booking booking)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task<bool> DeleteBookingAsync(int bookingId)
    {
        Booking booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
          await  _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<Booking> GetBookingByIdAsync(int bookingId)
    {
       return await _context.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId);
       
    }

    public async Task<List<Booking>> GetMyBorrowingAsync(int userId)
    {
     return await _context.Bookings.Include(x => x.Item)
            .Where(b => b.BorrowerId == userId)
            .OrderByDescending(b => b.StartDate)
            .ToListAsync();
       
    }

    public async Task<List<Booking>> GetMyLendingAsync(int userId)
    {
      return await _context.Bookings.Include(_x => _x.Item)
            .Where(x => x.Item.OwnerId == userId)
            .OrderByDescending(x => x.StartDate)
            .ToListAsync();
    }

    public async Task<Booking> UpdateBookingAsync(Booking booking)
    {
        _context.Update(booking);
        await _context.SaveChangesAsync();
        return booking;
    }
}
