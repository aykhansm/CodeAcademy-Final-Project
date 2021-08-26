using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shitfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Superadmin,Admin")]
    public class ContactController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ContactController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, AppDbContext context, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Index(int page = 1)
        {
            ViewBag.SelectedPage = page;
            ViewBag.TotalPage = Math.Ceiling(_context.Contacts.Count() / 5m);
            List<Contact> contacts = _context.Contacts.Skip((page - 1) * 5).Take(5).ToList();
            return View(contacts);
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Delete(int id)
        {
            var existContact = _context.Contacts.FirstOrDefault(x => x.Id == id);
            _context.Contacts.Remove(existContact);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Detail(int id)
        {
            var existContact = _context.Contacts.FirstOrDefault(x => x.Id == id);
            return View(existContact);
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult addsubs()
        {
            Setting setting = new Setting();
            _context.Settings.Add(setting);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}