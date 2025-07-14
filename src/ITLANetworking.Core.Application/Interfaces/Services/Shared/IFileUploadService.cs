using Microsoft.AspNetCore.Http;

namespace ITLANetworking.Core.Application.Interfaces.Services.Shared
{
    public interface IFileUploadService
    {
        Task<string> UploadAsync(IFormFile file, string folder);
        bool DeleteFile(string filePath);
        Task DeleteAsync(string fileName, string folder);
        Task<string> UpdateAsync(IFormFile file, string fileName, string folder);
    }
}
