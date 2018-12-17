using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace COCAS.Models
{
    public class Redirect
    {
        [Key]
        [Required]
        public int RequestID { get; set; }

        public string Type { get; set; }

        [Required]
        public bool Status { get; set; }

        public string Reason { get; set; }


        [ForeignKey("RequestID")]
        public Request Request { get; set; }

        [ForeignKey("Type")]
        public UserType UserType { get; set; }
    }
}
