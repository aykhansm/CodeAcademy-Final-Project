using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Models
{
    public class AppUser : IdentityUser
    {
        
        [Required]
        [StringLength(maximumLength:150)]
        public string FullName { get; set; }
        [Required]
        [StringLength(maximumLength: 150)]
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength: 150)]
        public string Address { get; set; }
        
        [StringLength(maximumLength: 100)]
        public string PhoneNumber { get; set; }
        [StringLength(maximumLength: 100)]
        public string Website { get; set; }
        [StringLength(maximumLength: 1000)]
        public string Description { get; set; }
        [StringLength(maximumLength: 150)]
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public List<Property> Properties { get; set; }
        public List<Review> Reviews { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsAdmin { get; set; }
        
        [NotMapped]
        [StringLength(maximumLength: 50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [StringLength(maximumLength: 50)]
        [DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmedPassword { get; set; }

        [NotMapped]
        [StringLength(maximumLength: 50)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        public List<UserFavorite> UserFavorites { get; set; }
        public List<Booking> Bookings { get; set; }

    }
}
