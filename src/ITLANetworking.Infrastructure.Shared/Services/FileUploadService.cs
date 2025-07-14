using ITLANetworking.Core.Application.Interfaces.Services.Shared;
using Microsoft.AspNetCore.Http;

namespace ITLANetworking.Infrastructure.Shared.Services
{
    public class FileUploadService : IFileUploadService
    {
        public async Task<string> UploadAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            var uploadsFolder = Path.Combine("wwwroot", "uploads", folder);
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return $"/uploads/{folder}/{uniqueFileName}";
        }

        public bool DeleteFile(string filePath)
        {
            try
            {
                var fullPath = Path.Combine("wwwroot", filePath.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task DeleteAsync(string fileName, string folder)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            var filePath = Path.Combine("wwwroot", "uploads", folder, fileName);

            await Task.Run(() =>
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                else
                {
                    throw new FileNotFoundException($"File {fileName} not found in folder {folder}.");
                }
            });
        }

        public Task<string> UpdateAsync(IFormFile file, string fileName, string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File cannot be null or empty.", nameof(file));

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));

            if (string.IsNullOrEmpty(folder))
                throw new ArgumentException("Folder cannot be null or empty.", nameof(folder));

            var uploadsFolder = Path.Combine("wwwroot", "uploads", folder);
            Directory.CreateDirectory(uploadsFolder);
            var filePath = Path.Combine(uploadsFolder, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var newFilePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(newFilePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return Task.FromResult($"/uploads/{folder}/{uniqueFileName}");
        }
    }
}
