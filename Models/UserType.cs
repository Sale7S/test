using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COCAS.Models
{
    [Display(Name = "Type")]
    public class UserType
    {
        [Key]
        [Required]
        [Display(Name ="Type")]
        public string Type { get; set; }
    }
}
