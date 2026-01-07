using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeighborHub.Application.DTOs.Resource;
using NeighborHub.Domain.Entities;

namespace NeighborHub.Application.Interfaces;
 public interface IResourceService
{
    Task<IEnumerable<ResourceResponse>> GetAllResources();
    Task<ResourceResponse> GetResourceById(int resourceId);
    Task<ResourceResponse> CreateResource(ResourceRequest resourceRequest);
    Task<ResourceResponse> UpdateResource(int id, UpdateResourceRequest updateResourceRequest);
    Task<bool> DeleteResource(int resourceId);
}
