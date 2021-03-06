﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COCAS.Models
{
    public class Student
    {
        [Key]
        [Required]
        public string ID { get; set; }
        
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        
        [Required]
        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; }

        
        [ForeignKey("DepartmentCode")]
        public Department Department { get; set; }
    }
}
