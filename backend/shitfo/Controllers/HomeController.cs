using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shitfo.Models;
using shitfo.ViewModels;
using shitfo.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;

        }
        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel() {
                ContactImage = _context.Settings.First().ContactImage,
                ContactTitle = _context.Settings.First().ContactTitle,
                ContactMessage = _context.Settings.First().ContactMessage,
                Categories = _context.Categories.Include(x => x.Properties).ToList(),
                Cities = _context.Cities.Include(x=>x.Properties).OrderByDescending(x => x.Properties.Count()).ToList(),
                UserCount=_context.Users.Where(x=>!x.IsAdmin).Count(),
                LatestProperties=_context.Properties.Include(x=>x.Category).Include(x=>x.City).Include(x=>x.PropertyImages).Include(x=>x.UserFavorites).OrderByDescending(x=>x.CreatedAt).Include(x => x.AppUser).Take(8).ToList(),
                MostRentedProperties=_context.Properties.Include(x=>x.Bookings).Include(x => x.Category).Include(x => x.City).Include(x => x.PropertyImages).Include(x => x.UserFavorites).Include(x => x.AppUser).OrderByDescending(x=>x.Bookings.Count).Take(8).ToList(),
                FeaturedProperties=_context.Properties.Include(x => x.Category).Include(x => x.City).Include(x => x.PropertyImages).Include(x => x.UserFavorites).Include(x=>x.AppUser).Where(x=>x.IsFeatured).Take(8).ToList(),
                Reviews=_context.Reviews.Include(x=>x.AppUser).OrderByDescending(x=>x.CreatedAt).Take(8).ToList()

            };

            ViewBag.TotalUsersCount = _context.Users.Where(x=>!x.IsAdmin).Count();
            ViewBag.TotalBookings = _context.Bookings.Where(x=>x.Status==BookingStatus.Accepted).Count();
            ViewBag.TotalCities = _context.Cities.Count();
            ViewBag.TotalCategories = _context.Categories.Count();
            return View(homeViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Subscribe(Subscription subscription)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "There is an error!");
                return RedirectToAction("index");
            }
            if (_context.Subscriptions.Any(x => x.Email.ToLower() == subscription.Email.ToLower()))
            {
                ViewBag.SubscribingError = "Bele bir abune artiq var!";


            }
            subscription.CreatedAt = DateTime.UtcNow.AddHours(4);
            _context.Subscriptions.Add(subscription);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
