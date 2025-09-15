using DocFlow.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DocFlow.Infrastructure.Storage;

public class LocalStorageService : IFileStorageService
{
    private readonly string _storagePath;

    public LocalStorageService(string storagePath = "wwwroot/uploads")
    {
        _storagePath = storagePath;
        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
        }
    }

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(_storagePath, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return "/uploads/" + fileName;
    }
}
