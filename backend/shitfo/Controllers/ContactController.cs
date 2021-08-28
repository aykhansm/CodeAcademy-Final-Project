using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shitfo.Models;
using shitfo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Controllers
{
    public class ContactController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public ContactController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactViewModel contactViewModel)
        {
            if (contactViewModel != null)
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "There is an error!");
                    return RedirectToAction("index");
                }
                Contact contact = new Contact()
                {
                    Fullname = contactViewModel.Fullname,
                    Email = contactViewModel.Email,
                    
                    Message = contactViewModel.Message,
                    CreatedAt = DateTime.UtcNow.AddHours(4)
                };
                _context.Contacts.Add(contact);
                _context.SaveChanges();
            }
            return RedirectToAction("index","contact");
        }
    }
}
