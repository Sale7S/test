using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace COCAS.Models
{
    public class StudentViewModel
    {
        public string FormTitle { get; set; }
        
        public Form Form { get; set; }

        public List<Schedule> Schedule { get; set; }
    }
}
