using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeighborHub.Domain.Entities;

public class Resource
{
   
    public int Id { get; set; }

    // Basic Info
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; 
    
    // Status Logic
    public bool IsAvailable { get; set; } = true;
    public string? ImageUrl { get; set; } // URL to the photo stored in the cloud/folder

  
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relationships (Foreign Keys)
    // The neighbor who owns the tool
    public string OwnerId { get; set; } = string.Empty;
    
    // Navigation property (Optional, used by Entity Framework)
    // public virtual ApplicationUser Owner { get; set; } 
}
