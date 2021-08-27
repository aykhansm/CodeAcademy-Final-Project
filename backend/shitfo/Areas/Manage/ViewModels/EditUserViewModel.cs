using Microsoft.AspNetCore.Mvc;
using shitfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Areas.Manage.ViewModels
{
    public class EditUserViewModel
    {
        public AppUser AppUser { get; set; }
       
        public List<City> Cities { get; set; }
    }
}
