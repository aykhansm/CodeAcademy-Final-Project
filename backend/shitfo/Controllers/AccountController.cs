using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shitfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;

        }
        //public async Task<IActionResult> CreateAdmin()
        //{


        

           
        //    AppUser admin = new AppUser()
        //    {
        //        FullName = "Ayxan Ismayilzade",
        //        UserName = "ayxanism",
        //        Email = "ayxan@gmail.com",
        //        Address = "-",
        //        CityId = 1,
        //        IsAdmin = true
        //    };
        //    await _userManager.CreateAsync(admin, "admin123");
        //    await _userManager.AddToRoleAsync(admin, "Superadmin");

        //    return Content("ok");
        //}

        public IActionResult Index()
        {
            return View();
        }
    }
}
