﻿using Microsoft.AspNetCore.Mvc;

namespace COCAS.Controllers
{
    public class InstructorController : Controller
    {
        // 
        // GET: /Instructor/

        public IActionResult Index()
        {
            return View();
        }

        // 
        // GET: /Instructor/Welcome/ 
        public IActionResult Welcome(string name, int id = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["id"] = id;

            return View();
        }
    }
}