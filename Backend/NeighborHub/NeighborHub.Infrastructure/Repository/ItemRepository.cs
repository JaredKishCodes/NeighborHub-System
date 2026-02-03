using Microsoft.EntityFrameworkCore;
using NeighborHub.Domain.Entities;
using NeighborHub.Domain.Enums;
using NeighborHub.Domain.Interface;
using NeighborHub.Infrastructure.Persistence;

namespace NeighborHub.Infrastructure.Repository;

public class ItemRepository(AppDbContext _context) : IItemRepository
{
    public async Task<Item> CreateItem(Item item)
    {
        _context.Items.Add(item); 
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> DeleteItem(int itemId)
    {
        Item? item = await _context.Items.FirstOrDefaultAsync(r => r.Id == itemId);
        if (item != null)
        {
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return true; 
        }
        return false; 
    }

    public async Task<IEnumerable<Item>> GetAllItems()
    {
       
        return await _context.Items.AsNoTracking().ToListAsync();
    }

    public async Task<Item> GetItemById(int itemId)
    {
        
        return await _context.Items.FirstOrDefaultAsync(x => x.Id == itemId);
    }

    public async Task<Item> UpdateItem(Item item)
    {
        _context.Items.Update(item);
        await _context.SaveChangesAsync();
        return item;
    }

   public async Task<bool> IsItemAvailable(int itemId, DateTime start, DateTime end)
{
        bool exists = await _context.Bookings
        .AnyAsync(b =>
            b.ItemId == itemId &&
            b.BookingStatus != BookingStatus.Cancelled &&
            b.BookingStatus != BookingStatus.Completed &&
            start <= b.EndDate &&
            end >= b.StartDate
        );

    return !exists;
}



    public async Task<List<DateTime>> GetAvailableDates(int itemId)
    {
            Item? item = await _context.Items.FindAsync(itemId);

            DateTime today = DateTime.Today;
            DateTime nextMonth = today.AddDays(30);

        List<DateTime> availableDates = new();

        for (DateTime date = today; date <= nextMonth; date = date.AddDays(1))
        {
            bool isAvailable = await IsItemAvailable(itemId, date, date);

            if (isAvailable)
                {
                    availableDates.Add(date);
                }
            }

        return availableDates;
    }

}
