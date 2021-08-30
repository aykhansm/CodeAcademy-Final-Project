using shitfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.ViewModels
{
    public class PropertyDetailViewModel
    {
        public Property Property { get; set; }

        public Review Review { get; set; }

        public Booking Booking { get; set; }
    }
}
