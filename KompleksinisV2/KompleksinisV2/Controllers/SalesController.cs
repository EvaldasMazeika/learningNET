using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KompleksinisV2.Data;
using KompleksinisV2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KompleksinisV2.Controllers
{
    [Authorize]
    public class SalesController : Controller
    {
        private TestContext _context;

        public object DataTime { get; private set; }

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
                  .Include(x => x.State)
                  .AsNoTracking();

              return View(await ords.ToListAsync());
           
        }

        [HttpGet]
        public IActionResult NewOrder()
        {
            PopulateClientsDropDown();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewOrder([Bind("ClientID", "Notes")]Order order)
        {
            if (ModelState.IsValid)
            {
                order.CreateDate = DateTime.Now;
                order.EmployeeID = Int32.Parse(User.Identities.First(u => u.IsAuthenticated && u.HasClaim(c => c.Type == "UserID")).FindFirst("UserID").Value);
                order.StateID = _context.States.Single(i => i.Name == "Sukurta").ID;
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Orders));
            }

            return View(order);
        }

        private void PopulateClientsDropDown(object selected = null)
        {
            var query = from d in _context.Clients
                        orderby d.Name
                        select d;
            ViewBag.ClientID = new SelectList(query.AsNoTracking(), "ID", "FullName", selected);
        }
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var ord = _context.Orders
                .Include(c => c.Client)
                .Include(c => c.State)
                .Include(c => c.Employee)
                .Include(c => c.OrderItems)
                .ThenInclude(c=>c.Product)
                .SingleOrDefault(c=>c.ID == id);

            if (ord == null)
            {
                return null;
            }

            return View(ord);
        }

        [HttpPost]
        public async Task<IActionResult> Details([Bind("OrderID", "ProductID", "Quantity", "Price")] OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details));
            }
            
            return RedirectToAction(nameof(Details));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProductFromOrder(int id, int parent)
        {
            var prod = await _context.OrderItems
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (prod == null)
            {
                return RedirectToAction(nameof(Details));
            }

            try
            {
                _context.OrderItems.Remove(prod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new {  id = parent});
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Details));
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> MoveToWaiting(int id)
        {
            var order = _context.Orders.SingleOrDefault(x => x.ID == id);
            order.StateID = _context.States.Single(x => x.Name == "Laukiama").ID;

            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = id });
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