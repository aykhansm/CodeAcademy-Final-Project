using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shitfo.Models;
using shitfo.Enums;
using shitfo.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IWebHostEnvironment _env;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, AppDbContext context, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _env = env;
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
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("login");
            }
            var existUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == loginViewModel.Email.ToLower() && !x.IsAdmin);
            if (existUser == null)
            {
                ModelState.AddModelError("", "Email or Password is incorrect");
                return View(loginViewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(existUser, loginViewModel.Password, loginViewModel.IsPersistent, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email or Password is incorrect");
                return View(loginViewModel);
            }
            return RedirectToAction("index", "home");
        }
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerView)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "There is an error!");
                return View();
            }
            var checkresult = await _userManager.FindByNameAsync(registerView.Username);
            if (checkresult != null)
            {
                ModelState.AddModelError("UserName", "Username is already taken!");
                return View();
            }
            if (registerView.Username.Contains(" "))
            {
                ModelState.AddModelError("UserName", "Username cannot contain a space!");
                return View();
            }
            AppUser user = new AppUser
            {
                FullName = registerView.FullName,
                UserName = registerView.Username,
                Address = registerView.Address,
                Email = registerView.Email,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                CityId = 1

            };
            var result = await _userManager.CreateAsync(user, registerView.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Code);
                }
                return View();
            }

            await _userManager.AddToRoleAsync(user, "Member");

            await _signInManager.SignInAsync(user, true);

            return RedirectToAction("index", "home");

        }
        [Authorize(Roles = "Member")]
        public IActionResult Edit()
        {
            AppUser user = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            EditViewModel editView = new EditViewModel() {
                AppUser = user,
                Cities = _context.Cities.ToList()
            };
            return View(editView);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Edit(EditViewModel user, int cityId)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("edit");
            }
            AppUser userr = await _userManager.FindByNameAsync(User.Identity.Name);

            if (_userManager.Users.Any(x => x.UserName == user.AppUser.UserName && x.Id != userr.Id))
            {
                ModelState.AddModelError("UserName", "UserName already taken!");
                return View();
            }
            if (_userManager.Users.Any(x => x.Email == user.AppUser.Email && x.Id != userr.Id))
            {
                ModelState.AddModelError("UserName", "Email already taken!");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            userr.UserName = user.AppUser.UserName;

            userr.FullName = user.AppUser.FullName;
            userr.Address = user.AppUser.Address;
            userr.CityId = cityId;
            userr.Description = user.AppUser.Description;
            userr.Email = user.AppUser.Email;
            userr.Website = user.AppUser.Website;
            userr.PhoneNumber = user.AppUser.PhoneNumber;

            if (user.AppUser.ImageFile != null)
            {
                if (user.AppUser.ImageFile.ContentType != "image/png" && user.AppUser.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("PhotoFile", "File type yanlisdir!");
                    return View();
                }

                if (user.AppUser.ImageFile.Length > (1024 * 1024) * 2)
                {
                    ModelState.AddModelError("PhotoFile", "Faly olcusu 2MB-dan cox ola bilmez!");
                    return View();
                }

                string rootPath = _env.WebRootPath;
                var filename = Guid.NewGuid().ToString() + "." + user.AppUser.ImageFile.ContentType.Substring(user.AppUser.ImageFile.ContentType.IndexOf("/") + 1);
                var path = Path.Combine(rootPath, "uploads", filename);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    user.AppUser.ImageFile.CopyTo(stream);
                }

                userr.Image = filename;

            }
            else if (user.AppUser.Image == null)
            {
                if (userr.Image != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads", userr.Image);
                    if (System.IO.File.Exists(existPath))
                    {
                        System.IO.File.Delete(existPath);
                    }

                    userr.Image = null;
                }
            }

            if (!string.IsNullOrWhiteSpace(user.AppUser.Password))
            {
                if (string.IsNullOrWhiteSpace(user.AppUser.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "CurrentPassword can not be emtpy");
                    return View();
                }

                var result = await _userManager.ChangePasswordAsync(userr, user.AppUser.CurrentPassword, user.AppUser.Password);
                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                    return View();
                }
            }
            await _userManager.UpdateAsync(userr);

            await _signInManager.SignInAsync(userr, true);
            return RedirectToAction("index", "home");
        }
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> MyProperties()
        {

            var existUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (existUser != null) {
                List<Property> properties = _context.Properties.Where(x => x.AppUserId == existUser.Id).Include(x => x.AppUser).Include(x => x.Bookings).Include(x => x.Category).Include(x => x.City).Include(x => x.PropertyImages).Include(x => x.PropertyTags).ThenInclude(x => x.Tag).Include(x => x.Reviews).Include(x => x.UserFavorites).ToList();
                return View(properties);
            }
            return RedirectToAction("index", "home");
        }
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> MyFavorites()
        {

            var existUser = await _userManager.FindByNameAsync(User.Identity.Name);

            var MyUserFavourites = _context.UserFavorites.Where(x => x.AppUserId == existUser.Id).Include(x => x.Property).ThenInclude(x => x.PropertyImages).Include(x => x.Property).ThenInclude(x => x.Category).ToList();

            if (existUser != null)
            {
                List<UserFavorite> userFavorites = MyUserFavourites;
                return View(MyUserFavourites);
            }
            return RedirectToAction("index", "home");
        }
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> MyBookings()
        {

            var existUser = await _userManager.FindByNameAsync(User.Identity.Name);

            var MyBookings = _context.Bookings.Where(x => x.AppUserId == existUser.Id && x.Property.AppUserId != existUser.Id).Include(x => x.Property).ThenInclude(x => x.PropertyImages).Include(x => x.Property).ThenInclude(x => x.Category).ToList();

            if (existUser != null)
            {
                List<Booking> mybookings = MyBookings;
                return View(mybookings);
            }
            return RedirectToAction("index", "home");
        }
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> BookingsToMe(int? renttypeId)
        {

            var existUser = await _userManager.FindByNameAsync(User.Identity.Name);

            var BookingsToMe = _context.Bookings.Where(x => x.AppUserId != existUser.Id && x.Property.AppUserId == existUser.Id).Include(x => x.AppUser).Include(x => x.Property).ThenInclude(x => x.PropertyImages).Include(x => x.Property).ThenInclude(x => x.Category).ToList();

            if (existUser != null)
            {
                List<Booking> mybookings = BookingsToMe;
                return View(mybookings);
            }
            return RedirectToAction("index", "home");
        }
        [Authorize(Roles = "Member")]
        public async Task<IActionResult> StatusChange(int? renttypeId,int? bookingId)
        {
            var existUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var BookingsToMe = _context.Bookings.Where(x => x.AppUserId != existUser.Id && x.Property.AppUserId == existUser.Id).Include(x => x.AppUser).Include(x => x.Property).ThenInclude(x => x.PropertyImages).Include(x => x.Property).ThenInclude(x => x.Category).ToList();
            var existBooking = _context.Bookings.FirstOrDefault(x => x.Id == bookingId);
            if (existUser != null) { 
            if(existBooking!=null && renttypeId != null)
            {
                if (renttypeId == 0)
                    existBooking.Status = BookingStatus.Pending;            
                else if (renttypeId == 1)
                    existBooking.Status = BookingStatus.Accepted;
                
                else
                    existBooking.Status = BookingStatus.Rejected;
                
            }
           
                
                List<Booking> mybookings = BookingsToMe;
                _context.SaveChanges();
                return RedirectToAction("bookingstome", "account",mybookings);
            }

            return RedirectToAction("index", "home");
            
        }
    } 
}
