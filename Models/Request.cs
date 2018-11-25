﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COCAS.Models
{
    public class Request
    {
        public int ID { get; set; }
        
        [Required]
        public string FormTitle { get; set; }

        [Required]
        public string StudentID { get; set; }

        //[Required]
        public string SectionNumber { get; set; }


        [ForeignKey("FormTitle")]
        public Form Form { get; set; }

        [ForeignKey("StudentID")]
        public Student Student { get; set; }

        [ForeignKey("SectionNumber")]
        public Section Section { get; set; }
    }
}