using Microsoft.AspNetCore.Hosting;
using WebApplication1.Models.Services.Interfaces;

namespace WebApplication1.Models.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment )
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadFile(IFormFile file, string location)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is null or empty", nameof(file));
            }
            
            var path = Path.Combine(_webHostEnvironment.WebRootPath, location);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            using (FileStream fileStream = File.Create(Path.Combine(path, fileName)))
            {
                await file.CopyToAsync(fileStream);
                fileStream.Flush();
            }
            return $"{location}/{fileName}"; 
                
        }
        public async Task<bool> DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path is null or empty", nameof(filePath));
                return false ;
            }
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
            return false;
        }
        
    }
}
