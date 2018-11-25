using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COCAS.Models
{
    public class Section
    {
        [Key]
        public string Number { get; set; }
        
        //Theoritical or Practical.
        [Required]
        public string Activity { get; set; }

        //How many hours? e.g. 3 hours.
        public int Duration { get; set; }

        //In which days?
        public int Day { get; set; }
        
        [Display(Name = "Start Time")]
        public string StartTime { get; set; }

        //End time of section e.g. 2:30.
        [Display(Name = "End Time")]
        public string EndTime { get; set; }

        [Display(Name = "Final Exam")]
        public string FinalExam { get; set; }


        //In which class room? e.g. 101, 304.
        public string Place { get; set; }

        [Required]
        [Display(Name ="Course Code")]
        public string CourseCode { get; set; }
        
        [Display(Name = "Instructor ID")]
        public int? InstructorID { get; set; }


        [ForeignKey("CourseCode")]
        public Course Course { get; set; }

        [ForeignKey("InstructorID")]
        public Instructor Instructor { get; set; }
    }
}
