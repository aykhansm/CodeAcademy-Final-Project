using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Models
{
    public class PropertyTag:BaseEntity
    {
        public int TagId { get; set; }
        public Tag Tag { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }
}
