using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCAS.Models
{
    public class RequestViewModel
    {
        public DateTime? CurrentTime { get; set; }

        public string StudentID { get; set; }

        public List<Request> Requests { get; set; }
    }
}
