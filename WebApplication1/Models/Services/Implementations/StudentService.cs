
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models.Services.Interfaces;

namespace WebApplication1.Models.Services.Implementations
{
    public class StudentService : IStudentService
    {
        #region fields
        private List<Student> students;
        private readonly IFileService _fileService;
        private readonly AppDbContext _db;
        #endregion

        #region constructor
        public StudentService(IFileService fileService, AppDbContext db)
        {
            _db = db;
            _fileService = fileService;
        }
        #endregion

        #region functions
        public async Task<int> AddStudent(Student student)
        {
            try
            {
                await _db.Students.AddAsync(student);
                await _db.SaveChangesAsync();

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
                var res =await GetStudentById(id);

                if(res == null)
                    throw new Exception("Student not found");

                _fileService.DeleteFile(res.FileUrl);
                foreach (var image in res.StudentImages)
                {
                    _fileService.DeleteFile(image.FileUrl);
                }

                    _db.Students.Remove(res); 
                
                await _db.SaveChangesAsync();

                return "Student deleted successfully";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            
        }

        public async Task<Student?> GetStudentById(int id)
        {
            return await _db.Students.Include(x=>x.Gender).Include(x=>x.StudentImages).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Student>> GetStudents()
        {
            return await _db.Students.Include(x=>x.Gender).Include(x=>x.StudentImages).ToListAsync();
        }

        public async Task<string> UpdateStudent(Student student)
        {
            try
            {
                var res = await GetStudentById(student.Id);
                if (res != null)
                {
                    res.Name = student.Name;
                    res.FileUrl = student.FileUrl;
                    res.GenderId = student.GenderId;
                    _db.Students.Update(res);
                    await _db.SaveChangesAsync();
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
        public async Task<bool> IsNameExist(string name)
        {
            return await _db.Students.AnyAsync(s => s.Name == name);
        }

        public int numberOfStudents
        {
            get { return _db.Students.Count(); }
        }
        public async Task<List<Student>> GetStudentsByGender(int genderId)
        {
            try
            {
                var students = await _db.Students.Include(x=>x.Gender).Include(x=>x.StudentImages).Where(s => s.GenderId == genderId).ToListAsync();
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
            return _db.Students.Count();
        }
        #endregion

    }
}
