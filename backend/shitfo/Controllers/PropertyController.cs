using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shitfo.Enums;
using shitfo.Helpers;
using shitfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Controllers
{
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
        [Authorize(Roles = "Member")]
        public IActionResult Post()
        {
            ViewBag.Cities = _context.Cities.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            return View();
        }
        [Authorize(Roles = "Member")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Post(Property property)
        {
            AppUser existUser = await _userManager.FindByNameAsync(User.Identity.Name);

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





            if (property.Photos != null)
            {
                int i = 0;
                foreach (var photo in property.Photos)
                {

                    if (photo.ContentType != "image/png" && photo.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("photos", "Mime type yanlisdir!");
                        return View();
                    }

                    if (photo.Length > (1024 * 1024) * 2)
                    {
                        ModelState.AddModelError("photos", "Faly olcusu 2MB-dan cox ola bilmez!");
                        return View();
                    }

                    string filename = FileManager.Save(_env.WebRootPath, "uploads", photo);
                    PropertyImage propertyImage = new PropertyImage();
                    if (i == 0)
                    {

                        propertyImage.IsPoster = true;
                        propertyImage.Property = property;
                        propertyImage.CreatedAt = DateTime.UtcNow.AddHours(4);
                        propertyImage.Name = filename;

                    }
                    else
                    {
                        propertyImage.IsPoster = false;
                        propertyImage.Property = property;
                        propertyImage.CreatedAt = DateTime.UtcNow.AddHours(4);
                        propertyImage.Name = filename;
                    }
                    i++;
                    _context.PropertyImages.Add(propertyImage);

                }


            }
            if (property.TagIds != null)
            {
                foreach (var tagId in property.TagIds)
                {
                    PropertyTag propertyTag = new PropertyTag
                    {
                        Property = property,
                        TagId = tagId
                    };
                    //book.BookTags.Add(bookTag);
                    _context.PropertyTags.Add(propertyTag);
                }
            }


            property.AppUserId = existUser.Id;
            property.IsFeatured = false;
            property.CreatedAt = DateTime.UtcNow.AddHours(4);
            _context.Properties.Add(property);
            _context.SaveChanges();

            return RedirectToAction("index", "home");
        }


        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Edit(int id)
        {
            AppUser existUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var existProperty = _context.Properties.Include(x => x.Category).Include(x => x.PropertyTags).ThenInclude(x => x.Tag).Include(x => x.PropertyImages).Include(x => x.City).FirstOrDefault(x => x.Id == id);
            if (existProperty == null)
            {
                return RedirectToAction("index", "home");
            }
            ViewBag.Cities = _context.Cities.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            if (existUser.Id == existProperty.AppUserId)
                return View(existProperty);
            else
                return RedirectToAction("index","home");
        }

        public IActionResult Detail(int id)
        {

            var existProperty = _context.Properties.Include(x=>x.AppUser).Include(x=>x.Bookings).Include(x=>x.Category).Include(x=>x.City).Include(x=>x.PropertyImages).Include(x=>x.PropertyTags).ThenInclude(x=>x.Tag).Include(x=>x.Reviews).Include(x=>x.UserFavorites).FirstOrDefault(x => x.Id == id);
            if (existProperty == null)
                return RedirectToAction("index", "home");
            ViewBag.PropertyCount = _context.Properties.Count();
            ViewBag.Cities = _context.Cities.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            existProperty.ViewCount++;
            _context.SaveChanges();
            return View(existProperty);
        }
        [Authorize(Roles = "Member")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Property property)
        {

            AppUser existUser = await _userManager.FindByNameAsync(User.Identity.Name);
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

            _context.SaveChanges();
           
         

            return RedirectToAction("index","home");
        }
        [Authorize(Roles = "Member")]
        public IActionResult Delete(int id)
        {
            var existProperty = _context.Properties.FirstOrDefault(x => x.Id == id);
            _context.Properties.Remove(existProperty);
            _context.SaveChanges();
            return RedirectToAction("index","home");
        }

        [Authorize(Roles = "Member")]
        public  async Task<IActionResult> ToggleFavorite(int id,int? detailpage=0)
        {
            var existProperty = _context.Properties.Include(x => x.AppUser).Include(x => x.Bookings).Include(x => x.Category).Include(x => x.City).Include(x => x.PropertyImages).Include(x => x.PropertyTags).ThenInclude(x => x.Tag).Include(x => x.Reviews).Include(x => x.UserFavorites).FirstOrDefault(x => x.Id == id);
            var existUser =await _userManager.FindByNameAsync(User.Identity.Name);

            if (existProperty == null)
            {
                return RedirectToAction("index", "home");
            }

            var existUserFavorite = _context.UserFavorites.FirstOrDefault(x => x.AppUserId == existUser.Id && x.PropertyId == existProperty.Id);

            if (existUserFavorite != null)
            {
                _context.UserFavorites.Remove(existUserFavorite);
            }
            else
            {
                UserFavorite userFavorite = new UserFavorite()
                {

                    AppUserId = existUser.Id,
                    PropertyId = existProperty.Id,
                    CreatedAt = DateTime.UtcNow.AddHours(4)
                };
                _context.UserFavorites.Add(userFavorite);
            }
            
            _context.SaveChanges();
            if (detailpage == 0)
                return RedirectToAction("index", "home");
            else 
                return RedirectToAction("detail", "property", existProperty);
        }
       
    }
}
