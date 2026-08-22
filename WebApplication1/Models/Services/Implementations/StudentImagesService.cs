using Microsoft.AspNetCore.Razor.TagHelpers;
using WebApplication1.Data;
using WebApplication1.Models.Services.Interfaces;

namespace WebApplication1.Models.Services.Implementations
{
    public class StudentImagesServicem : IStudentImagesService
    {
        private readonly IFileService _fileService;
        private readonly AppDbContext _db;
        public StudentImagesServicem(IFileService fileService , AppDbContext db)
        {
            _fileService = fileService;
            _db = db;
        }
        public async Task<string> AddStudentImage(List<StudentImages> studentImages)
        {
            _db.StudentImages.AddRange(studentImages);
            await _db.SaveChangesAsync();
            return await Task.FromResult("Image added successfully");
        }
        public async Task<string> UpdateStudentImage(StudentImages studentImage)
        {
            var existingImage = await _db.StudentImages.FindAsync(studentImage.Id);
            if (existingImage == null)
            {
                return "Image not found";
            }
            existingImage.FileUrl = studentImage.FileUrl;
            existingImage.StudentId = studentImage.StudentId;
            await _db.SaveChangesAsync();
            return "Image updated successfully";
        }
    }
}
