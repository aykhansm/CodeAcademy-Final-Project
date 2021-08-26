using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shitfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Superadmin,Admin")]
    public class TagController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public TagController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, AppDbContext context, IWebHostEnvironment env)
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
            ViewBag.TotalPage = Math.Ceiling(_context.Tags.Count() / 5m);
            List<Tag> tags = _context.Tags.Include(x => x.PropertyTags).Skip((page - 1) * 5).Take(5).ToList();
            return View(tags);
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Create(Tag tag)
        {
           

            if (!ModelState.IsValid)
            {
                return View();
            }



            tag.CreatedAt = DateTime.UtcNow.AddHours(4);
            _context.Tags.Add(tag);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Edit(int id)
        {
            var existTag = _context.Tags.FirstOrDefault(x => x.Id == id);
            if (existTag != null)
            {
                return View(existTag);
            }
            return RedirectToAction("index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Edit(int id, Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("index");
            }
            var existTag = _context.Tags.FirstOrDefault(x => x.Id == id);
            if (existTag != null)
            {
                existTag.Name = tag.Name;

            }
           
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Delete(int id)
        {
            var existTag = _context.Tags.FirstOrDefault(x => x.Id == id);
            _context.Tags.Remove(existTag);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
