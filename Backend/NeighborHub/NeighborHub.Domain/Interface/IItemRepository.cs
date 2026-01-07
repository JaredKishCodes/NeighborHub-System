using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeighborHub.Domain.Entities;

namespace NeighborHub.Domain.Interface;
public interface IItemRepository
{
    Task<IEnumerable<Item>> GetAllItems();
    Task<Item> GetItemById(int itemId);
    Task<Item> CreateItem(Item item);
    Task<Item> UpdateItem(Item item);
    Task<bool> DeleteItem(int itemId);
}
