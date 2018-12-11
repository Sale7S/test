using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COCAS.Models
{
    public class User
    {
        [Key]
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public string Type { get; set; }


        [ForeignKey("Type")]
        public UserType UserType { get; set; }
    }
}
