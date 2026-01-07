using Microsoft.EntityFrameworkCore;
using NeighborHub.Domain.Entities;
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
}
