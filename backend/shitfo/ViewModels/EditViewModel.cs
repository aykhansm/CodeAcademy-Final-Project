using shitfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.ViewModels
{
    public class EditViewModel
    {
        public AppUser AppUser { get; set; }

        public List<City> Cities { get; set; }
    }
}
