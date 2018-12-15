using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COCAS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace COCAS.Controllers
{
    public class RequestsController : BaseController
    {
        private readonly COCASContext _context;

        public RequestsController(COCASContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Fill(string id, string formTitle)
        {
            if (id == null || formTitle == null)
                return NotFound();

            if (!IsAuthenticated(id))
                return RedirectToAction("Login_Ar", "Users");

            var student = await _context.Student
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.ID.Equals(id));

            var sections = _context.Section
                .Include(se => se.Course);

            var CreateRequest = new CreateRequestViewModel()
            {
                FormTitle = formTitle,
                Student = student,
                Sections = await sections.ToListAsync()
            };

            return View(CreateRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Fill([Bind("Student,SectionsNumbers")] CreateRequestViewModel CreateRequest, string id, string formTitle)
        {
            if (!IsAuthenticated(id))
                return RedirectToAction("Login_Ar", "Users");

            if (ModelState.IsValid)
            {
                var sectionsNumbers = _context.Section.Select(se => se.Number);
                var validSections = new List<string>();

                foreach (var section in CreateRequest.SectionsNumbers)
                    if (sectionsNumbers.Any(seN => seN == section))
                        validSections.Add(section);

                var IsRequested = _context.Request
                    .Any(r => r.StudentID == id && r.FormTitle == formTitle && CreateRequest.SectionsNumbers.Any(se => se == r.SectionNumber));
                
                if (!IsRequested)
                {
                    var time = new Time();
                    if (validSections.Count > 0)
                    {
                        _context.Time.Add(time);
                        await _context.SaveChangesAsync();
                    }

                    foreach (var section in validSections)
                    {
                        var request = new Request()
                        {
                            StudentID = id,
                            FormTitle = formTitle,
                            SectionNumber = section,
                            CurrentTime = time.Current
                        };
                        _context.Request.Add(request);
                        await _context.SaveChangesAsync();
                    }
                    ViewData["Request"] += $"تم إرسال طلب {formTitle}";

                    return RedirectToAction("Student", "Users", new { id });
                }
                else
                    ViewData["Requested"] += "يوجد شعبة أو أكثر مطلوبة مسبقاً.";
            }

            var student = await _context.Student
                .Include(s => s.Department)
                .FirstOrDefaultAsync(s => s.ID.Equals(id));

            var sections = _context.Section
                .Include(se => se.Course);

            CreateRequest.FormTitle = formTitle;
            CreateRequest.Student = student;
            CreateRequest.Sections = await sections.ToListAsync();

            return View(CreateRequest);
        }
    }
}