using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COCAS.Models
{
    public class Department
    {
        [Key]
        [Required]
        public string Code { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
}
