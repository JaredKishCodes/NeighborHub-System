using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeighborHub.Domain.Entities;

namespace NeighborHub.Domain.Interface;
public interface IResourceRepository
{
    Task<IEnumerable<Resource>> GetAllResources();
    Task<Resource> GetResourceById(int resourceId);
    Task<Resource> CreateResource(Resource resource);
    Task<Resource> UpdateResource(Resource resource);
    Task<bool> DeleteResource(int resourceId);
}
