using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COCAS.Models
{
    public class Response
    {
        [Key]
        [Required]
        public int RequestID { get; set; }

        [Required]
        public bool Status { get; set; }
        
        public string Reason { get; set; }


        [ForeignKey("RequestID")]
        public Request Request { get; set; }
    }
}