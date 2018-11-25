using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COCAS.Models
{
    [Display(Name = "User Type")]
    public class UserType
    {
        [Key]
        [Display(Name ="Type")]
        public string Type { get; set; }
    }
}
