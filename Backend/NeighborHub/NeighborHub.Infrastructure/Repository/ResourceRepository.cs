using Microsoft.EntityFrameworkCore;
using NeighborHub.Domain.Entities;
using NeighborHub.Domain.Interface;
using NeighborHub.Infrastructure.Persistence;

namespace NeighborHub.Infrastructure.Repository;

public class ResourceRepository(AppDbContext _context) : IResourceRepository
{
    public async Task<Resource> CreateResource(Resource resource)
    {
        _context.Resources.Add(resource); // Best practice: use the specific DbSet
        await _context.SaveChangesAsync();
        return resource;
    }

    public async Task<bool> DeleteResource(int resourceId)
    {
        Resource? res = await _context.Resources.FirstOrDefaultAsync(r => r.Id == resourceId);
        if (res != null)
        {
            _context.Resources.Remove(res);
            await _context.SaveChangesAsync();
            return true; // You MUST return a boolean here
        }
        return false; // Return false if the ID wasn't found
    }

    public async Task<IEnumerable<Resource>> GetAllResources()
    {
        // Using .AsNoTracking() makes this much faster for Read-Only calls
        return await _context.Resources.AsNoTracking().ToListAsync();
    }

    public async Task<Resource> GetResourceById(int resourceId)
    {
        // Use FindAsync or FirstOrDefaultAsync
        return await _context.Resources.FirstOrDefaultAsync(x => x.Id == resourceId);
    }

    public async Task<Resource> UpdateResource(Resource resource)
    {
        _context.Resources.Update(resource);
        await _context.SaveChangesAsync();
        return resource;
    }
}
