namespace WebApplication1.Models.Services.Interfaces
{
    public interface IStudentService
    {
        public Task<List<Student>> GetStudents();
        public Task<Student?> GetStudentById(int id);
        public Task<int> AddStudent(Student student);
        public Task<string> UpdateStudent(Student student);
        public Task<string> DeleteStudent(int id);
        public Task<bool> IsNameExist(string name);
        public int numberOfStudents { get; }
        public Task<List<Student>> GetStudentsByGender(int genderId);
    }
}
