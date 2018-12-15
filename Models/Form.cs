using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COCAS.Models
{
    public class Form
    {
        [Key]
        [Required]
        public string Title { get; set; }

        [Required]
        public string Type { get; set; }


        [ForeignKey("Type")]
        public UserType UserType { get; set; }
    }
}
