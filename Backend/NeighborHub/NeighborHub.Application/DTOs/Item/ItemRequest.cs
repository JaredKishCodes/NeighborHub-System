using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NeighborHub.Domain.Enums;

namespace NeighborHub.Application.DTOs.Item;
public class ItemRequest
{
   // Basic Info

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    // Status Logic
    public ItemStatus ItemStatus { get; set; }
    public IFormFile? ImageUrl { get; set; } 


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int OwnerId { get; set; }

}
