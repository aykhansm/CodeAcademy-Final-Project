using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(maximumLength: 150)]
        public string FullName { get; set; }
        [Required]
        [StringLength(maximumLength: 150)]
        public string Username { get; set; }
        [Required]
        [StringLength(maximumLength: 150)]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 150)]
        public string Address { get; set; }
       
        [StringLength(maximumLength: 50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
