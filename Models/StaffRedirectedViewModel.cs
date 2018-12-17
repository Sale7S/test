using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COCAS.Models
{
    public class StaffRedirectedViewModel
    {
        public string ID { get; set; }

        public string UserType { get; set; }
        
        public List<Response> Responses { get; set; }

        public List<Redirect> Redirects { get; set; }
    }
}
