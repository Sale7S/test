using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using COCAS.Models;
using Microsoft.AspNetCore.Http;

namespace COCAS.Controllers
{
    public class UsersController : BaseController
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
                return NotFound();

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
                return NotFound();

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
            if (ModelState.IsValid)
            {
                try
                {
                    var userRow = await _context.User.FirstOrDefaultAsync(u => u.Username == id);
                    _context.Remove(userRow);
                    _context.Add(user);
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
                return NotFound();

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

        public IActionResult Login_Ar()
        {
            if (IsLoggedIn())
            {
                if (IsStudent())
                    return RedirectToAction(nameof(Student), new { id = UsernameSession });
                else if (IsStaff())
                    return RedirectToAction(nameof(Staff), new { id = UsernameSession });
                else if (IsHoD())
                    return RedirectToAction(nameof(HoD), new { id = UsernameSession });
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login_Ar([Bind("Username,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                User db_user = await _context.User
                        .FirstOrDefaultAsync(m => m.Username == user.Username);
                if (db_user == null)
                    ViewData["validate_username"] = "اسم المستخدم غير صحيح";
                else if (user.Password != db_user.Password)
                    ViewData["validate_password"] = "كلمة المرور غير صحيحة";
                else
                {
                    if (db_user.IsFirstLogin == true)
                        return RedirectToAction(nameof(Password_change_Ar));
                    else
                    {
                        HttpContext.Session.SetString("Username", db_user.Username);
                        HttpContext.Session.SetString("UserType", db_user.Type);

                        UsernameSession = HttpContext.Session.GetString("Username");
                        UserTypeSession = HttpContext.Session.GetString("UserType");

                        if (IsStudent())
                            return RedirectToAction(nameof(Student), new { id = db_user.Username });
                        else if (IsStaff())
                            return RedirectToAction(nameof(Staff), new { id = db_user.Username });
                        else if (IsHoD())
                            return RedirectToAction(nameof(HoD), new { id = db_user.Username });
                    }
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            if (!IsLoggedIn())
                return RedirectToAction(nameof(Login_Ar));

            HttpContext.Session.SetString("Username", "");
            HttpContext.Session.SetString("UserType", "");

            UsernameSession = HttpContext.Session.GetString("Username");
            UserTypeSession = HttpContext.Session.GetString("UserType");

            return RedirectToAction(nameof(Login_Ar));
        }
        
        public IActionResult Password_change_Ar()
        {
            if (IsLoggedIn())
                return RedirectToAction(nameof(Login_Ar));
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Password_change_Ar([Bind("Username, NewPassword, ConfirmNewPassword")] PasswordChangeViewModel change)
        {
            if (change == null)
                return NotFound();

            if (IsLoggedIn())
                return RedirectToAction(nameof(Login_Ar));

            var user = await _context.User
                     .FirstOrDefaultAsync(u => u.Username == change.Username);

            if (user == null)
                ViewData["msg"] = "اسم مستخدم غير صحيح.";
            else if (change.NewPassword != change.ConfirmNewPassword)
                ViewData["msg"] = "كلمتا المرور غير متطابقتين.";
            else
            {
                user.Password = change.NewPassword;
                ViewData["msg"] = "تم تغيير كلمة المرور بنجاح.";
                
                user.IsFirstLogin = false;
                _context.User.Update(user);
                await _context.SaveChangesAsync();
            }

            return View();
        }

        public async Task<IActionResult> Student(string id)
        {
            if (id == null)
                return NotFound();

            if (!IsStudent())
                return RedirectToAction(nameof(Login_Ar));

            var schedule = _context.Schedule
                .Include(sc => sc.Section)
                    .ThenInclude(se => se.Course)
                .Include(sc => sc.Section)
                    .ThenInclude(se => se.Instructor)
                .Include(sc => sc.Student)
                    .ThenInclude(s => s.Department)
                .Where(sc => sc.Student.ID.Equals(id));

            var forms = _context.Form
                .Where(f => f.Type.Equals("Student"));
            ViewData["Forms"] = new SelectList(forms, "Title", "Title");

            return View(new StudentViewModel { Schedule = await schedule.ToListAsync() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Student([Bind("FormTitle")] StudentViewModel studentView, string id)
        {
            if (!IsStudent())
                return RedirectToAction("Login_Ar", "Users");

            if (ModelState.IsValid)
                return RedirectToAction("Fill", "Requests", new { id, formTitle = studentView.FormTitle });

            return View(studentView);
        }

        public IActionResult Staff(string id)
        {
            if (id == null)
                return NotFound();

            if (!IsStaff())
                return RedirectToAction(nameof(Login_Ar));
            
            var requests = _context.Request
                .Include(r => r.Student)
                .Where(r => !_context.Response.Any(res => res.RequestID == r.ID))
                .GroupBy(
                r => r.CurrentTime,
                r => r,
                (key, value) => new { time = key, Requests = value.ToList() });

            var requestsView = new List<RequestViewModel>();

            foreach (var req in requests)
            {
                var requestView = new RequestViewModel
                {
                    CurrentTime = req.time,
                    Requests = req.Requests
                };
                requestsView.Add(requestView);
            }
            if (requestsView.Count == 0)
                ViewData["noRequests"] = "لا يوجد طلبات جديدة.";
            return View(requestsView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Staff(string id, int current_time)
        {
            if (!IsStaff())
                return RedirectToAction("Login_Ar", "Users");

            return RedirectToAction("Fill", "Responses", new { id, current_time });
        }

        public async Task<IActionResult> HoD(string id)
        {
            var requests = await _context.Request
                .Include(r => r.Student)
                .ToListAsync();
            if (id == null)
                return NotFound();
            if (!IsHoD())
                return RedirectToAction(nameof(Login_Ar));

            var forms = _context.Form
                .Where(f => f.Type.Equals("Student"));

            ViewData["Forms"] = new SelectList(forms, "Title", "Title");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HoD(string a, string id)
        {
            return View();
        }
    }
}