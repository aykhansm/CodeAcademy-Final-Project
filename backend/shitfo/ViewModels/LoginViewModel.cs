using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(maximumLength: 150)]
        public string Email { get; set; }
        [StringLength(maximumLength: 50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsPersistent { get; set; }
    }
}
