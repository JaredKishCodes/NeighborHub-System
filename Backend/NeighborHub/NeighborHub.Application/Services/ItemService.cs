using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeighborHub.Application.DTOs.Item;
using NeighborHub.Application.Interfaces;
using NeighborHub.Domain.Entities;
using NeighborHub.Domain.Interface;

namespace NeighborHub.Application.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;

    public ItemService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<ItemResponse> CreateItem(ItemRequest itemRequest)
    {
        var item = new Item
        {
            Name = itemRequest.Name,
            Description = itemRequest.Description,
            Category = itemRequest.Category,
            Status = itemRequest.Status,
            ImageUrl = itemRequest.ImageUrl,
            CreatedAt = itemRequest.CreatedAt,
            OwnerId = itemRequest.OwnerId,
        };

        await _itemRepository.CreateItem(item);

        return new ItemResponse
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Category = item.Category,
            Status = item.Status,
            ImageUrl = item.ImageUrl,
            CreatedAt = item.CreatedAt,
            OwnerId = item.OwnerId,
        };
    }

    public async Task<bool> DeleteItem(int itemId)
    {
        Item item = await _itemRepository.GetItemById(itemId);

        if (item != null)
        {
            await _itemRepository.DeleteItem(itemId);
            return true;
        }

        return false;
    }

    public async Task<IEnumerable<ItemResponse>> GetAllItems()
    {
        IEnumerable<Item> items = await _itemRepository.GetAllItems();

        if (items == null)
        {
            return Enumerable.Empty<ItemResponse>();
        }

        return items.Select(x => new ItemResponse
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Category = x.Category,
            Status = x.Status,
            ImageUrl = x.ImageUrl,
            CreatedAt = x.CreatedAt,
            OwnerId = x.OwnerId,
        }).ToList();
    }

    public async Task<ItemResponse> GetItemById(int itemId)
    {
        Item item = await _itemRepository.GetItemById(itemId)
            ?? throw new Exception("Item not found");

        return new ItemResponse
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Category = item.Category,
            Status = item.Status,
            ImageUrl = item.ImageUrl,
            CreatedAt = item.CreatedAt,
            OwnerId = item.OwnerId,
        };
    }

    public async Task<ItemResponse> UpdateItem(int itemId, UpdateItemRequest updateItemRequest)
    {
        Item item = await _itemRepository.GetItemById(itemId)
            ?? throw new Exception("Item ID not found");

        item.Name = updateItemRequest.Name;
        item.Description = updateItemRequest.Description;
        item.Category = updateItemRequest.Category;
        item.Status = updateItemRequest.Status;
        item.ImageUrl = updateItemRequest.ImageUrl;
        item.LastUpdatedAt = DateTime.UtcNow;

        Item updatedItem = await _itemRepository.UpdateItem(item);

        return new ItemResponse
        {
            Id = updatedItem.Id,
            Name = updatedItem.Name,
            Description = updatedItem.Description,
            Category = updatedItem.Category,
            Status = updatedItem.Status,
            ImageUrl = updatedItem.ImageUrl,
            CreatedAt = updatedItem.CreatedAt,
            LastUpdatedAt = updatedItem.LastUpdatedAt,
            OwnerId = updatedItem.OwnerId,
        };
    }
}
