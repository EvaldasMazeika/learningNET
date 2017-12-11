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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KompleksinisV2.Controllers
{
    public class AccountController : Controller
    {
        private readonly TestContext _context;

        public AccountController(TestContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Employee employee)
        {

            if (LoginUser(employee.Email, employee.Password))
            {
                employee.Name = _context.Employees.Single(x => x.Email == employee.Email).Name;
                employee.ID = _context.Employees.Single(x => x.Email == employee.Email).ID;
              //  employee.Name =  _context.Employees.Where(x => x.Email == employee.Email).Select(x => x.Name).ToString();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, employee.Name), 
                    new Claim("UserID",String.Join("",employee.ID))
                };

                var userIdentity = new ClaimsIdentity(claims, "Login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
                return Redirect("/");
            }

            return View();
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
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login","Account");
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


        /* public async Task<IActionResult> Login(string returnUrl = null)
         {
             const string Issuer = "https://consoto.com";
             var claims = new List<Claim>();
             claims.Add(new Claim(ClaimTypes.Name, "barry", ClaimValueTypes.String, Issuer));
             claims.Add(new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String, Issuer));
             var userIdentity = new ClaimsIdentity("SuperSecureLogin");
             userIdentity.AddClaims(claims);
             var userPrincipal = new ClaimsPrincipal(userIdentity);

             await HttpContext.SignInAsync(
                 CookieAuthenticationDefaults.AuthenticationScheme,
                 userPrincipal,
                 new AuthenticationProperties
                 {
                     ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                     IsPersistent = false,
                     AllowRefresh = false
                 });

             return RedirectToLocal(returnUrl);
         }*/

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

        public IActionResult Forbidden()
        {
            return View();
        }
    }
}