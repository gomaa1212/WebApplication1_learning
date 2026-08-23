using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Models.Services.Implementations;
using WebApplication1.Models.Services.Interfaces;
using WebApplication1.Models.ViewModel;

namespace WebApplication1.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IFileService _fileService;
        private readonly IGenderService _genderService;
        private readonly IStudentImagesService _studentImagesService;
        private readonly IMapper _mapper;
        public StudentController(IStudentService studentService, IFileService fileService, IGenderService genderService, IStudentImagesService studentImagesService, IMapper mapper)
        {
            _studentService = studentService;
            _fileService = fileService;
            _genderService = genderService;
            _studentImagesService = studentImagesService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Genders = new SelectList(await _genderService.GetAllGenders(), "Id", "Name");

            if (_studentService.numberOfStudents == 0)
            {
                return RedirectToAction("Create");
            }
            var students = await _studentService.GetStudents();
            ViewBag.Title = "students";
            ViewBag.Count = students.Count;
            return View(students);
        }
        public async Task<IActionResult> Details(int id)
        {

            var student = await _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound($"No student found with id = {id}");
            }
            ViewBag.Title = $"students by id = {id}";
            return View(student);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Title = "Create Student";
            ViewBag.Genders = new SelectList(await _genderService.GetAllGenders(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddStudentViewModel model)
        {
            var studentImages = new List<StudentImages>();

            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Error in {key}: {error.ErrorMessage}");
                    }
                }
                return View(model);

            }
            var student = _mapper.Map<Student>(model);
            student.FileUrl = await _fileService.UploadFile(student.File, "images");
            var result = await _studentService.AddStudent(student);
            foreach (var f in student.Files)
            {
                var fileUrl = await _fileService.UploadFile(f, "images");
                studentImages.Add(new StudentImages { StudentId = result, FileUrl = fileUrl });
            }
            await _studentImagesService.AddStudentImage(studentImages);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _studentService.GetStudentById(id);
            return View(student);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Student student)
        {
            await _studentService.DeleteStudent(student.Id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentService.GetStudentById(id);
            ViewBag.Genders = new SelectList(await _genderService.GetAllGenders(), "Id", "Name", student?.GenderId);
            if (student == null)
            {
                return NotFound($"No student found with id = {id}");
            }
            return View(student);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            var studentImages = new List<StudentImages>();
            if (!ModelState.IsValid)
            {
                return View(student);
            }
            if (student.File != null && student.File.Length > 0)
            {
                // نمسح الصورة القديمة قبل رفع الجديدة
                await _fileService.DeleteFile(student.FileUrl);
                student.FileUrl = await _fileService.UploadFile(student.File, "images");
            }
            if(student.Files?.Count>0)
            {
               
                foreach (var f in student.Files)
                {
                    var fileUrl = await _fileService.UploadFile(f, "images");
                    studentImages.Add(new StudentImages { StudentId = student.Id, FileUrl = fileUrl });
                }
                await _studentImagesService.AddStudentImage(studentImages);
            }
            else
            {
                var existingStudent = await _studentService.GetStudentById(student.Id);
                student.FileUrl = existingStudent.FileUrl;
            }
            await _studentService.UpdateStudent(student);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> IsNameExist(string Name)
        {
            var res = await _studentService.IsNameExist(Name);
            return Json(!res);
        }
        
        public async Task<IActionResult> SearchByGender(int GenderId)
        {
            if(GenderId == 0)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Genders = new SelectList(await _genderService.GetAllGenders(), "Id", "Name",GenderId);
            var students = await _studentService.GetStudentsByGender(GenderId);
            ViewBag.Count = students.Count;
            return View("Index", students);


        }
    }
}
