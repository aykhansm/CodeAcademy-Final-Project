using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class SettingController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SettingController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, AppDbContext context, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _env = env;
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Index()
        {

            Setting setting = _context.Settings.First();
            
            
            return View(setting);
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Edit()
        {
            Setting setting = _context.Settings.First();
            return View(setting);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Edit(Setting setting)
        {
            Setting existSetting = _context.Settings.First();
            if (existSetting == null)
            {
                return RedirectToAction("index");
            }
            if (setting.LogoFile != null)
            {
                if (setting.LogoFile.ContentType != "image/png" && setting.LogoFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("PhotoFile", "File type yanlisdir!");
                    return View();
                }

                if (setting.LogoFile.Length > (1024 * 1024) * 2)
                {
                    ModelState.AddModelError("PhotoFile", "Faly olcusu 2MB-dan cox ola bilmez!");
                    return View();
                }

                string rootPath = _env.WebRootPath;
                var filename = Guid.NewGuid().ToString() + setting.LogoFile.ContentType.Substring(setting.LogoFile.ContentType.IndexOf("/") + 1);
                var path = Path.Combine(rootPath, "uploads", filename);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    setting.LogoFile.CopyTo(stream);
                }

                existSetting.Logo = filename;

            }
            else if (setting.Logo == null)
            {
                if (existSetting.Logo != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads", existSetting.Logo);
                    if (System.IO.File.Exists(existPath))
                    {
                        System.IO.File.Delete(existPath);
                    }

                    existSetting.Logo = null;
                }
            }
            if (setting.ContactImageFile != null)
            {
                if (setting.ContactImageFile.ContentType != "image/png" && setting.ContactImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("PhotoFile", "File type yanlisdir!");
                    return View();
                }

                if (setting.ContactImageFile.Length > (1024 * 1024) * 2)
                {
                    ModelState.AddModelError("PhotoFile", "Faly olcusu 2MB-dan cox ola bilmez!");
                    return View();
                }

                string rootPath = _env.WebRootPath;
                var filename = Guid.NewGuid().ToString() + setting.LogoFile.ContentType.Substring(setting.LogoFile.ContentType.IndexOf("/") + 1);
                var path = Path.Combine(rootPath, "uploads", filename);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    setting.ContactImageFile.CopyTo(stream);
                }

                existSetting.ContactImage = filename;

            }
            else if (setting.ContactImage == null)
            {
                if (existSetting.ContactImage != null)
                {
                    string existPath = Path.Combine(_env.WebRootPath, "uploads", existSetting.ContactImage);
                    if (System.IO.File.Exists(existPath))
                    {
                        System.IO.File.Delete(existPath);
                    }

                    existSetting.ContactImage = null;
                }
            }
          
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Nese sehv getdi");
                return View();
            }

            existSetting.ContactAddress = setting.ContactAddress;
            existSetting.ContactEmailAddress = setting.ContactEmailAddress;
            existSetting.ContactPhone = setting.ContactPhone;
            existSetting.ContactTitle = setting.ContactTitle;
            existSetting.ContactMessage = setting.ContactMessage;
           
            
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
