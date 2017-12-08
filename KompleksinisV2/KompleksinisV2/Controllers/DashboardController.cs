using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KompleksinisV2.Data;
using KompleksinisV2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KompleksinisV2.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        // private readonly KompleksinisDBContext _context;
        private readonly TestContext _context;

        public DashboardController(TestContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var text = _context.Messages
                .Include(c => c.Employee)
                .AsNoTracking();

            return View(await text.ToListAsync());
        }

        [HttpGet]
        public IActionResult Message()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Message([Bind("WrittenText")] Message message)
        {
            Employee emp = _context.Employees.Single(x => x.Username == User.Identities.First(
      u => u.IsAuthenticated &&
      u.HasClaim(c => c.Type == ClaimTypes.Name)).FindFirst(ClaimTypes.Name).Value);
            try
            {
                if(ModelState.IsValid)
                {
                    message.Employee = emp;
                    message.WriteDate = DateTime.Now;
                    _context.Add(message);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {

                throw;
            }

            return View(message);
        }
    }
}