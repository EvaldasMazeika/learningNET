using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KompleksinisV2.Data;
using KompleksinisV2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

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
              //  employee.Name =  _context.Employees.Where(x => x.Email == employee.Email).Select(x => x.Name).ToString();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, employee.Name) 
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