using Microsoft.AspNetCore.Razor.TagHelpers;
using WebApplication1.Data;
using WebApplication1.Models.Services.Interfaces;
using WebApplication1.Repository.Interfaces;
using WebApplication1.UnitOfWorks;

namespace WebApplication1.Models.Services.Implementations
{
    public class StudentImagesServicem : IStudentImagesService
    {
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        public StudentImagesServicem(IFileService fileService , IUnitOfWork unitOfWork)
        {
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }
        public async Task<string> AddStudentImage(List<StudentImages> studentImages)
        {
            await _unitOfWork.Repository<StudentImages>().AddRangeAsync(studentImages);
            return await Task.FromResult("Image added successfully");
        }
       
    }
}
