using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            List<Student> students = new List<Student>()
        {
            new Student() { Id = 1, Name = "abdo" },
            new Student() { Id = 2, Name = "Ahmed" }
        };
            ViewBag.Title = "Home";
            return View(students);
        }
        public IActionResult Details(int number)
        {
            return View();
        }
    }
}
