﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COCAS.Models
{
    public class FormType
    {
        [Key]
        [Required]
        public string Type { get; set; }
    }
}
