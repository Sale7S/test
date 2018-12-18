using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COCAS.Models
{
    public class HoD
    {
        [Key]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; }

        [ForeignKey("DepartmentCode")]
        public Department Department {get; set;}

        [ForeignKey("Username")]
        public User User { get; set; }
    }
}