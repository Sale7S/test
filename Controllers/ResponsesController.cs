﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COCAS.Models;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Student(string id)
        {
            if (id == null)
                return NotFound();

            if (!IsStudent())
                return RedirectToAction("Login_Ar", "Users");

            var studentRequests = await _context.Request
                .Where(r => r.StudentID == id)
                .Include(r => r.Section)
                    .ThenInclude(r => r.Course)
                .ToListAsync();
            var studentResponses = await _context.Response
                .Where(res => res.Request.StudentID == id)
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

        public async Task<IActionResult> Fill(string id, DateTime current_time, string student_id)
        {
            if (id == null || current_time == null || student_id == null)
                return NotFound();

            if(!IsStaff())
                return RedirectToAction("Login_Ar", "Users");

            var studentsRequestsPerForm = await _context.Request
                .Include(r => r.Student)
                .Include(r => r.Section)
                    .ThenInclude(se => se.Course)
                .GroupBy(
                r => new { r.CurrentTime, r.StudentID },
                r => r,
                (key, value) => new { StudentRequest = key, Requests = value.ToList() })
                .Where(r => r.StudentRequest.StudentID == student_id)
                .ToListAsync();

            var responsesVM = new List<CreateResponseViewModel>();
            foreach (var item in studentsRequestsPerForm)
            {
                foreach (var request in item.Requests)
                {
                    var responseVM = new CreateResponseViewModel
                    {
                        Request = request
                    };
                    responsesVM.Add(responseVM);
                }
            }

            return View(responsesVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Fill([Bind("[i].Request.ID, [i].Status, [i].Reason")] List<CreateResponseViewModel> responsesVM, string id)
        {
            if (!IsStaff())
                return RedirectToAction("Login_Ar", "Users");
            
            foreach (var responseVM in responsesVM)
            {
                var response = new Response()
                {
                    RequestID = responseVM.Request.ID,
                    Status = responseVM.Status,
                    Reason = responseVM.Reason
                };
                _context.Response.Add(response);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("Staff", "Users", new { id });
        }
    }
}