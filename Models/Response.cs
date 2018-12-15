using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COCAS.Models
{
    public class Response
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public int RequestID { get; set; }

        [Required]
        public bool Status { get; set; }
        
        public string Reason { get; set; }


        [ForeignKey("RequestID")]
        public Request Request { get; set; }
    }
}