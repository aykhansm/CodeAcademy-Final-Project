using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shitfo.Areas.Manage.ViewModels;
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
    public class AppUserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public AppUserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, AppDbContext context, IWebHostEnvironment env)
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
            ViewBag.TotalPage = Math.Ceiling(_userManager.Users.Count() / 5m);
            List<AppUser> appUsers = _userManager.Users.Skip((page - 1) * 5).Take(5).ToList();
            return View(appUsers);
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Create()
        {
            if (User.IsInRole("Superadmin"))
                ViewBag.Roles = _context.Roles.Where(x => x.Name == "Admin" || x.Name == "Member");
            if (User.IsInRole("Admin"))
                ViewBag.Roles = _context.Roles.Where(x => x.Name == "Member");
            return View();
        }
        [Authorize(Roles = "Superadmin,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel createUserViewModel)
        {

            if (User.IsInRole("Superadmin"))
                ViewBag.Roles = _context.Roles.Where(x => x.Name == "Admin" || x.Name == "Member");
            if (User.IsInRole("Admin"))
                ViewBag.Roles = _context.Roles.Where(x =>x.Name == "Member");
            if (!ModelState.IsValid) return View();
            var checkresult = await _userManager.FindByNameAsync(createUserViewModel.UserName);
            if (checkresult != null)
            {
                ModelState.AddModelError("UserName", "Username is already taken!");
                return View();
            }
            if (createUserViewModel.UserName.Contains(" "))
            {
                ModelState.AddModelError("UserName", "Username cannot contain a space!");
                return View();
            }
            AppUser user = new AppUser
            {
                FullName = createUserViewModel.FullName,
                UserName = createUserViewModel.UserName,
                Email=createUserViewModel.Email,
                CreatedAt=DateTime.UtcNow.AddHours(4),
                CityId=1,
                Address="-"
            };
            var role = await _roleManager.FindByIdAsync(createUserViewModel.UserRoleId);
            if (await _roleManager.GetRoleNameAsync(role) != "Member")
            {
                user.IsAdmin = true;
            }
            var result = await _userManager.CreateAsync(user, createUserViewModel.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Code);
                }
                return View();
            }

            await _userManager.AddToRoleAsync(user, await _roleManager.GetRoleNameAsync(role));



            return RedirectToAction("index");
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Edit(string id)
        {

            AppUser user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            EditUserViewModel editView = new EditUserViewModel()
            {
                AppUser = user,
                Cities = _context.Cities.ToList()
            };
            return View(editView);

           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Superadmin,Admin")]
        public async Task<IActionResult> Edit(string id,EditUserViewModel editvm, int cityId)
        {
            AppUser userr = await _userManager.FindByIdAsync(id);

            if (_userManager.Users.Any(x => x.UserName == editvm.AppUser.UserName && x.Id != userr.Id))
            {
                ModelState.AddModelError("UserName", "UserName already taken!");
                return View();
            }
            if (_userManager.Users.Any(x => x.Email == editvm.AppUser.Email && x.Id != userr.Id))
            {
                ModelState.AddModelError("UserName", "Email already taken!");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            userr.UserName = editvm.AppUser.UserName;

            userr.FullName = editvm.AppUser.FullName;
            userr.Address = editvm.AppUser.Address;
            userr.CityId = cityId;
            userr.Description = editvm.AppUser.Description;
            userr.Email = editvm.AppUser.Email;
            userr.Website = editvm.AppUser.Website;
            userr.PhoneNumber = editvm.AppUser.PhoneNumber;

            if (editvm.AppUser.ImageFile != null)
            {
                if (editvm.AppUser.ImageFile.ContentType != "image/png" && editvm.AppUser.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "File type yanlisdir!");
                    return View();
                }

                if (editvm.AppUser.ImageFile.Length > (1024 * 1024) * 2)
                {
                    ModelState.AddModelError("ImageFile", "Faly olcusu 2MB-dan cox ola bilmez!");
                    return View();
                }

                string rootPath = _env.WebRootPath;
                var filename = Guid.NewGuid().ToString()+"." + editvm.AppUser.ImageFile.ContentType.Substring(editvm.AppUser.ImageFile.ContentType.IndexOf("/") + 1);
                var path = Path.Combine(rootPath, "uploads", filename);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    editvm.AppUser.ImageFile.CopyTo(stream);
                }

                userr.Image = filename;

            }
            else if (editvm.AppUser.Image == null)
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

            if (!string.IsNullOrWhiteSpace(editvm.AppUser.Password))
            {

                if (string.IsNullOrWhiteSpace(editvm.AppUser.CurrentPassword))
                {
                    ModelState.AddModelError("", "CurrentPassword can not be emtpy");
                    return View();
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(userr);
                var result = await _userManager.ResetPasswordAsync(userr,token, editvm.AppUser.Password);
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

            
            return RedirectToAction("index", "dashboard");
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Delete(string id)
        {
            var existAppUser = _userManager.Users.FirstOrDefault(x => x.Id == id);
            _context.Users.Remove(existAppUser);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
