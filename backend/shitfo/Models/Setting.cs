using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Models
{
    public class Setting:BaseEntity
    {
       
        [StringLength(maximumLength: 150)]
        public string Logo { get; set; }
        [NotMapped]
        public IFormFile LogoFile { get; set; }
       
        [StringLength(maximumLength: 150)]
        public string ContactImage { get; set; }
        [NotMapped]
        public IFormFile ContactImageFile { get; set; }
        
        [StringLength(maximumLength: 100)]
        public string ContactPhone { get; set; }
        
        [StringLength(maximumLength: 100)]
        public string ContactEmailAddress { get; set; }
        
        [StringLength(maximumLength: 150)]
        public string ContactAddress { get; set; }

    }
}
