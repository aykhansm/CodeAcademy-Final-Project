using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shitfo.Areas.Manage.ViewModels;
using shitfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginModel adminLoginModel)
        {
            AppUser adminUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == adminLoginModel.UserName && x.IsAdmin);

            if (adminUser == null)
            {
                ModelState.AddModelError("", "UserName or Password is incorrect");
                return View(adminLoginModel);
            }

            var result = await _signInManager.PasswordSignInAsync(adminUser, adminLoginModel.Password, true, true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "UserName or Password is incorrect");
                return View(adminLoginModel);
            }

            return RedirectToAction("index", "dashboard");

        }
        [Authorize(Roles = "Superadmin,Admin")]
        public IActionResult Edit()
        {

            AppUser user = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            return View(user);
        }

        [Authorize(Roles = "Superadmin,Admin")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login");
        }
    }
}
