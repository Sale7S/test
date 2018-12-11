using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COCAS.Models
{
    public class Time
    {
        [Key]
        public DateTime? Current { get; set; }
    }
}
