using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Models
{
    public class Tag:BaseEntity
    {
        [Required]
        [StringLength(maximumLength:100)]
        public string Name { get; set; }
        public List<PropertyTag> PropertyTags { get; set; }
    }
}
