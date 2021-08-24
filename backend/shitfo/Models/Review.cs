using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Models
{
    public class Review:BaseEntity
    {
        [Required]
        public int Rate { get; set; }
        [Required]
        [StringLength(maximumLength:500)]
        public string Message { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
