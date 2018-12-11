using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace COCAS.Models
{
    public class StaffViewModel
    {
        public List<Request> Requests { get; set; }
        
        public string FormTitle { get; set; }
    }
}
