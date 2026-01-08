using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeighborHub.Domain.Enums;

namespace NeighborHub.Application.DTOs.Item;
public class UpdateItemRequest
{
    // Basic Info
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;

    // Status Logic
    public ItemStatus ItemStatus { get; set; }
    public string? ImageUrl { get; set; }


    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

    public string OwnerId { get; set; } = string.Empty;

    // Navigation property (Optional, used by Entity Framework)
    // public virtual ApplicationUser Owner { get; set; } 
}
