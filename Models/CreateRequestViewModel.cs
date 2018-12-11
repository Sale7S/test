using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COCAS.Models
{
    public class CreateRequestViewModel
    {
        public string FormTitle { get; set; }

        public List<string> SectionsNumbers { get; set; }

        public Student Student { get; set; }

        public List<Section> Sections { get; set; }
    }
}