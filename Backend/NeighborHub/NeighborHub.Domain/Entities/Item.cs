using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeighborHub.Domain.Enums;

namespace NeighborHub.Domain.Entities;

public class Item
{
   
    public int Id { get; set; }

    // Basic Info
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; 
    
    // Status Logic
    public ItemStatus ItemStatus { get; set; }
    public string? ImageUrl { get; set; } // URL to the photo stored in the cloud/folder
  
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdatedAt { get; set; }

    // Relationships (Foreign Keys)
    // The neighbor who owns the tool
    public int OwnerId { get; set; }
    
    // Navigation property (Optional, used by Entity Framework)
     public virtual DomainUser Owner { get; set; } 
}       
