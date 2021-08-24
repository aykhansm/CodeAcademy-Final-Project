using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Models
{
    public class Contact:BaseEntity
    {
        [Required]
        [StringLength(maximumLength: 100)]
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string Fullname { get; set; }
        [Required]
        [StringLength(maximumLength: 500)]
        public string Message { get; set; }

    }
}
