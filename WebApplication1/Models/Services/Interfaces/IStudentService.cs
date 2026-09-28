namespace WebApplication1.Models.Services.Interfaces
{
    public interface IStudentService
    {
        public Task<List<Student>> GetStudents();
        public Task<Student?> GetStudentById(int id);
        public Task<int> AddStudent(Student student);
        public Task<string> UpdateStudent(Student student);
        public Task<string> DeleteStudent(int id);
        public Task<bool> IsNameEnExist(string nameEn);
        public Task<bool> IsNameArExist(string nameAr);
        public Task<bool> IsNameArExistForUpdate(string nameAr, int id);
        public Task<bool> IsNameEnExistForUpdate(string nameEn, int id);
        public int numberOfStudents { get; }
        public Task<List<Student>> GetStudentsByGender(int genderId);
        public int GetStudentCount();
        public IQueryable<Student> SearchByName(string name);
    }
}
