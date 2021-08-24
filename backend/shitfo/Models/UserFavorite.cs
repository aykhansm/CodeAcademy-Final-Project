using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Models
{
    public class UserFavorite:BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }
}
