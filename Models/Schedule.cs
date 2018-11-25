using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COCAS.Models
{
    public class Schedule
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Student ID")]
        public string CourseCode { get; set; }

        [Display(Name = "Section Number")]
        public string SectionNumber { get; set; }
        
        [ForeignKey("StudentID")]
        public Student Student { get; set; }

        [ForeignKey("SectionNumber")]
        public Section Section { get; set; }
    }
}
