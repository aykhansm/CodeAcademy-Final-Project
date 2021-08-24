using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Models
{
    public class PropertyImage:BaseEntity
    {
        public int PropertyId { get; set; }
        public Property Property { get; set; }

        [StringLength(maximumLength:150)]
        public string Name { get; set; }
        public bool IsPoster { get; set; }
    }
}
