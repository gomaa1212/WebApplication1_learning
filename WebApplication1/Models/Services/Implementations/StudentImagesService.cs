using Microsoft.AspNetCore.Razor.TagHelpers;
using WebApplication1.Data;
using WebApplication1.Models.Services.Interfaces;
using WebApplication1.Repository.Interfaces;

namespace WebApplication1.Models.Services.Implementations
{
    public class StudentImagesServicem : IStudentImagesService
    {
        private readonly IFileService _fileService;
        private readonly IStudentImagesRepository _studentImagesRepository;
        public StudentImagesServicem(IFileService fileService , IStudentImagesRepository studentImagesRepository)
        {
            _fileService = fileService;
            _studentImagesRepository = studentImagesRepository;
        }
        public async Task<string> AddStudentImage(List<StudentImages> studentImages)
        {
            await _studentImagesRepository.AddRangeAsync(studentImages);
            return await Task.FromResult("Image added successfully");
        }
       
    }
}
