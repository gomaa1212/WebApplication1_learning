using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Helper;
using WebApplication1.Models.Identity;
using WebApplication1.Models.Services.Implementations;
using WebApplication1.Models.Services.Interfaces;
using WebApplication1.Models.ViewModel.Genderss;
using WebApplication1.Models.ViewModel.Identity.Claims;

namespace WebApplication1.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ClaimController : Controller
    {
        private readonly IClaimService _claimService;
        public readonly IMapper _mapper;
        public ClaimController(IClaimService claimService,IMapper mapper)
        {
            _claimService = claimService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var claims =await _claimService.GetAllClaimsAsync();
            var getClaimsList = _mapper.Map<List<GetClaimsListViewModel>>(claims);
            return View(getClaimsList);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Choose = new SelectList(Store.chosses, "ChooseValue", CultureHelper.IsEnglish()?"NameEn":"NameAr");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddClaimViewModel model)
        {
            ViewBag.Choose = new SelectList(Store.chosses, "ChooseValue", CultureHelper.IsEnglish() ? "NameEn" : "NameAr");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var newClaim = _mapper.Map<Claim>(model);
            var res = await _claimService.AddClaimAsync(newClaim);
            if(res== "Successful")
            {
                TempData["SuccessMessage"] = "Claim Added Successfuly";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, res);
            return View(model);
        }
        public async Task<IActionResult> Edit(int Id)
        {
            var claim = await _claimService.GetClaimByIdAsync(Id);
            ViewBag.ChooseUserClaim = new SelectList(Store.chosses, "ChooseValue", CultureHelper.IsEnglish() ? "NameEn" : "NameAr",claim.IsUserClaim);
            ViewBag.ChooseRoleClaim = new SelectList(Store.chosses, "ChooseValue", CultureHelper.IsEnglish() ? "NameEn" : "NameAr", claim.IsRoleClaim);

            if (claim == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<UpdateClaimViewModel>(claim);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int Id,UpdateClaimViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            if(Id!=model.Id)
            {
                return NotFound();
            }
            var newClaim = _mapper.Map<Claim>(model);
            var res =await _claimService.UpdateClaimAsync(newClaim);
            if(res == "Successful")
            {
                TempData["SuccessMessage"] = "Claim Edited Successfuly";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            var claim = await _claimService.GetClaimByIdAsync(Id);
           
            if (claim == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<GetClaimByIdViewModel>(claim);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int Id,GetClaimByIdViewModel model)
        {
            if (Id != model.Id)
            {
                return NotFound();
            }
            var res =await _claimService.DeleteClaimAsync(Id);
            if(res == "Successful")
            {
                TempData["SuccessMessage"] = "Claim Deleted Successfuly";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
