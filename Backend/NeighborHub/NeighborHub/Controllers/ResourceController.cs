
using System.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NeighborHub.Application.DTOs;
using NeighborHub.Application.DTOs.Resource;
using NeighborHub.Application.Interfaces;
using NeighborHub.Domain.Entities;

namespace NeighborHub.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ResourceController : ControllerBase
{
    private readonly IResourceService _resourceService;
    public ResourceController(IResourceService resourceService)
    {
        _resourceService = resourceService;
    }

    [HttpGet("GetAllResources")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ResourceResponse>>>> GetAllResourcesAsync()
    {
        IEnumerable<ResourceResponse> resources = await _resourceService.GetAllResources();

        if (!resources.Any())
        {
          return NotFound(  new ApiResponse<IEnumerable<ResourceResponse>>()
            {
                Success = false,
                Message = "Resources not found",
                Data = null
            });
        }

        return Ok(new ApiResponse<IEnumerable<ResourceResponse>>
        {
            Success = true,
            Message = "Resources Retrieved Successfully!",
            Data = resources
        });
    }

    [HttpGet("GetResourceById/{id}",Name = "GetResourceById")]
    public async Task<ActionResult<ApiResponse<ResourceResponse>>> GetResourceByIdAsync(int id)
    {
        ResourceResponse resource = await _resourceService.GetResourceById(id);

        if (resource == null)
        {
            return NotFound(new ApiResponse<ResourceResponse>
            {
                Success = false,
                Message = "Resource ID not found",
                Data = resource
            });
        }

        return Ok(new ApiResponse<ResourceResponse>
        {
            Success = true,
            Message = "Resource Retrieved Successfully!",
            Data = resource

        });

    }

    [HttpPost("CreateResource")]
    public async Task<ActionResult<ApiResponse<ResourceResponse>>> CreateResourceAsync([FromBody] ResourceRequest resourceRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<ResourceRequest>
            {
                Success = false,
                Message = "Invalid request data.",
                Data = null
            });
        }


        ResourceResponse resource = await _resourceService.CreateResource(resourceRequest);

        if (resource == null)
        {
            return NotFound(new ApiResponse<ResourceResponse>
            {
                Success = false,
                Message = "Resource not found",
                Data = resource
            });
        }

        return CreatedAtAction( "GetResourceById", new { id = resource.Id} ,new ApiResponse<ResourceResponse>
        {
            Success = true,
            Message = "Resource created Successfully!",
            Data = resource

        });

    }

    [HttpPut("UpdateResource/{id}")]
    public async Task<ActionResult<ApiResponse<ResourceResponse>>> UpdateResourceAsync( int id, [FromBody] UpdateResourceRequest updateResourceRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<UpdateResourceRequest>
            {
                Success = false,
                Message = "Invalid request data.",
                Data = null
            });
        }


        ResourceResponse resource = await _resourceService.UpdateResource(id, updateResourceRequest);

        if (resource == null)
        {
            return NotFound(new ApiResponse<ResourceResponse>
            {
                Success = false,
                Message = "Resource not found",
                Data = resource
            });
        }

        return Ok( new ApiResponse<ResourceResponse>
        {
            Success = true,
            Message = "Resource updated Successfully!",
            Data = resource

        });

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteResourceAsync(int id)
    {
        bool isDeleted = await _resourceService.DeleteResource(id);

        if (!isDeleted)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Resource ID not found",
                Data = null
            });
        }

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "Resource deleted successfully!",
            Data = null
        });
    }




}
