
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models.Services.Interfaces;
using WebApplication1.Repository.Interfaces;

namespace WebApplication1.Models.Services.Implementations
{
    public class StudentService : IStudentService
    {
        #region fields
        private readonly IStudentRepository _studentRepository;
        private readonly IFileService _fileService;
        #endregion

        #region constructor
        public StudentService(IFileService fileService, IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
            _fileService = fileService;
        }
        #endregion

        #region functions
        public async Task<int> AddStudent(Student student)
        {
            try
            {
                await _studentRepository.AddAsync(student);
                return student.Id;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        public async Task<string> DeleteStudent(int id)
        {
            try
            {
                var res =await _studentRepository.GetByIdAsync(id);

                if (res == null)
                    throw new Exception("Student not found");

                _fileService.DeleteFile(res.FileUrl);
                foreach (var image in res.StudentImages)
                {
                    _fileService.DeleteFile(image.FileUrl);
                }

                    await _studentRepository.DeleteAsync(res); 
                
                return "Student deleted successfully";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            
        }

        public async Task<Student?> GetStudentById(int id)
        {
            return await _studentRepository.GetAsQueryable().Include(s=>s.Gender).Include(s=>s.StudentImages).FirstOrDefaultAsync(s => s.Id == id);
        }


        public async Task<List<Student>> GetStudents()
        {
            return await _studentRepository.GetAsQueryable().Include(s=>s.StudentImages).ToListAsync();
        }

        public async Task<string> UpdateStudent(Student student)
        {
            try
            {
                var res = await GetStudentById(student.Id);
                if (res != null)
                {
                    res.NameEn = student.NameEn;
                    res.NameAr = student.NameAr;
                    res.FileUrl = student.FileUrl;
                    res.GenderId = student.GenderId;
                    await _studentRepository.UpdateAsync(res);
                    return "Student updated successfully";
                }
                else
                {
                    throw new Exception("Student not found");
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            
        }
        public async Task<bool> IsNameEnExist(string nameEn)
        {
            return await _studentRepository.GetAsQueryable().AnyAsync(s => s.NameEn == nameEn);
        }
        public async Task<bool> IsNameArExist(string nameAr)
        {
            return await _studentRepository.GetAsQueryable().AnyAsync(s => s.NameAr == nameAr);
        }
        public async Task<bool> IsNameArExistForUpdate(string nameAr, int id)
        {
            return await _studentRepository.GetAsQueryable().AnyAsync(s => s.NameAr == nameAr && s.Id != id);
        }
        public async Task<bool> IsNameEnExistForUpdate(string nameEn, int id)
        {
            return await _studentRepository.GetAsQueryable().AnyAsync(s => s.NameEn == nameEn && s.Id != id);
        }
        public int numberOfStudents
        {
            get { return _studentRepository.GetAsQueryable().Count(); }
        }
        public async Task<List<Student>> GetStudentsByGender(int genderId)
        {
            try
            {
                var students = await _studentRepository.GetAsQueryable().Include(x=>x.Gender).Include(x=>x.StudentImages).Where(s => s.GenderId == genderId).ToListAsync();
                if (students == null || students.Count == 0)
                {
                    throw new Exception("No students found for");
                }
                return students;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int GetStudentCount()
        {
            return _studentRepository.GetAsQueryable().Count();
        }
        public IQueryable<Student> SearchByName(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                return _studentRepository.GetAsQueryable().Include(s => s.StudentImages).Include(x=>x.Gender);
            }
            return _studentRepository.GetAsQueryable().Include(s => s.StudentImages).Include(x=>x.Gender).Where(s => s.NameEn.Contains(name) || s.NameAr.Contains(name));
        }
        #endregion

    }
}
