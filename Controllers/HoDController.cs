using Microsoft.AspNetCore.Mvc;

namespace COCAS.Controllers
{
    public class HoDController : Controller
    {
        // 
        // GET: /HoD/

        public IActionResult Index()
        {
            return View();
        }

        // 
        // GET: /HoD/Welcome/ 
        public IActionResult Welcome(string name, int id = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["id"] = id;

            return View();
        }
    }
}