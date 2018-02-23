using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KompleksinisV2.Data;
using KompleksinisV2.Models;
using KompleksinisV2.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KompleksinisV2.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly RoleManager<AppIdentityRole> _roleManager;

        public AccountController(AppDbContext context,
            SignInManager<AppIdentityUser> signInManager,
            UserManager<AppIdentityUser> userManager,
            RoleManager<AppIdentityRole> roleManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password,model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Login Failed");
            return View(model);

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            Guid _id = user.Id;

            EditMySelfViewModel temp = new EditMySelfViewModel
            {
                Email = user.Email,
                Surname = user.Surname,
                MobileNumber = user.PhoneNumber
            };
            return View(temp);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMySelfViewModel emp)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            Guid _id = user.Id;

            if (ModelState.IsValid)
            {
                user.Surname = emp.Surname;
                user.Email = emp.Email;
                user.PhoneNumber = emp.MobileNumber;

                IdentityResult result = await _userManager.UpdateAsync(user);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Dashboard");
                }  
            }
            return View(emp);
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                if (changePasswordViewModel.NewPassword == changePasswordViewModel.RepeatPassword)
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    Guid _id = user.Id;

                    IdentityResult result = await _userManager.ChangePasswordAsync(user, changePasswordViewModel.CurrentPassword, changePasswordViewModel.NewPassword);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
            }
            return View(changePasswordViewModel);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}