using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCAS.Models
{
    public class CreateResponseViewModel
    {
        public Request Request { get; set; }
        
        public bool Status { get; set; }

        public string Reason { get; set; }
    }
}
