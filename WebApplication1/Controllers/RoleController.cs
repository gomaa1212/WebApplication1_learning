using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using WebApplication1.Models.Identity;
using WebApplication1.Models.ViewModel.Identity.Role;
using WebApplication1.UnitOfWorks;
using SecurityClaim = System.Security.Claims;


namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RoleController(RoleManager<IdentityRole> roleManager, IMapper mapper, UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            var res = _mapper.Map<List<GetRolesViewModel>>(roles);
            return View(res);
        }
        [Authorize(Policy ="CreateRole")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Policy = "CreateRole")]
        [HttpPost]
        public async Task<IActionResult> Create(AddRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var isRoleExist = await _roleManager.RoleExistsAsync(model.Name);
            if (isRoleExist)
            {
                ModelState.AddModelError("Name", "Role already exists");
                return View(model);
            }
            var role = _mapper.Map<IdentityRole>(model);
            var res = await _roleManager.CreateAsync(role);
            if (res.Succeeded)
            {
                TempData["SuccessMessage"] = "Role created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in res.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<UpdateRoleViewModel>(role);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, UpdateRoleViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            role.Name = model.Name;
            var res = await _roleManager.UpdateAsync(role);
            if (res.Succeeded)
            {
                TempData["SuccessMessage"] = "Role updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in res.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<GetRolesViewModel>(role);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id, GetRolesViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            var res = await _roleManager.DeleteAsync(role);
            if (res.Succeeded)
            {
                TempData["SuccessMessage"] = "Role deleted successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        public async Task<IActionResult> Details(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
            {
                return NotFound();
            }
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            var model = _mapper.Map<List<GetUsersInRoleViewModel>>(usersInRole);
            ViewData["roleName"] = role.Name;
            return View(model);
        }
        public async Task<IActionResult> ManageUsersInRole(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                return NotFound();
            }
            var manageUsersInRoleList = new List<ManageUsersInRoleViewModel>();
            var users =await _userManager.Users.ToListAsync();
            foreach(var user in users)
            {
                var manageUserInRole =_mapper.Map<ManageUsersInRoleViewModel>(user);
                var isInRole = await _userManager.IsInRoleAsync(user, role.Name);
                
                manageUserInRole.IsSelected = isInRole;
                manageUsersInRoleList.Add(manageUserInRole);
            }
            ViewData["RoleName"] = role.Name;
            return View(manageUsersInRoleList);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUsersInRole(string Id, List<ManageUsersInRoleViewModel> models)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {

                var role = await _roleManager.FindByIdAsync(Id);
                if (role == null)
                {
                    return NotFound();
                }
                foreach (var model in models)
                {
                    var user = await _userManager.FindByIdAsync(model.UserId);
                    if (user == null)
                        continue;
                    var isInRole = await _userManager.IsInRoleAsync(user, role.Name);
                    var res = new IdentityResult();
                    if (model.IsSelected && !isInRole)
                    {
                        res = await _userManager.AddToRoleAsync(user, role.Name);
                        if (res.Succeeded)
                        {
                            TempData["SuccessMessage"] = "User added to role successfully";
                        }
                    }
                    else if (!model.IsSelected && isInRole)
                    {
                        res = await _userManager.RemoveFromRoleAsync(user, role.Name);
                        if (res.Succeeded)
                        {
                            TempData["SuccessMessage"] = "User removed from role successfully";
                        }
                    }
                }
                await transaction.CommitAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                TempData["ErrorMessage"] = "An error occurred while managing users in role: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
        public async Task<IActionResult> ManageRoleClaims(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if(role == null)
            {
                return NotFound();
            }
            var manageRoleClaimsList = new List<ManageRoleClaimsViewModel>();
            var allClaims =await _unitOfWork.Repository<Claim>().GetAllListAsync();
            var claims = allClaims.Where(x => x.IsRoleClaim == true);
            var roleClaims =await _roleManager.GetClaimsAsync(role);
            foreach(var claim in claims)
            {
                var manageRoleClaims = new ManageRoleClaimsViewModel();
                manageRoleClaims.ClaimName = claim.NameEn;
                if(roleClaims.Any(x=>x.Type==claim.NameEn))
                {
                    manageRoleClaims.IsSelected = true;
                }
                manageRoleClaimsList.Add(manageRoleClaims);
            }
            ViewData["UserName"] = role.Name;
            return View(manageRoleClaimsList);
        }
        [HttpPost]
        public async Task<IActionResult> ManageRoleClaims(string Id , List<ManageRoleClaimsViewModel>models)
        {
            var role =await _roleManager.FindByIdAsync(Id);
            if(role == null)
            {
                return NotFound();
            }
            var claims =await _roleManager.GetClaimsAsync(role);
            foreach(var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role,claim);
            }
            foreach(var model in models)
            {
                var res = new IdentityResult();
                var isInClaim = claims.Any(x => x.Type == model.ClaimName);
                var claim = new SecurityClaim.Claim(model.ClaimName, "True");
                if (model.IsSelected && !isInClaim)
                {
                    res = await _roleManager.AddClaimAsync(role, claim);
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
            return RedirectToAction(nameof(Index));
        }



    }
}
