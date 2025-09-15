
using Microsoft.AspNetCore.Http;

namespace DocFlow.Application.Services.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(IFormFile file);
    }
}
