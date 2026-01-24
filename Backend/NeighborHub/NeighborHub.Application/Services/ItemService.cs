using NeighborHub.Application.DTOs.Item;
using NeighborHub.Application.Interfaces;
using NeighborHub.Domain.Entities;
using NeighborHub.Domain.Interface;

namespace NeighborHub.Application.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly IDomainUserRepository _domainUserRepository;

    public ItemService(IItemRepository itemRepository, IDomainUserRepository domainUserRepository)
    {
        _itemRepository = itemRepository;
        _domainUserRepository = domainUserRepository;
    }

    public async Task<ItemResponse> CreateItem(ItemRequest itemRequest)
    {
        var item = new Item
        {
            Name = itemRequest.Name,
            Description = itemRequest.Description,
            Category = itemRequest.Category,
            ItemStatus = itemRequest.ItemStatus,
            ImageUrl = itemRequest.ImageUrl,
            CreatedAt = itemRequest.CreatedAt,
            OwnerId = itemRequest.OwnerId,
        };

        await _itemRepository.CreateItem(item);

        DomainUser? owner = await _domainUserRepository.GetDomainUserById(item.OwnerId);

        return MapToResponse(item, owner?.FullName);
    }

    public async Task<IEnumerable<ItemResponse>> GetAllItems()
    {
        IEnumerable<Item> items = await _itemRepository.GetAllItems();
        if (items == null || !items.Any())
        {
            return Enumerable.Empty<ItemResponse>();
        }

        IEnumerable<int> ownerIds = items.Select(x => x.OwnerId).Distinct();
        var owners = new Dictionary<int, string>();

        foreach (int id in ownerIds)
        {
            DomainUser? user = await _domainUserRepository.GetDomainUserById(id);
            owners[id] = user?.FullName ?? "Unknown";
        }

        return items.Select((Item item) => MapToResponse(item, owners.GetValueOrDefault(item.OwnerId)));
    }

    public async Task<ItemResponse> GetItemById(int itemId)
    {
        Item item = await _itemRepository.GetItemById(itemId)
            ?? throw new Exception("Item not found");

        DomainUser? owner = await _domainUserRepository.GetDomainUserById(item.OwnerId);

        return MapToResponse(item, owner?.FullName);
    }

    public async Task<ItemResponse> UpdateItem(int itemId, UpdateItemRequest updateItemRequest)
    {
        Item item = await _itemRepository.GetItemById(itemId)
            ?? throw new Exception("Item ID not found");

        item.Name = updateItemRequest.Name;
        item.Description = updateItemRequest.Description;
        item.Category = updateItemRequest.Category;
        item.ItemStatus = updateItemRequest.ItemStatus;
        item.ImageUrl = updateItemRequest.ImageUrl;
        item.LastUpdatedAt = DateTime.UtcNow;

        Item updatedItem = await _itemRepository.UpdateItem(item);
        DomainUser? owner = await _domainUserRepository.GetDomainUserById(updatedItem.OwnerId);

        return MapToResponse(updatedItem, owner?.FullName);
    }

    public async Task<bool> DeleteItem(int itemId)
    {
        Item? item = await _itemRepository.GetItemById(itemId);
        if (item == null)
        {
            return false;
        }

        await _itemRepository.DeleteItem(itemId);
        return true;
    }

    private static ItemResponse MapToResponse(Item item, string? ownerName)
    {
        return new ItemResponse
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Category = item.Category,
            ItemStatus = item.ItemStatus,
            ImageUrl = item.ImageUrl,
            CreatedAt = item.CreatedAt,
            LastUpdatedAt = item.LastUpdatedAt,
            OwnerName = ownerName ?? "Unknown"
        };
    }
}
