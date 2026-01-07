using System.Collections.Generic;
using System.Threading.Tasks;
using NeighborHub.Application.DTOs.Item;


namespace NeighborHub.Application.Interfaces;

public interface IItemService
{
    Task<IEnumerable<ItemResponse>> GetAllItems();
    Task<ItemResponse> GetItemById(int itemId);
    Task<ItemResponse> CreateItem(ItemRequest itemRequest);
    Task<ItemResponse> UpdateItem(int itemId, UpdateItemRequest updateItemRequest);
    Task<bool> DeleteItem(int itemId);
}
