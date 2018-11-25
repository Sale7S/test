using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using COCAS.Models;

namespace COCAS.Controllers
{
    public class UsersController : Controller
    {
        private readonly COCASContext _context;

        public UsersController(COCASContext context)
        {
            _context = context;
        }
        
        // GET: Users
        public async Task<IActionResult> Index()
        {
            var cOCASContext = _context.User.Include(u => u.UserType);
            return View(await cOCASContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.UserType)
                .FirstOrDefaultAsync(m => m.Username == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["Type"] = new SelectList(_context.UserType, "Type", "Type");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,Password,Type")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Type"] = new SelectList(_context.UserType, "Type", "Type", user.Type);
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["Type"] = new SelectList(_context.UserType, "Type", "Type", user.Type);
            return View(user);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Username,Password,Type")] User user)
        {
            if (id != user.Username)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Username))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Type"] = new SelectList(_context.UserType, "Type", "Type", user.Type);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.UserType)
                .FirstOrDefaultAsync(m => m.Username == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.Username == id);
        }

        public IActionResult Login_Student_Ar()
        {
            ViewData["Type"] = new SelectList(_context.UserType, "Type", "Type");
            ViewData["E_Type"] = "Wrong type";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login_Student_Ar([Bind("Username,Password,Type")] User user)
        {  
            if (ModelState.IsValid)
            {
                if (UserExists(user.Username))
                {
                    User db_user = await _context.User.FindAsync(user.Username);
                    if (user.Password == db_user.Password)
                        ViewData["E_Password"] = "Wrong password";
                    if (user.Type == db_user.Type)
                        ViewData["E_Type"] = "Wrong type";
                }
                ViewData["E_Username"] = "Wrong username";
                return RedirectToAction(nameof(Index));
            }
            ViewData["Type"] = new SelectList(_context.UserType, "Type", "Type", user.Type);
            return View(user);
        }
    }
}
