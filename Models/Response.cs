using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COCAS.Models
{
    public class Response
    {
        [Key]
        public int ID {
            get { return ID; }
            set { ID = RequestID; }

        }
        
        public int RequestID { get; set; }

        [Required]
        public Boolean Status { get; set; }
        
        public string Reason { get; set; }


        [ForeignKey("RequestID")]
        public Request Request { get; set; }
    }
}