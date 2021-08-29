using Microsoft.AspNetCore.Http;
using shitfo.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Models
{
    public class Property : BaseEntity
    {
        public int CityId { get; set; }
        public City City { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Review> Reviews { get; set; }
        public List<PropertyTag> PropertyTags { get; set; }
        public List<UserFavorite> UserFavorites { get; set; }
        public List<Booking> Bookings { get; set; }
       
        public bool IsFeatured { get; set; }
        [Required]
        [StringLength(maximumLength:150)]
        public string Name { get; set; }
        [NotMapped]
        public List<IFormFile> Photos { get; set; }
        [NotMapped]
        public List<int> TagIds { get; set; }
        [NotMapped]
        public List<int> PhotoIds { get; set; }
        public List<PropertyImage> PropertyImages { get; set; }
        public RentType RentType { get; set; }
       
        public int DailyPrice { get; set; }
       
        public int MonthlyPrice { get; set; }
        public int ViewCount { get; set; }
        
        public int Area { get; set; }
        [Required]
        public int BedroomCount { get; set; }
        [Required]
        public int BathroomCount { get; set; }
        [Required]
        public int FloorCount { get; set; }
        [Required]
        [StringLength(maximumLength: 1000)]
        public string Description { get; set; }
        
    }
}
