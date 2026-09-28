
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models.Services.Interfaces;
using WebApplication1.Repository.Interfaces;
using WebApplication1.UnitOfWorks;

namespace WebApplication1.Models.Services.Implementations
{
    public class StudentService : IStudentService
    {
        #region fields
        private readonly IUnitOfWork _unitOFWork;
        private readonly IFileService _fileService;
 
        #endregion

        #region constructor
        public StudentService(IFileService fileService, IUnitOfWork unitOFWork)
        {
            _unitOFWork = unitOFWork;
            _fileService = fileService;
        }
        #endregion

        #region functions
        public async Task<string> AddStudent(Student student)
        {
            var trans = await _unitOFWork.BeginTransactionAsync();
            try
            {
                List<StudentImages> studentImages = new List<StudentImages>();
                student.FileUrl = await _fileService.UploadFile(student.File, "images");
                await _unitOFWork.Repository<Student>().AddAsync(student);

                foreach (var f in student.Files)
                {
                    var fileUrl = await _fileService.UploadFile(f, "images");
                    studentImages.Add(new StudentImages { StudentId = student.Id, FileUrl = fileUrl });
                }
                await _unitOFWork.Repository<StudentImages>().AddRangeAsync(studentImages);
                await trans.CommitAsync();
                return "Sucessful";
            }
            catch(Exception ex)
            {
                return "failed";
            }
        }

        public async Task<string> DeleteStudent(int id)
        {
            var trans = await _unitOFWork.BeginTransactionAsync();
            try
            {
                var res =await GetStudentById(id);

                if (res == null)
                    throw new Exception("Student not found");

                _fileService.DeleteFile(res.FileUrl);
                foreach (var image in res.StudentImages)
                {
                    _fileService.DeleteFile(image.FileUrl);
                }

                    await _unitOFWork.Repository<Student>().DeleteAsync(res);
                await trans.CommitAsync();
                return "Student deleted successfully";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            
        }

        public async Task<Student?> GetStudentById(int id)
        {
            return await _unitOFWork.Repository<Student>().GetAsQueryable().Include(s=>s.Gender).Include(s=>s.StudentImages).FirstOrDefaultAsync(s => s.Id == id);
        }


        public async Task<List<Student>> GetStudents()
        {
            return await _unitOFWork.Repository<Student>().GetAsQueryable().Include(s=>s.StudentImages).ToListAsync();
        }

        public async Task<string> UpdateStudent(Student student)
        {
            var trans = await _unitOFWork.BeginTransactionAsync();
            try
            {
                var res = await GetStudentById(student.Id);
                
                if (res != null)
                {
                    var studentImages = new List<StudentImages>();
                    if (student.File != null && student.File.Length > 0)
                    {
                        // نمسح الصورة القديمة قبل رفع الجديدة
                        await _fileService.DeleteFile(res.FileUrl);
                        student.FileUrl = await _fileService.UploadFile(student.File, "images");
                    }
                    if (student.Files?.Count > 0)
                    {

                        foreach (var f in student.Files)
                        {
                            var fileUrl = await _fileService.UploadFile(f, "images");
                            studentImages.Add(new StudentImages { StudentId = student.Id, FileUrl = fileUrl });
                        }
                        await _unitOFWork.Repository<StudentImages>().AddRangeAsync(studentImages);
                        
                    }

                    res.NameEn = student.NameEn;
                    res.NameAr = student.NameAr;
                    res.FileUrl = student.FileUrl;
                    res.GenderId = student.GenderId;
                    await _unitOFWork.Repository<Student>().UpdateAsync(res);
                    await trans.CommitAsync();
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
            return await _unitOFWork.Repository<Student>().GetAsQueryable().AnyAsync(s => s.NameEn == nameEn);
        }
        public async Task<bool> IsNameArExist(string nameAr)
        {
            return await _unitOFWork.Repository<Student>().GetAsQueryable().AnyAsync(s => s.NameAr == nameAr);
        }
        public async Task<bool> IsNameArExistForUpdate(string nameAr, int id)
        {
            return await _unitOFWork.Repository<Student>().GetAsQueryable().AnyAsync(s => s.NameAr == nameAr && s.Id != id);
        }
        public async Task<bool> IsNameEnExistForUpdate(string nameEn, int id)
        {
            return await _unitOFWork.Repository<Student>().GetAsQueryable().AnyAsync(s => s.NameEn == nameEn && s.Id != id);
        }
        public int numberOfStudents
        {
            get { return _unitOFWork.Repository<Student>().GetAsQueryable().Count(); }
        }
        public async Task<List<Student>> GetStudentsByGender(int genderId)
        {
            try
            {
                var students = await _unitOFWork.Repository<Student>().GetAsQueryable().Include(x=>x.Gender).Include(x=>x.StudentImages).Where(s => s.GenderId == genderId).ToListAsync();
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
            return _unitOFWork.Repository<Student>().GetAsQueryable().Count();
        }
        public IQueryable<Student> SearchByName(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                return _unitOFWork.Repository<Student>().GetAsQueryable().Include(s => s.StudentImages).Include(x=>x.Gender);
            }
            return _unitOFWork.Repository<Student>().GetAsQueryable().Include(s => s.StudentImages).Include(x=>x.Gender).Where(s => s.NameEn.Contains(name) || s.NameAr.Contains(name));
        }
        #endregion

    }
}
