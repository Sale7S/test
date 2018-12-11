using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COCAS.Models
{
    public class UploadViewModel
    {
        [Required]
        public List<IFormFile> Files { get; set; }
    }
}