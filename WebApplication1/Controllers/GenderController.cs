using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebApplication1.Models;
using WebApplication1.Models.Services.Interfaces;
using WebApplication1.Models.ViewModel.Genderss;
using static Azure.Core.HttpHeader;

namespace WebApplication1.Controllers
{
    public class GenderController : Controller
    {
        public readonly IGenderService _genderService;
        public readonly IMapper _mapper;
        public GenderController(IGenderService genderService, IMapper mapper)
        {
            _genderService = genderService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var genders =await _genderService.GetAllGenders();
            var res = _mapper.Map<List<GetAllGenderViewModel>>(genders);
            return View(res);
        }
        public async Task<IActionResult> Details(int id)
        {
            var gender =await _genderService.GetGenderByIdWithIncludeStudent(id);
            var res = _mapper.Map<GetAllGenderViewModel>(gender);
            return View(res);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddGenderViewModel gender)
        {
            if(!ModelState.IsValid)
            {
                return View(gender);
            }
            var newGender = _mapper.Map<Gender>(gender);
            await _genderService.AddGender(newGender);
            TempData["SuccessMessage"] = "Gender created successfully!";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> IsNameGenderArExist(string NameAr)
        {
            var res = await _genderService.IsGenderNameArExist(NameAr);
            return Json(!res);
        }
        public async Task<IActionResult> IsNameGenderEnExist(string NameEn)
        {
            var res = await _genderService.IsGenderNameEnExist(NameEn);
            return Json(!res);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var gender = await _genderService.GetGenderById(id);
            if(gender is null)
            {
                return NotFound();
            }
            var mappedGender = _mapper.Map<UpdateGenderViewModel>(gender);
            return View(mappedGender);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id,UpdateGenderViewModel gender)
        {
            if(!ModelState.IsValid)
            {
                return View(gender);
            }
            var editedGender = _mapper.Map<Gender>(gender);
            var msg = await _genderService.UpdateGender(editedGender);
            if(msg== "Gender not found.")
            {
                return NotFound();
            }
            TempData["EditMessage"] = "Gender updated successfully!";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var gender =await _genderService.GetGenderById(id);
            var res = _mapper.Map<GetAllGenderViewModel>(gender);
            return View(res);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id,GetAllGenderViewModel model)
        {
            await _genderService.DeleteGender(model.id);
            TempData["DeleteMessage"] = "Student deleted successfully!";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> IsNameGenderEnExistForUpdate(string NameEn,int Id)
        {
            var res =await _genderService.IsGenderNameEnExistForUpdate(NameEn,Id);
            return Json(!res);
        }
        public async Task<IActionResult> IsNameGenderArExistForUpdate(string NameAr, int Id)
        {
            var res = await _genderService.IsGenderNameEnExistForUpdate(NameAr, Id);
            return Json(!res);
        }

    }
}
