using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Models
{
    public class Subscription:BaseEntity
    {
        [Required]
        [StringLength(maximumLength:100)]
        public string Email { get; set; }
    }
}
