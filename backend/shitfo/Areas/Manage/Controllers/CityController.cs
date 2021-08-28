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
    public class CityController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CityController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, AppDbContext context, IWebHostEnvironment env)
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
            ViewBag.TotalPage = Math.Ceiling(_context.Cities.Count() / 5m);
            List<City> cities = _context.Cities.Include(x => x.Properties).Include(x=>x.AppUsers).Skip((page - 1) * 5).Take(5).ToList();
            return View(cities);
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Create(City city)
        {
            if (city.ImageFile != null)
            {
                if (city.ImageFile.ContentType != "image/png" && city.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("PhotoFile", "File type yanlisdir!");
                    return View();
                }

                if (city.ImageFile.Length > (1024 * 1024) * 2)
                {
                    ModelState.AddModelError("PhotoFile", "Faly olcusu 2MB-dan cox ola bilmez!");
                    return View();
                }

                string rootPath = _env.WebRootPath;
                var filename = Guid.NewGuid().ToString() + city.ImageFile.ContentType.Substring(city.ImageFile.ContentType.IndexOf("/") + 1);
                var path = Path.Combine(rootPath, "uploads", filename);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    city.ImageFile.CopyTo(stream);
                }

                city.Image = filename;

            }

            if (!ModelState.IsValid)
            {
                return View();
            }



            city.CreatedAt = DateTime.UtcNow.AddHours(4);
            _context.Cities.Add(city);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Edit(int id)
        {
            var existCity = _context.Cities.FirstOrDefault(x => x.Id == id);
            if (existCity != null)
            {
                return View(existCity);
            }
            return RedirectToAction("index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Edit(int id, City city)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("index");
            }
            var existCity = _context.Cities.FirstOrDefault(x => x.Id == id);
            if (existCity != null)
            {
                existCity.Name = city.Name;

            }
            if (city.ImageFile != null)
            {
                if (city.ImageFile.ContentType != "image/png" && city.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("PhotoFile", "File type yanlisdir!");
                    return View();
                }

                if (city.ImageFile.Length > (1024 * 1024) * 2)
                {
                    ModelState.AddModelError("PhotoFile", "Faly olcusu 2MB-dan cox ola bilmez!");
                    return View();
                }

                string rootPath = _env.WebRootPath;
                var filename = Guid.NewGuid().ToString() + city.ImageFile.ContentType.Substring(city.ImageFile.ContentType.IndexOf("/") + 1);
                var path = Path.Combine(rootPath, "uploads", filename);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    city.ImageFile.CopyTo(stream);
                }

                existCity.Image = filename;

            }
            else if (city.Image == null)
            {
                if (existCity.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads", existCity.Image);
                    if (System.IO.File.Exists(existPath))
                    {
                        System.IO.File.Delete(existPath);
                    }

                    existCity.Image = null;
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
