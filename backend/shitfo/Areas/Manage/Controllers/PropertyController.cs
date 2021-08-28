using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shitfo.Helpers;
using shitfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Superadmin,Admin")]
    public class PropertyController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public PropertyController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, AppDbContext context, IWebHostEnvironment env)
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
            ViewBag.TotalPage = Math.Ceiling(_context.Properties.Count() / 5m);
            List<Property> properties = _context.Properties.Include(x=>x.AppUser).Include(x=>x.City).Include(x=>x.Category).Include(x=>x.Bookings).Skip((page - 1) * 5).Take(5).ToList();
            return View(properties);
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Edit(int id)
        {
            var existProperty = _context.Properties.Include(x => x.City).Include(x => x.Category).Include(x => x.Bookings).Include(x=>x.PropertyImages).Include(x=>x.PropertyTags).ThenInclude(x=>x.Tag).FirstOrDefault(x => x.Id == id);
            if (existProperty != null)
            {
                
                ViewBag.Cities = _context.Cities.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Tags = _context.Tags.ToList();
                return View(existProperty);
            }
            return RedirectToAction("index");
        }
       

        [Authorize(Roles = "Superadmin,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit(int id, Property property)
        {

           
            var existProperty = _context.Properties.Include(x => x.Category).Include(x => x.PropertyTags).Include(x => x.PropertyImages).Include(x => x.City).FirstOrDefault(x => x.Id == id);

            ViewBag.Cities = _context.Cities.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            if (!_context.Cities.Any(x => x.Id == property.CityId))
            {
                ModelState.AddModelError("AuthorId", "Xeta var!");
            }

            if (!_context.Categories.Any(x => x.Id == property.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Xeta var!");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            existProperty.Area = property.Area;
            existProperty.BathroomCount = property.BathroomCount;
            existProperty.BedroomCount = property.BedroomCount;
            existProperty.CategoryId = property.CategoryId;
            existProperty.CityId = property.CityId;
            existProperty.DailyPrice = property.DailyPrice;
            existProperty.Description = property.Description;
            existProperty.FloorCount = property.FloorCount;
            existProperty.MonthlyPrice = property.MonthlyPrice;
            existProperty.Name = property.Name;
            existProperty.RentType = property.RentType;



            if (property.Photos != null)
            {
                int i = 0;
                foreach (var item in property.Photos)
                {
                    if (item.ContentType != "image/png" && item.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("PosterPhoto", "Mime type yanlisdir!");
                        return View(property);
                    }

                    if (item.Length > (1024 * 1024) * 2)
                    {
                        ModelState.AddModelError("PosterPhoto", "Faly olcusu 2MB-dan cox ola bilmez!");
                        return View(property);
                    }

                    string filename = FileManager.Save(_env.WebRootPath, "uploads", item);

                    PropertyImage propertyImage = new PropertyImage();
                    if (i == 0)
                    {

                        propertyImage.IsPoster = true;
                        propertyImage.PropertyId = id;
                        propertyImage.CreatedAt = DateTime.UtcNow.AddHours(4);
                        propertyImage.Name = filename;

                    }
                    else
                    {
                        propertyImage.IsPoster = false;
                        propertyImage.PropertyId = id;
                        propertyImage.CreatedAt = DateTime.UtcNow.AddHours(4);
                        propertyImage.Name = filename;
                    }
                    i++;

                    _context.PropertyImages.Add(propertyImage);

                }

            }

            List<PropertyImage> existPropertyImageIds = _context.PropertyImages.Where(b => b.PropertyId == property.Id).ToList();
            if (property.PhotoIds == null)
            {
                foreach (var propertyImage in _context.PropertyImages.Where(b => b.PropertyId == property.Id))
                {
                    _context.PropertyImages.Remove(propertyImage);
                }
            }
            else
            {
                foreach (var item in existPropertyImageIds)
                {

                    if (!property.PhotoIds.Contains(item.Id))
                    {
                        _context.PropertyImages.Remove(item);
                    }
                }
            }
            var existTags = _context.PropertyTags.Where(x => x.PropertyId == id).ToList();

            if (property.TagIds != null)
            {
                foreach (var tagId in property.TagIds)
                {
                    var existTag = existTags.FirstOrDefault(x => x.TagId == tagId);
                    if (existTag == null)
                    {
                        PropertyTag propertyTag = new PropertyTag
                        {
                            PropertyId = id,
                            TagId = tagId
                        };
                        _context.PropertyTags.Add(propertyTag);
                    }
                    else
                    {
                        existTags.Remove(existTag);
                    }
                }

            }

            _context.PropertyTags.RemoveRange(existTags);


            _context.SaveChanges();



            return RedirectToAction("index");
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Delete(int id)
        {
            var existProperty = _context.Properties.FirstOrDefault(x => x.Id == id);
            _context.Properties.Remove(existProperty);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
