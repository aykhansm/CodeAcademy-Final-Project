using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shitfo.Models;
using shitfo.ViewModels;
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
                Cities = _context.Cities.Include(x=>x.Properties).OrderByDescending(x => x.Properties.Count()).ToList()
            
            };
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
