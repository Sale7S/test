using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace COCAS.Controllers
{
    public class PasswordChangeController : Controller
    {
        public IActionResult password_change_Ar()
        {
            return View();
        }

        public IActionResult password_change_Eng()
        {
            return View();
        }
    }
}