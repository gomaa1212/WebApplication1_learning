using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using WebApplication1.Models.Identity;
using WebApplication1.Models.Services.Interfaces;
using WebApplication1.Models.ViewModel.Identity;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManger;
        private readonly SignInManager<User> _signInManager;

        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        public AccountController(UserManager<User> userManger, SignInManager<User> signInManager, IMapper mapper, IEmailService emailService)
        {
            _userManger = userManger;
            _signInManager = signInManager;
            _mapper = mapper;
            _emailService = emailService;
        }
        public IActionResult LogIn(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var validEmail = new EmailAddressAttribute().IsValid(model.EmailOrUsername);
            var user = validEmail ? await _userManger.FindByEmailAsync(model.EmailOrUsername) : await _userManger.FindByNameAsync(model.EmailOrUsername);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Email or Password is wrong.");
                return View(model);
            }
            //var validPassword = await _userManger.CheckPasswordAsync(user, model.Password);
            //if (!validPassword)
            //{
            //    ModelState.AddModelError(string.Empty, "Password is wrong.");
            //    return View(model);
            //}
            var res = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);
            if (res.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }
                return RedirectToAction("Index", "Student");
            }
           
            if (res.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Account locked, try again later.");
                return View(model);
            }
            ModelState.AddModelError(string.Empty, "Email or Password is wrong.");
            return View(model);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userByUserName = await _userManger.FindByNameAsync(model.UserName);
            if (userByUserName is not null)
            {
                ModelState.AddModelError(string.Empty, "This User Name is already Exist");
                return View(model);
            }
            //var userByEamil = await _userManger.FindByEmailAsync(model.Email);
            //if (userByEamil is not null)
            //{
            //    ModelState.AddModelError(string.Empty, "This Email is already Exist");
            //    return View(model);
            //}
            var newUser = _mapper.Map<User>(model);

            var res = await _userManger.CreateAsync(newUser, model.Password);
             await _userManger.AddToRoleAsync(newUser, "User");
            if (res.Succeeded)
            {
                var token = await _userManger.GenerateEmailConfirmationTokenAsync(newUser);
                var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = newUser.Id, token = token }, Request.Scheme);
                await _emailService.SendEmailAsync(newUser.Email, "Confirm your email", $"Dear {newUser.UserName}, Please confirm your account by clicking this link: <a href='{confirmationLink}'>link</a>");
                return RedirectToAction("RegisterConfirmation");
            }
            foreach (var error in res.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest();
            }
            var user = await _userManger.FindByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }
            var res = await _userManger.ConfirmEmailAsync(user, token);
            if (res.Succeeded)
            {
                return View("ConfirmEmail");
            }
            return BadRequest();
        }
        public IActionResult RegisterConfirmation()
        {
            return View();
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var validEmail = new EmailAddressAttribute().IsValid(model.EmailOrUserName);
            var user = validEmail ? await _userManger.FindByEmailAsync(model.EmailOrUserName) : await _userManger.FindByNameAsync(model.EmailOrUserName);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Email or User Name is not found.");
                return View(model);
            }
            var token = await _userManger.GeneratePasswordResetTokenAsync(user);
            var code = _emailService.GenerateCode();
            var codeViewModel = new CodeViewModel
            {
                ConfirmCode = code,
                Email = user.Email,
                Token = token
            };
            await _emailService.SendEmailAsync(user.Email, "Reset Password", $"Dear {user.UserName}, Please reset your password by using this code: {code}");
            return View("ConfirmCode", codeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmCode(CodeViewModel inputCode)
        {
            if (string.IsNullOrEmpty(inputCode.Email) || string.IsNullOrEmpty(inputCode.Token))
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(inputCode);
            }
            if (inputCode.ConfirmCode != inputCode.EntryCode)
            {
                ModelState.AddModelError(string.Empty, "Code is not valid.");
                return View(inputCode);
            }
            var model = new ResetPasswordViewModel()
            {
                Email = inputCode.Email,
                Token = inputCode.Token
            };
            return View("ResetPassword", model);

        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Token))
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManger.FindByEmailAsync(model.Email);
            if (user is null)
            {
                return NotFound();
            }
            var res = await _userManger.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (res.Succeeded)
            {
                TempData["SuccessMessage"] = "Password reset successfully. You can now log in with your new password.";
                return RedirectToAction("LogIn");
            }
            foreach (var error in res.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
        public IActionResult AccessDenied(string? ReturnUrl)
        {
            return View();
        }
    }
}
