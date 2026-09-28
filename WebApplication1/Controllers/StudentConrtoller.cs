using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Models.Services.Implementations;
using WebApplication1.Models.Services.Interfaces;
using WebApplication1.Models.ViewModel.Students;

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

        public async Task<IActionResult> Index(string name)
        {
            var Gender = new Gender();
            ViewBag.Genders = new SelectList(await _genderService.GetAllGenders(), "Id", Gender.localize("NameEn", "NameAr"));

            if (_studentService.numberOfStudents == 0)
            {
                return RedirectToAction("Create");
            }

            var students =  _studentService.SearchByName(name);
            var res = await students.ToListAsync();
            var GetStudents = _mapper.Map<List<GetAllStudentViewModel>>(res);
            ViewBag.Title = "students";
            ViewBag.Count = res.Count;
            ViewBag.Admin = Request.Cookies["userName"];
            return View(GetStudents);
        }
        public async Task<IActionResult> Details(int id)
        {

            var student = await _studentService.GetStudentById(id);
            if (student == null)
            {
                return NotFound($"No student found with id = {id}");
            }
            ViewBag.Title = $"students by id = {id}";
            var res = _mapper.Map<GetAllStudentViewModel>(student);
            return View(res);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Title = "Create Student";
            var Gender = new Gender();
            ViewBag.Genders = new SelectList(await _genderService.GetAllGenders(), "Id", Gender.localize("NameEn", "NameAr"));
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddStudentViewModel model)
        {
            var studentImages = new List<StudentImages>();

            if (!ModelState.IsValid)
            {
                var Gender = new Gender();
                ViewBag.Genders = new SelectList(await _genderService.GetAllGenders(), "Id", Gender.localize("NameEn", "NameAr"));
                TempData["ErrorMessage"] = "Please fill in all required fields.";
                return View(model);

            }
            var student = _mapper.Map<Student>(model);
            student.FileUrl = await _fileService.UploadFile(student.File, "images");
            if (student.FileUrl == "no file choosen")
            {
                TempData["ErrorMessage"] = "Please upload a valid image file.";
                ViewBag.Genders = new
                    SelectList(await _genderService.GetAllGenders(), "Id", "Name");
                return View(model);
            }
                var result = await _studentService.AddStudent(student);
            foreach (var f in student.Files)
            {
                var fileUrl = await _fileService.UploadFile(f, "images");
                studentImages.Add(new StudentImages { StudentId = result, FileUrl = fileUrl });
            }
            await _studentImagesService.AddStudentImage(studentImages);
            TempData["SuccessMessage"] = "Student created successfully!";

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _studentService.GetStudentById(id);
            var res = _mapper.Map<GetAllStudentViewModel>(student);
            return View(res);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Student student)
        {
            await _studentService.DeleteStudent(student.Id);
            TempData["DeleteMessage"] = "Student deleted successfully!";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studentService.GetStudentById(id);
            var Gender = new Gender();
            ViewBag.Genders = new SelectList(await _genderService.GetAllGenders(), "Id", Gender.localize("NameEn", "NameAr"), student.GenderId);
            if (student == null)
            {
                return NotFound($"No student found with id = {id}");
            }
            var res = _mapper.Map<UpdateStudentViewModel>(student);
            return View(res);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateStudentViewModel student)
        {
            if(id != student.Id)
            {
                return BadRequest("Student ID mismatch.");
            }
            var studentImages = new List<StudentImages>();
            if (!ModelState.IsValid)
            {
                foreach(var item in ModelState)
                 {
                    foreach (var error in item.Value.Errors)
                    {
                        Console.WriteLine($"Field: {item.Key}");
                        Console.WriteLine($"Error: {error.ErrorMessage}");
                    }
                }
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
           
            var updatedStudent = _mapper.Map<Student>(student);
            await _studentService.UpdateStudent(updatedStudent);
            TempData["EditMessage"] = "Student updated successfully!";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> IsNameEnExist(string NameEn)
        {
            var res = await _studentService.IsNameEnExist(NameEn);
            return Json(!res);
        }
        [HttpPost]
        public async Task<IActionResult> IsNameArExist(string NameAr)
        {
            var res = await _studentService.IsNameArExist(NameAr);
            return Json(!res);
        }
        [HttpPost]
        public async Task<IActionResult> IsNameEnExistForUpdate(string NameEn, int Id)
        {
            var res = await _studentService.IsNameEnExistForUpdate(NameEn, Id);
            return Json(!res);
        }
        [HttpPost]
        public async Task<IActionResult> IsNameArExistForUpdate(string NameAr, int Id)
        {
            var res = await _studentService.IsNameArExistForUpdate(NameAr, Id);
            return Json(!res);
        }

        public async Task<IActionResult> SearchByGender(int GenderId)
        {
            ViewBag.Admin = Request.Cookies["userName"];
            if (GenderId == 0)
            {
                return RedirectToAction("Index");
            }
            var Gender = new Gender();
            ViewBag.Genders = new SelectList(await _genderService.GetAllGenders(), "Id", Gender.localize("NameEn", "NameAr"),GenderId);
            var students = await _studentService.GetStudentsByGender(GenderId);
            ViewBag.Count = students.Count;
            var res = _mapper.Map<List<GetAllStudentViewModel>>(students);
            return View("Index", res);
        }
        public async Task<IActionResult> SearchByName(string name)
        {
            var students = _studentService.SearchByName(name);
            var res = await students.ToListAsync();
            var GetStudents = _mapper.Map<List<GetAllStudentViewModel>>(res);
            return PartialView("_StudentListPartial", GetStudents);
        }
        public async Task<IActionResult> SearchByNameWithJson(string name)
        {
            var students = _studentService.SearchByName(name);
            var res = await students.ToListAsync();
            var GetStudents = _mapper.Map<List<GetAllStudentViewModel>>(res);
            return Json(GetStudents);
        }
    }
}
