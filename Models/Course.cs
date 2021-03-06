﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COCAS.Models
{
    public class Course
    {
        [Key]
        [Required]
        public string Code { get; set; }

        [Required]
        public string Title { get; set; }
        
        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; }


        [ForeignKey("DepartmentCode")]
        public Department Department { get; set; }
    }
}
