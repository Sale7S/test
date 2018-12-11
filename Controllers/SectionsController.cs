using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COCAS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COCAS.Controllers
{
    public class SectionsController : Controller
    {
        private readonly COCASContext _context;

        public SectionsController(COCASContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var cOCASContext = _context.Section.Include(se => se.Course).Include(se => se.Instructor);
            return View(await cOCASContext.ToListAsync());
        }
    }
}