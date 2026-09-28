using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Text.Json;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private int UserId = 1;
        private string UserName = "Abdelrahman";
        public IActionResult Index()
        {
            //CookieOptions cookieOption = new CookieOptions();
            //cookieOption.Expires = DateTimeOffset.UtcNow.AddDays(60);
            //Response.Cookies.Append("userId", UserId.ToString(), cookieOption);
            //Response.Cookies.Append("userName", UserName, cookieOption);

            //HttpContext.Session.SetInt32("user_id", UserId);
            //HttpContext.Session.SetString("username", UserName);

            var student = new Student() { Id = 5, NameEn = "abdo" };
            HttpContext.Session.SetString("student", JsonSerializer.Serialize(student));

            var studentJson = HttpContext.Session.GetString("student");
            var st = JsonSerializer.Deserialize<Student>(studentJson);
            ViewBag.Title = "Home";
            TempData["Message"] = "This is a message from HomeController";
            
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

    }
}
