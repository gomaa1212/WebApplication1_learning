using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.PDB;
using Microsoft.EntityFrameworkCore;
using SecurityClaim = System.Security.Claims;
using WebApplication1.Models.Identity;
using WebApplication1.Models.ViewModel.Identity.User;
using WebApplication1.UnitOfWorks;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersViewModels = new List<GetUsersViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userViewModel = _mapper.Map<GetUsersViewModel>(user);
                userViewModel.RoleName = roles.ToList();
                usersViewModels.Add(userViewModel);
            }
            return View(usersViewModels);
        }
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userViewModel = _mapper.Map<UpdateUserViewModel>(user);
            return View(userViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateUserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userViewModel);
            }

            var user = await _userManager.FindByIdAsync(userViewModel.Id);
            if (user == null)
            {
                return NotFound();
            }

            _mapper.Map(userViewModel, user);
            var res =await _userManager.UpdateAsync(user);
            if(res.Succeeded == false)
            {
                foreach (var error in res.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(userViewModel);
            }
            TempData["SuccessMessage"] = "User updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            var resUser = _mapper.Map<GetUserByIdViewModel>(user);
            return View(resUser);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id,GetUserByIdViewModel model)
        {
            if(id != model.Id)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            var res = await _userManager.DeleteAsync(user);
            if(!res.Succeeded)
            {
                foreach (var error in res.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            TempData["DeleteMessage"] = "User deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ManageRolesInUser(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if(user == null)
            {
                return NotFound();
            }
            var roles = await _roleManager.Roles.ToListAsync();
            var manageRolesInUserViewModelsList = new List<ManageRolesInUserViewModel>();
            foreach(var role in roles)
            {
                var manageRolesInUserViewModel = _mapper.Map<ManageRolesInUserViewModel>(role);
                manageRolesInUserViewModel.IsSelected = await _userManager.IsInRoleAsync(user, role.Name);
                manageRolesInUserViewModelsList.Add(manageRolesInUserViewModel);
            }
            ViewData["UserName"] = user.UserName;
            return View(manageRolesInUserViewModelsList);
        }
        [HttpPost]
        public async Task<IActionResult> ManageRolesInUser(string Id, List<ManageRolesInUserViewModel> models)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {

                var user = await _userManager.FindByIdAsync(Id);
                if (user == null)
                {
                    return NotFound();
                }
                foreach (var model in models)
                {
                    var role = await _roleManager.FindByIdAsync(model.RoleId);
                    var isInRole = await _userManager.IsInRoleAsync(user, model.RoleName);
                    var res = new IdentityResult();
                    if (model.IsSelected && !isInRole)
                    {
                        res = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (res.Succeeded)
                        {
                            TempData["SuccessMessage"] = "Roles updated successfully.";
                        }

                    }
                    else if (!model.IsSelected && isInRole)
                    {
                        res = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (res.Succeeded)
                        {
                            TempData["SuccessMessage"] = "Roles updated successfully.";
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError(string.Empty, "An error occurred while updating roles. Please try again.");
                return View(models);
            }
            finally
            {
                await transaction.CommitAsync();
            }
        }
        public async Task<IActionResult> ManageUserClaims(string Id)
        {
            var user =await _userManager.FindByIdAsync(Id);
            if(user == null)
            {
                return NotFound();
            }
            var manageUserClaims = new List<ManageUserClaimsViewModel>();
            var allClaims = await _unitOfWork.Repository<Models.Identity.Claim>().GetAllListAsync();
            var Claims = allClaims.Where(x => x.IsUserClaim==true);
            var userClaims =await _userManager.GetClaimsAsync(user);
            foreach(var claim in Claims)
            {
                var manageUserClaim = new ManageUserClaimsViewModel();
                manageUserClaim.ClaimName = claim.NameEn;
                if(userClaims.Any(x=>x.Type==claim.NameEn))
                {
                    manageUserClaim.IsSelected = true;
                }
                manageUserClaims.Add(manageUserClaim);
            }
            ViewData["UserName"] = user.UserName;
            return View(manageUserClaims);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(string Id,List<ManageUserClaimsViewModel> models)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {

                var user =await _userManager.FindByIdAsync(Id);
                if(user == null)
                {
                    return NotFound();
                }
                if(!ModelState.IsValid)
                {
                    return View(models);
                }
                var userClaims =await _userManager.GetClaimsAsync(user);
                 await _userManager.RemoveClaimsAsync(user, userClaims);
                foreach(var model in models)
                {

                    var res = new IdentityResult();
                    var isInClaim = userClaims.Any(x => x.Type == model.ClaimName);
                    var claim = new SecurityClaim.Claim(model.ClaimName, "True");
                    if (model.IsSelected && !isInClaim)
                    {
                        res = await _userManager.AddClaimAsync(user, claim);
                        if (res.Succeeded)
                        {
                            TempData["SuccessMessage"] = "User Claims updated successfully.";
                        }
                    }
                    else if (!model.IsSelected && isInClaim)
                    {
                        if (res.Succeeded)
                        {
                            TempData["SuccessMessage"] = "User Claims deleted successfully.";
                        }
                    }
                }
                await _unitOfWork.CommitTransactionAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError(string.Empty, "An error occurred while updating claims. Please try again.");
                return View(models);
            }
            
            
        }
      
        
    }
}
