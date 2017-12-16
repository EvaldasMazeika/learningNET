using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KompleksinisV2.Data;
using KompleksinisV2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KompleksinisV2.Controllers
{
    [Authorize]
    public class SalesController : Controller
    {
        private TestContext _context;

        public SalesController(TestContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Orders()
        {
            var ords = _context.Orders
                .Include(c => c.Client)
                .Include(c => c.Product)
                .AsNoTracking();

            return View(await ords.ToListAsync());
        }

        public IActionResult NewOrder()
        {
            return View();
        }




        public async Task<IActionResult> Clients(string sortOrder, string searchString)
        {
            ViewData["mailSortParam"] = String.IsNullOrEmpty(sortOrder) ? "mail_desc" : "";
            ViewData["CurrentFilter"] = searchString;

            var clients = from s in _context.Clients
                          select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(s => s.Email.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "mail_desc":
                    clients = clients.OrderByDescending(s => s.Email);
                    break;
                default:
                    clients = clients.OrderBy(s => s.Email);
                    break;
            }

            return View(await clients.AsNoTracking().ToListAsync());
        }

        [HttpGet]
        public IActionResult NewClient()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewClient(Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Clients));
            }
            return View(client);
        }

        [HttpGet]
        public async Task<IActionResult> EditClient(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cltToEdit = await _context.Clients
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (cltToEdit == null)
            {
                return NotFound();
            }

            return View(cltToEdit);
        }

        public async Task<IActionResult> EditClient(int id, [Bind("ID,Name,Surname,Email,PhoneNum,Adress")] Client client)
        {
            if (id != client.ID)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                _context.Update(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Clients));
            }
            return View(client);
        }


        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (client == null)
            {
                return RedirectToAction(nameof(Clients));
            }

            try
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Clients));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(client));
            }
        }

        public IActionResult Reports()
        {
            return View();
        }

    }
}