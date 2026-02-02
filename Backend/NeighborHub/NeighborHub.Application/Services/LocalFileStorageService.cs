using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NeighborHub.Application.Interfaces;

namespace NeighborHub.Application.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _env;

    public LocalFileStorageService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveFileAsync(IFormFile file, string folderName)
    {
        // Use ContentRootPath instead of WebRootPath
        string uploadPath = Path.Combine(_env.ContentRootPath, "Resources", folderName);

        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        string fullPath = Path.Combine(uploadPath, fileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return a URL-friendly path
        return $"/Resources/{folderName}/{fileName}";
    }
}
