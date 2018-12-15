using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace COCAS.Models
{
    public class RedirectRequest
    {
        public int RequestID { get; set; }


        [ForeignKey("RequestID")]
        public Request Request { get; set; }
    }
}
