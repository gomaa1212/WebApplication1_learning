namespace WebApplication1.Models.Services.Interfaces
{
    public interface IStudentImagesService
    {
        public Task<string> AddStudentImage(List<StudentImages> studentImages);
        public Task<string> UpdateStudentImage(StudentImages studentImage);
    }
}
