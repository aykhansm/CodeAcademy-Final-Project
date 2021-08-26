using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shitfo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Superadmin,Admin")]
    public class CategoryController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CategoryController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, AppDbContext context, IWebHostEnvironment env)
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
            ViewBag.TotalPage = Math.Ceiling(_context.Categories.Count() / 5m);
            List<Category> categories = _context.Categories.Include(x => x.Properties).Skip((page - 1) * 5).Take(5).ToList();
            return View(categories);
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Superadmin,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public IActionResult Create(Category category)
        {
            if (category.ImageFile != null)
            {
                if (category.ImageFile.ContentType != "image/png" && category.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("PhotoFile", "File type yanlisdir!");
                    return View();
                }

                if (category.ImageFile.Length > (1024 * 1024) * 2)
                {
                    ModelState.AddModelError("PhotoFile", "Faly olcusu 2MB-dan cox ola bilmez!");
                    return View();
                }

                string rootPath = _env.WebRootPath;
                var filename = Guid.NewGuid().ToString() + category.ImageFile.FileName;
                var path = Path.Combine(rootPath, "uploads", filename);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    category.ImageFile.CopyTo(stream);
                }

                category.Image = filename;

            }

            if (!ModelState.IsValid)
            {
                return View();
            }



            category.CreatedAt = DateTime.UtcNow.AddHours(4);
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Edit(int id)
        {
            var existCategory = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (existCategory != null)
            {
                return View(existCategory);
            }
            return RedirectToAction("index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Edit(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("index");
            }
            var existCategory = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (existCategory != null)
            {
                existCategory.Name = category.Name;

            }
            if (category.ImageFile != null)
            {
                if (category.ImageFile.ContentType != "image/png" && category.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("PhotoFile", "File type yanlisdir!");
                    return View();
                }

                if (category.ImageFile.Length > (1024 * 1024) * 2)
                {
                    ModelState.AddModelError("PhotoFile", "Faly olcusu 2MB-dan cox ola bilmez!");
                    return View();
                }

                string rootPath = _env.WebRootPath;
                var filename = Guid.NewGuid().ToString() + category.ImageFile.FileName;
                var path = Path.Combine(rootPath, "uploads", filename);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    category.ImageFile.CopyTo(stream);
                }

                existCategory.Image = filename;

            }
            else if (category.Image == null)
            {
                if (existCategory.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads", existCategory.Image);
                    if (System.IO.File.Exists(existPath))
                    {
                        System.IO.File.Delete(existPath);
                    }

                    existCategory.Image = null;
                }
            }
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Delete(int id)
        {
            var existCategory = _context.Categories.FirstOrDefault(x => x.Id == id);
            _context.Categories.Remove(existCategory);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
