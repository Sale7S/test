using Microsoft.AspNetCore.Mvc;

namespace COCAS.Controllers
{
    public class StudentController : Controller
    {
        // 
        // GET: /Student/

        public IActionResult Index()
        {
            return View();
        }

        // 
        // GET: /Student/Welcome/ 
        public IActionResult Welcome(string name, int id = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["id"] = id;

            return View();
        }
    }
}