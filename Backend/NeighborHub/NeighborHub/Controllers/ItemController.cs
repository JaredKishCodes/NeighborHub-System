using Microsoft.AspNetCore.Mvc;
using NeighborHub.Application.DTOs;
using NeighborHub.Application.DTOs.Item;
using NeighborHub.Application.Interfaces;
using NeighborHub.Domain.Interface;
using NeighborHub.Infrastructure.Persistence;

namespace NeighborHub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly IItemService _itemService;
    private readonly IItemRepository _itemRepository;

    public ItemController(IItemService itemService, IItemRepository itemRepository)
    {
        _itemService = itemService;
        _itemRepository = itemRepository;
    }

    // GET: api/item
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ItemResponse>>>> GetAllItemsAsync()
    {
        IEnumerable<ItemResponse> items = await _itemService.GetAllItems();

        if (!items.Any())
        {
            return NotFound(new ApiResponse<IEnumerable<ItemResponse>>
            {
                Success = false,
                Message = "No items found.",
                Data = null
            });
        }

        return Ok(new ApiResponse<IEnumerable<ItemResponse>>
        {
            Success = true,
            Message = "Items retrieved successfully.",
            Data = items
        });
    }

    // GET: api/item/{id}
    [HttpGet("{id}", Name = "GetItemById")]
    public async Task<ActionResult<ApiResponse<ItemResponse>>> GetItemByIdAsync(int id)
    {
        ItemResponse item = await _itemService.GetItemById(id);

        if (item == null)
        {
            return NotFound(new ApiResponse<ItemResponse>
            {
                Success = false,
                Message = "Item ID not found.",
                Data = null
            });
        }

        return Ok(new ApiResponse<ItemResponse>
        {
            Success = true,
            Message = "Item retrieved successfully.",
            Data = item
        });
    }

    [HttpGet("items/{itemId}/available-dates")]
public async Task<IActionResult> GetAvailableDates(int itemId)
{
    List<DateTime> result = await _itemRepository.GetAvailableDates(itemId);
    return Ok(result);
}


    // POST: api/item
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ItemResponse>>> CreateItemAsync([FromForm] ItemRequest itemRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<ItemRequest>
            {
                Success = false,
                Message = "Invalid request data.",
                Data = null
            });
        }

        ItemResponse item = await _itemService.CreateItem(itemRequest);

        return CreatedAtRoute(
            "GetItemById",
            new { id = item.Id },
            new ApiResponse<ItemResponse>
            {
                Success = true,
                Message = "Item created successfully.",
                Data = item
            });
    }

    // PUT: api/item/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ItemResponse>>> UpdateItemAsync(int id, [FromBody] UpdateItemRequest updateItemRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<UpdateItemRequest>
            {
                Success = false,
                Message = "Invalid request data.",
                Data = null
            });
        }

        ItemResponse item = await _itemService.UpdateItem(id, updateItemRequest);

        if (item == null)
        {
            return NotFound(new ApiResponse<ItemResponse>
            {
                Success = false,
                Message = "Item not found.",
                Data = null
            });
        }

        return Ok(new ApiResponse<ItemResponse>
        {
            Success = true,
            Message = "Item updated successfully.",
            Data = item
        });
    }

    // DELETE: api/item/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteItemAsync(int id)
    {
        bool isDeleted = await _itemService.DeleteItem(id);

        if (!isDeleted)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Item ID not found.",
                Data = null
            });
        }

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "Item deleted successfully.",
            Data = null
        });
    }
}
