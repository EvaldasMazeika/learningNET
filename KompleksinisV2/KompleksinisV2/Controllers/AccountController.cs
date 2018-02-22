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

        private bool LoginUser(string email, string password)
        {
            var lol = _context.Employees.Where(x => x.Email == email && x.Password == password);
            
            if (lol.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
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
        public IActionResult Edit()
        {
            int _id = Int32.Parse(User.Identities.First(u => u.IsAuthenticated && u.HasClaim(c => c.Type == "UserID")).FindFirst("UserID").Value);
            var empToUpdate = _context.Employees.SingleOrDefault(x => x.ID == _id);

            EditMySelfViewModel _temp = new EditMySelfViewModel
            {
                Email = empToUpdate.Email,
                Surname = empToUpdate.Surname,
                MobileNumber = empToUpdate.MobileNumber
            };

            return View(_temp);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditMySelfViewModel emp)
        {
            int _id = Int32.Parse(User.Identities.First(u => u.IsAuthenticated && u.HasClaim(c => c.Type == "UserID")).FindFirst("UserID").Value);
            var empToUpdate = await _context.Employees.SingleOrDefaultAsync(x => x.ID == _id);

            if (ModelState.IsValid)
            {
                empToUpdate.Surname = emp.Surname;
                empToUpdate.Email = emp.Email;
                empToUpdate.MobileNumber = emp.MobileNumber;

                _context.Update(empToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Dashboard");
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
                    int _id = Int32.Parse(User.Identities.First(u => u.IsAuthenticated && u.HasClaim(c => c.Type == "UserID")).FindFirst("UserID").Value);
                    var empToUpdate = _context.Employees.SingleOrDefault(x => x.ID == _id);

                    empToUpdate.Password = changePasswordViewModel.NewPassword;

                    _context.Update(empToUpdate);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Dashboard");

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