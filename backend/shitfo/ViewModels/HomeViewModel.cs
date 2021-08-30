using shitfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.ViewModels
{
    public class HomeViewModel
    {
        public string ContactTitle { get; set; }
        public string ContactMessage { get; set; }
        public string ContactImage { get; set; }
        public List<Property> LatestProperties { get; set; }
        public List<Property> MostRentedProperties { get; set; }
        public List<Property> FeaturedProperties { get; set; }
        public List<Category> Categories { get; set; }
        public List<City> Cities { get; set; }
        public List<Review> Reviews { get; set; }
        public int UserCount { get; set; }

    }
}
