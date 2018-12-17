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
                else if (IsDean())
                    return RedirectToAction(nameof(Dean), new { id = UsernameSession });
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
                        return RedirectToAction(nameof(Password_change_Ar), new { user.Username });
                    else
                    {
                        HttpContext.Session.SetString("Username", db_user.Username);
                        HttpContext.Session.SetString("UserType", db_user.Type);

                        UsernameSession = HttpContext.Session.GetString("Username");
                        UserTypeSession = HttpContext.Session.GetString("UserType");

                        return RedirectToAction(nameof(Login_Ar), new { id = UsernameSession });
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
        
        public IActionResult Password_change_Ar(string username)
        {
            if (username == null)
                return NotFound();

            if (IsLoggedIn())
                return RedirectToAction(nameof(Login_Ar));

            var change = new PasswordChangeViewModel
            {
                Username = username
            };

            return View(change);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Password_change_Ar([Bind("Username, NewPassword, ConfirmNewPassword")] PasswordChangeViewModel change, string unused)
        {
            if (change == null)
                return NotFound();

            if (IsLoggedIn())
                return RedirectToAction(nameof(Login_Ar));

            if (ModelState.IsValid)
            {
                var user = await _context.User
                         .FirstOrDefaultAsync(u => u.Username == change.Username);

                if (user == null)
                    ViewData["validation"] = "اسم المستخدم غير صحيح.";
                else if (change.NewPassword != change.ConfirmNewPassword)
                    ViewData["validation"] = "كلمتا المرور غير متطابقتين.";
                else
                {
                    user.Password = change.NewPassword;
                    user.IsFirstLogin = false;
                    _context.User.Update(user);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Login_Ar));
                }
            }
            return View(change);
        }

        public async Task<IActionResult> Student(string id)
        {
            if (id == null)
                return NotFound();

            if (!IsAuthenticated(id))
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
        public IActionResult Student([Bind("FormTitle")] StudentViewModel studentVM, string id)
        {
            if (!IsAuthenticated(id))
                return RedirectToAction(nameof(Login_Ar));

            if (ModelState.IsValid)
                return RedirectToAction("Fill", "Requests", new { id, formTitle = studentVM.FormTitle });

            return View(studentVM);
        }

        public IActionResult Staff(string id)
        {
            if (id == null)
                return NotFound();

            if (!IsAuthenticated(id))
                return RedirectToAction(nameof(Login_Ar));
            
            var requests = _context.Request
                .Include(r => r.Student)
                .Where(r => !_context.Response.Any(res => res.RequestID == r.ID))
                .Where(r => !_context.Redirect.Any(red => red.RequestID == r.ID && _context.Response.Any(res => res.Type != UserTypeSession)))
                .GroupBy(
                r => r.CurrentTime,
                r => r,
                (key, value) => new { time = key, Requests = value.ToList() });
            var requestsView = new List<RequestViewModel>();

            foreach (var req in requests)
            {
                var requestVM = new RequestViewModel
                {
                    CurrentTime = req.time,
                    Requests = req.Requests
                };
                requestsView.Add(requestVM);
            }
            if (requestsView.Count == 0)
                ViewData["noRequests"] = "لا يوجد طلبات جديدة.";
            return View(requestsView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Staff(string id, int current_time)
        {
            if (!IsAuthenticated(id))
                return RedirectToAction(nameof(Login_Ar));

            return RedirectToAction("Fill", "Responses", new { id, current_time });
        }

        public async Task<IActionResult> HoD(string id)
        {
            if (!IsAuthenticated(id))
                return RedirectToAction(nameof(Login_Ar));

            var redirects = await _context.Redirect
                .Include(red => red.Request)
                    .ThenInclude(r => r.Student)
                .Where(red => red.Type == UserTypeSession && !_context.Response.Any(res => res.RequestID == red.RequestID && res.Type == UserTypeSession))
                .GroupBy(
                red => red.Request.CurrentTime,
                red => red.Request,
                (key, value) => new { Time = key, Requests = value.ToList() })
                .ToListAsync();
            var redirctsVM = new List<RedirectViewModel>();

            foreach (var red in redirects)
            {
                var redirctVM = new RedirectViewModel
                {
                    Time = red.Time,
                    Requests = red.Requests
                };
                redirctsVM.Add(redirctVM);
            }

            if (redirects.Count == 0)
                ViewData["noRedirects"] = "لا يوجد طلبات جديدة.";
            return View(redirctsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HoD(string id, int current_time)
        {
            if (id == null)
                return NotFound();

            if (!IsAuthenticated(id))
                return RedirectToAction(nameof(Login_Ar));

            return RedirectToAction("FillHoD", "Responses", new { id, current_time });
        }

        public async Task<IActionResult> Dean(string id)
        {
            if (!IsAuthenticated(id))
                return RedirectToAction(nameof(Login_Ar));

            var redirects = await _context.Redirect
                .Include(red => red.Request)
                    .ThenInclude(r => r.Student)
                .Where(red => red.Type == UserTypeSession && !_context.Response.Any(res => res.RequestID == red.RequestID && res.Type == UserTypeSession))
                .GroupBy(
                red => red.Request.CurrentTime,
                red => red.Request,
                (key, value) => new { Time = key, Requests = value.ToList() })
                .ToListAsync();
            var redirctsVM = new List<RedirectViewModel>();

            foreach (var red in redirects)
            {
                var redirctVM = new RedirectViewModel
                {
                    Time = red.Time,
                    Requests = red.Requests
                };
                redirctsVM.Add(redirctVM);
            }

            if (redirects.Count == 0)
                ViewData["noRedirects"] = "لا يوجد طلبات جديدة.";
            return View(redirctsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Dean(string id, int current_time)
        {
            if (id == null)
                return NotFound();

            if (!IsAuthenticated(id))
                return RedirectToAction(nameof(Login_Ar));

            return RedirectToAction("FillDean", "Responses", new { id, current_time });
        }
    }
}