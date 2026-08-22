namespace WebApplication1.Models.Services.Interfaces
{
    public interface IFileService
    {
        public Task<string> UploadFile(IFormFile file ,string lcation);
        public Task<bool> DeleteFile(string filePath);
    }
}
