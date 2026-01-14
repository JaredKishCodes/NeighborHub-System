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
public class DashboardRepository(AppDbContext _context) : IDashboardRepository
{
    public async Task<IEnumerable<Booking>> GetBookingsForCurrentMonthAsync(int? userId = null)
    {
        DateTime today = DateTime.Now;

        IQueryable<Booking> query =  _context.Bookings
                                     .Include(i => i.Item)
                                     .Include(b => b.Borrower).AsQueryable();

      query = query.Where(b => b.StartDate.Month == today.Month && b.StartDate.Year == today.Year);

        if (userId.HasValue)
        {

            query = query.Where(b => b.BorrowerId == userId);
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetLendingsForCurrentMonthAsync(int? userId = null)
    {
        DateTime today = DateTime.UtcNow; // Better for server consistency

       
        IQueryable<Booking> query = _context.Bookings
            .Include(i => i.Item)
            .Include(i => i.Borrower)
            .AsQueryable();

        
        query = query.Where(b => b.BookingStatus == Domain.Enums.BookingStatus.Confirmed
                     && b.Item.ItemStatus == Domain.Enums.ItemStatus.borrowed
                     && b.StartDate.Month == today.Month
                     && b.StartDate.Year == today.Year);

        
        if (userId.HasValue)
        {
            query = query.Where(b => b.BorrowerId == userId.Value);
        }

        return await query.ToListAsync();
    }

    public async Task<int> GetTotalBookingsCountAsync(int? userId = null)
    {
        DateTime now = DateTime.UtcNow;
        IQueryable<Booking> query = _context.Bookings.AsQueryable();

        query = query.Where(b => b.StartDate.Month == now.Month
                                && b.StartDate.Year == now.Year);
        
        if (userId.HasValue)
        {
            query = query.Where(b => b.BorrowerId == userId.Value);
        }

        return await query.CountAsync();
    }


    public async Task<int> GetTotalLendingsCountAsync(int? userId = null)
    {
        IQueryable<Booking> query = _context.Bookings.AsQueryable();

        query = query.Where(b => b.BookingStatus == Domain.Enums.BookingStatus.Confirmed
                            && b.Item.ItemStatus == Domain.Enums.ItemStatus.borrowed);  

        if (userId.HasValue)
        {
            query = query.Where(b => b.BorrowerId == userId.Value);
        }

        return await query.CountAsync();
    }

    public async Task<int> GetTotalItemsCountAsync()
    {
        return await _context.Items.CountAsync();
    }

    public async Task<int> GetTotalUsersCountAsync()
    {
      return await _context.Users.CountAsync();
    }
}
