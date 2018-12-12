using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCAS.Models
{
    public class RequestViewModel
    {
        public int CurrentTime { get; set; }

        public List<Request> Requests { get; set; }
    }
}
