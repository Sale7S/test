using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCAS.Models
{
    public class ResponseViewModel
    {
        public string ID { get; set; }
        public List<Request> Requests { get; set; }
        public List<Response> Responses { get; set; }

    }
}
