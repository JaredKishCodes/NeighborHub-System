using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using NeighborHub.Application.DTOs.Resource;
using NeighborHub.Application.Interfaces;
using NeighborHub.Domain.Entities;
using NeighborHub.Domain.Interface;


namespace NeighborHub.Application.Services;
public class ResourceService(IResourceRepository _resourceRepository) : IResourceService
{
    public async Task<ResourceResponse> CreateResource(ResourceRequest resourceRequest)
    {
        var resource = new Resource
        {
            Name = resourceRequest.Name,
            Description = resourceRequest.Description,
            Category = resourceRequest.Category,
            IsAvailable = resourceRequest.IsAvailable,
            ImageUrl = resourceRequest.ImageUrl,
            CreatedAt = resourceRequest.CreatedAt,
            OwnerId = resourceRequest.OwnerId,
        };

       await _resourceRepository.CreateResource(resource);

        return new ResourceResponse
        {
            Id = resource.Id,
            Name = resource.Name,
            Description = resource.Description,
            Category = resource.Category,
            IsAvailable = resource.IsAvailable,
            ImageUrl = resource.ImageUrl,
            CreatedAt = resource.CreatedAt,
            OwnerId = resource.OwnerId,
        };


    }

    public async Task<bool> DeleteResource(int resourceId)
    {
        Resource resource = await _resourceRepository.GetResourceById(resourceId);

        if (resource != null)
        {
            await _resourceRepository.DeleteResource(resourceId);
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<ResourceResponse>> GetAllResources()
    {
        IEnumerable<Resource> resources = await _resourceRepository.GetAllResources();

        if (resources == null)
        { 
        return Enumerable.Empty<ResourceResponse>();
    }

        return resources.Select(x => new ResourceResponse
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Category = x.Category,
            IsAvailable = x.IsAvailable,
            ImageUrl = x.ImageUrl,
            CreatedAt = x.CreatedAt,
            OwnerId = x.OwnerId,
        }).ToList();
    }

    public async Task<ResourceResponse> GetResourceById(int resourceId)
    {
        Resource resource = await _resourceRepository.GetResourceById(resourceId) ?? throw new Exception("No resource found");

        return new ResourceResponse
        {
            Id = resource.Id,
            Name = resource.Name,
            Description = resource.Description,
            Category = resource.Category,
            IsAvailable = resource.IsAvailable,
            ImageUrl = resource.ImageUrl,
            CreatedAt = resource.CreatedAt,
            OwnerId = resource.OwnerId,


        };
    }

    public async Task<ResourceResponse> UpdateResource(int id, UpdateResourceRequest updateResourceRequest)
    {
        Resource resource = await _resourceRepository.GetResourceById(id) ?? throw new Exception("Resource ID not found");

        resource.Name = updateResourceRequest.Name;
        resource.Description = updateResourceRequest.Description;
        resource.Category = updateResourceRequest.Category;
        resource.IsAvailable = updateResourceRequest.IsAvailable;
        resource.ImageUrl = updateResourceRequest.ImageUrl;
        resource.LastUpdatedAt = DateTime.UtcNow;


        Resource updatedResource = await _resourceRepository.UpdateResource(resource);

        return new ResourceResponse
        {
            Id = updatedResource.Id,
            Name = updatedResource.Name,
            Description = updatedResource.Description,
            Category = updatedResource.Category,
            IsAvailable = updatedResource.IsAvailable,
            ImageUrl = updatedResource.ImageUrl,
            CreatedAt = updatedResource.CreatedAt,
            LastUpdatedAt = updatedResource.LastUpdatedAt,
            OwnerId = updatedResource.OwnerId,
        };

    }
}
