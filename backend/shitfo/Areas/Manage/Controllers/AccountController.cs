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
            EditViewModel editView = new EditViewModel() { 
            FullName=user.FullName,
            UserName=user.UserName,
            Password=user.Password
            
            };
            return View(editView);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Superadmin,Admin")]
        public async Task<IActionResult> Edit(EditViewModel user)
        {
            AppUser loginedUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (_userManager.Users.Any(x => x.UserName == user.UserName && x.Id != loginedUser.Id))
            {
                ModelState.AddModelError("UserName", "UserName already taken!");
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }
           
                loginedUser.UserName = user.UserName;
            
                loginedUser.FullName = user.FullName;



            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                if (string.IsNullOrWhiteSpace(user.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "CurrentPassword can not be emtpy");
                    return View();
                }

                var result = await _userManager.ChangePasswordAsync(loginedUser, user.CurrentPassword, user.Password);
                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                    return View();
                }
            }
            await _userManager.UpdateAsync(loginedUser);

            await _signInManager.SignInAsync(loginedUser, true);
            return RedirectToAction("edit");
        }
        [Authorize(Roles = "Superadmin,Admin")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login");
        }
    }
}
