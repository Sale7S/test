using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using COCAS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace COCAS.Controllers
{
    public class ResponsesController : BaseController
    {
        private readonly COCASContext _context;

        public ResponsesController(COCASContext context)
        {
            _context = context;
        }

        //This method list all students' requests and responses.
        public async Task<IActionResult> Student(string id)
        {
            if (!IsAuthenticated(id) || !IsStudent())
                return RedirectToAction("Login_Ar", "Users");

            var studentRequests = await _context.Request
                .Where(r => r.StudentID == id)
                .Include(r => r.Section)
                    .ThenInclude(r => r.Course)
                .ToListAsync();
            var studentResponses = await _context.Response
                .Where(res => res.Request.StudentID == id && res.Type == "Staff")
                .ToListAsync();
            var studentNonRespondedRequests = new List<Request>();

            foreach (var item in studentRequests)
                if (!studentResponses.Any(res => res.RequestID == item.ID))
                    studentNonRespondedRequests.Add(item);

            var responseVM = new ResponseViewModel()
            {
                ID = id,
                Requests = studentNonRespondedRequests,
                Responses = studentResponses
            };

            return View(responseVM);
        }

        // This method shows the Staff the form he asked to response to.
        public async Task<IActionResult> Fill(string id, int current_time)
        {
            if (!IsAuthenticated(id) || !IsStaff())
                return RedirectToAction("Login_Ar", "Users");

            var studentsRequestsPerForm = await _context.Request
                .Include(r => r.Student)
                .Include(r => r.Section)
                    .ThenInclude(se => se.Course)
                .Where(r => r.CurrentTime == current_time)
                .ToListAsync();

            var responsesVM = new List<CreateResponseViewModel>();
            foreach (var request in studentsRequestsPerForm)
            {
                var responseVM = new CreateResponseViewModel
                {
                    Request = request
                };
                responsesVM.Add(responseVM);
            }
            var userType = _context.UserType.Where(ut => ut.Type != UserTypeSession && ut.Type != "Student");
            ViewData["UserType"] = new SelectList(userType, "Type", "TypeAr");

            return View(responsesVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Fill([Bind("[i].Request.ID, [i].Status, [i].Reason, [i].IsRedirected, [i].UserType")] List<CreateResponseViewModel> responsesVM, string id, string unused)
        {
            if (!IsAuthenticated(id) || !IsStaff())
                return RedirectToAction("Login_Ar", "Users");

            foreach (var responseVM in responsesVM)
            {
                if (responseVM.IsRedirected)
                {
                    var redirect = new Redirect()
                    {
                        RequestID = responseVM.Request.ID,
                        Type = responseVM.UserType
                    };
                    _context.Redirect.Add(redirect);
                }
                else
                {
                    var response = new Response()
                    {
                        RequestID = responseVM.Request.ID,
                        Status = responseVM.Status,
                        Reason = responseVM.Reason,
                        Type = UserTypeSession
                    };
                    _context.Response.Add(response);
                }
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Staff", "Users", new { id });
        }

        public async Task<IActionResult> Staff()
        {
            if (!IsStaff())
                return RedirectToAction("Login_Ar", "Users");

            var redirects = await _context.Redirect
                .Include(red => red.Request)
                    .ThenInclude(r => r.Section)
                        .ThenInclude(se => se.Course)
                .Include(res => res.UserType)
                .ToListAsync();
            var responses = await _context.Response
                .Include(res => res.Request)
                    .ThenInclude(r => r.Section)
                        .ThenInclude(se => se.Course)
                .Include(res => res.UserType)
                .Where(res => redirects.Any(red => red.RequestID == res.RequestID && red.Type == res.Type))
                .ToListAsync();
            var staffNonRespondedRedirects = new List<Redirect>();

            foreach (var item in redirects)
                if (!responses.Any(red => red.RequestID == item.RequestID))
                    staffNonRespondedRedirects.Add(item);
            
            var staffRedirectedVM = new StaffRedirectedViewModel()
            {
                //ID = id,
                //UserType = HoD
                Redirects = staffNonRespondedRedirects,
                Responses = responses
            };

            return View(staffRedirectedVM);
        }

        // This method shows the HoD the form he asked to responses to.
        public async Task<IActionResult> FillHoD(string id, int current_time)
        {
            if (!IsAuthenticated(id) || !IsHoD())
                return RedirectToAction("Login_Ar", "Users");

            var studentsRequestsPerForm = await _context.Redirect
                .Include(red => red.Request)
                    .ThenInclude(r => r.Student)
                .Include(red => red.Request)
                    .ThenInclude(r => r.Section)
                        .ThenInclude(se => se.Course)
                .Where(red => red.Request.CurrentTime == current_time && red.Type == UserTypeSession)
                .ToListAsync();

            var responsesVM = new List<CreateRedirectViewModel>();
            foreach (var request in studentsRequestsPerForm)
            {
                var responseVM = new CreateRedirectViewModel
                {
                    Request = request.Request
                };
                responsesVM.Add(responseVM);
            }

            return View(responsesVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FillHoD([Bind("[i].Request.ID, [i].Status, [i].Reason")] List<CreateRedirectViewModel> redirectsVM, string id, string unused)
        {
            if (!IsAuthenticated(id) || !IsHoD())
                return RedirectToAction("Login_Ar", "Users");

            foreach (var redirectVM in redirectsVM)
            {
                var response = new Response()
                {
                    RequestID = redirectVM.Request.ID,
                    Status = redirectVM.Status,
                    Reason = redirectVM.Reason,
                    Type = UserTypeSession
                };
                _context.Response.Add(response);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Login_Ar", "Users");
        }

        // This method shows the Dean the form he asked to responses to.
        public async Task<IActionResult> FillDean(string id, int current_time)
        {
            if (!IsAuthenticated(id) || !IsDean())
                return RedirectToAction("Login_Ar", "Users");

            var studentsRequestsPerForm = await _context.Redirect
                .Include(red => red.Request)
                    .ThenInclude(r => r.Student)
                .Include(red => red.Request)
                    .ThenInclude(r => r.Section)
                        .ThenInclude(se => se.Course)
                .Where(red => red.Request.CurrentTime == current_time && red.Type == UserTypeSession)
                .ToListAsync();

            var responsesVM = new List<CreateRedirectViewModel>();
            foreach (var request in studentsRequestsPerForm)
            {
                var responseVM = new CreateRedirectViewModel
                {
                    Request = request.Request
                };
                responsesVM.Add(responseVM);
            }

            return View(responsesVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FillDean([Bind("[i].Request.ID, [i].Status, [i].Reason")] List<CreateRedirectViewModel> redirectsVM, string id, string unused)
        {
            if (!IsAuthenticated(id) || !IsDean())
                return RedirectToAction("Login_Ar", "Users");

            foreach (var redirectVM in redirectsVM)
            {
                var response = new Response()
                {
                    RequestID = redirectVM.Request.ID,
                    Status = redirectVM.Status,
                    Reason = redirectVM.Reason,
                    Type = UserTypeSession
                };
                _context.Response.Add(response);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Login_Ar", "Users");
        }
    }
}