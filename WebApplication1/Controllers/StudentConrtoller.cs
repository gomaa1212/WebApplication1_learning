using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IGenderService _genderService;
        private readonly IMapper _mapper;
        public StudentController(IStudentService studentService, IGenderService genderService, IMapper mapper)
        {
            _studentService = studentService;
            _genderService = genderService;
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
            if(res == null || res.Count == 0)
            {
                return NotFound();
            }
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
        [Authorize(Policy = "CreateStudent")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Title = "Create Student";
            var Gender = new Gender();
            ViewBag.Genders = new SelectList(await _genderService.GetAllGenders(), "Id", Gender.localize("NameEn", "NameAr"));
            return View();
        }
        [Authorize(Policy = "CreateStudent")]
        [HttpPost]
        public async Task<IActionResult> Create(AddStudentViewModel model)
        {
            var Gender = new Gender();
            ViewBag.Genders = new SelectList(await _genderService.GetAllGenders(), "Id", Gender.localize("NameEn", "NameAr"));
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields.";
                return View(model);

            }
            var student = _mapper.Map<Student>(model);
            if (student.File is null)
            {
                TempData["ErrorMessage"] = "Please upload a valid image file.";
                return View(model);
            }
            var res=await _studentService.AddStudent(student);
            if(res != "Succeeded")
            {
                ModelState.AddModelError(string.Empty, res);
                return View(model);
            }
            
            TempData["SuccessMessage"] = "Student created successfully!";

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _studentService.GetStudentById(id);
            var res = _mapper.Map<GetAllStudentViewModel>(student);
            return View(res);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(Student student)
        {
            var res =await _studentService.DeleteStudent(student.Id);
            if(res != "Succeeded")
            {
                ModelState.AddModelError(string.Empty, res);
                return RedirectToAction("Delete", new { id = student.Id });
            }
            TempData["DeleteMessage"] = "Student deleted successfully!";
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateStudentViewModel student)
        {
            var Gender = new Gender();
            ViewBag.Genders = new SelectList(await _genderService.GetAllGenders(), "Id", Gender.localize("NameEn", "NameAr"), student.GenderId);
            if (id != student.Id)
            {
                return BadRequest("Student ID mismatch.");
            }
            if (!ModelState.IsValid)
            {
                return View(student);
            }
             
            var updatedStudent = _mapper.Map<Student>(student);
            var res = await _studentService.UpdateStudent(updatedStudent);
            if(res != "Succeeded")
            {
                ModelState.AddModelError(string.Empty, res);
                return View(student);
            }
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
            try
            {
                var students = await _studentService.GetStudentsByGender(GenderId);
                ViewBag.Count = students.Count;
                var res = _mapper.Map<List<GetAllStudentViewModel>>(students);
                return View("Index", res);

            }
            catch 
            {
                return NotFound("No Students found for the selected gender.");
            }

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
