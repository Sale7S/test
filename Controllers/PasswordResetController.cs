using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace COCAS.Controllers
{
    public class PasswordResetController : Controller
    {
        public IActionResult password_reset_Ar()
        {
            return View();
        }

        public IActionResult password_reset_Eng()
        {
            return View();
        }
    }
}