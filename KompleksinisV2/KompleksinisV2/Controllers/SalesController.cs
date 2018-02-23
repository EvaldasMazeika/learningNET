using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KompleksinisV2.Data;
using KompleksinisV2.Models;
using KompleksinisV2.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KompleksinisV2.Controllers
{
    [Authorize(Roles ="Sales")]
    public class SalesController : Controller
    {
        private AppDbContext _context;

        public SalesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var text = _context.Messages.Where(x => x.DepartmentID == _context.Departments.Single(z => z.Name == "Pardavimai").ID)
                .OrderByDescending(x => x.WriteDate)
                .Include(c => c.Employee)
                .AsNoTracking();

            return View(await text.ToListAsync());
        }

        public async Task<IActionResult> Orders(string sortOrder, string searchString, string SearchMe)
        {
            PopulateStatesDropDown(SearchMe);

            ViewData["clientSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["stateSortParam"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["CurrentFilter"] = searchString;

            var orders = _context.Orders
                          .Include(c => c.Client)
                          .Include(x => x.State).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(s => s.Client.Name.Contains(searchString));

            }
            if (!String.IsNullOrEmpty(SearchMe))
            {
                orders = orders.Where(s => s.State.ID == (Int32.Parse(SearchMe)));

            }


            switch (sortOrder)
            {
                case "name_desc":
                    orders = orders.OrderByDescending(s => s.Client.Name);
                    break;
                case "Date":
                    orders = orders.OrderBy(s => s.State.Name);
                    break;
                case "date_desc":
                    orders = orders.OrderByDescending(s => s.State.Name);
                    break;
                default:
                    orders = orders.OrderBy(s => s.Client.Name);
                    break;
            }

            return View(await orders.AsNoTracking().ToListAsync());
        }

        private void PopulateStatesDropDown(object selected = null)
        {
            var query = from d in _context.States
                        orderby d.Name
                        select d;
            ViewBag.States = new SelectList(query.AsNoTracking(), "ID", "Name", selected);
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
            //if (ModelState.IsValid)
            //{
            //    order.CreateDate = DateTime.Now;
            //    order.EmployeeID = Int32.Parse(User.Identities.First(u => u.IsAuthenticated && u.HasClaim(c => c.Type == "UserID")).FindFirst("UserID").Value);
            //    order.StateID = _context.States.Single(i => i.Name == "Sukurta").ID;
            //    _context.Add(order);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Orders));
            //}

            //return View(order);
            return View();
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
            if (id == null)
            {
                return NotFound();
            }
            var ord = _context.Orders
                .Include(c => c.Client)
                .Include(c => c.State)
                .Include(c => c.Employee)
                .Include(c => c.OrderItems)
                .ThenInclude(c => c.Product)
                .SingleOrDefault(c => c.ID == id);

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
                return RedirectToAction(nameof(Details), new { id = parent });
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

        public async Task<IActionResult> MoveToStarted(int id)
        {
            var tmp = _context.OrderItems.Where(x => x.OrderID == id);
            Decimal totalPrice= 0, totalProfit =0;

            foreach (var item in tmp)
            {
                item.TotalPrice = item.Price * item.Quantity;
                item.TotalProfit = item.TotalPrice - (item.Quantity * _context.Products.Single(c => c.ID == item.ProductID).Price);
                
                totalPrice +=  item.TotalPrice ?? 0;
                totalProfit += item.TotalProfit ?? 0;
                _context.Update(item);
            }

            var items = (from o in _context.OrderItems
                         where o.OrderID == id
                         group o by o.ProductID into g
                         select new
                         {
                             k = g.Key,
                             v = g.Sum(x => x.Quantity),
                         }).ToDictionary(x => x.k, x => x.v);

            foreach (var item in items)
            {
                var temp = _context.Products.SingleOrDefault(x => x.ID == item.Key);
                var numbs = temp.Quantity;
                numbs = numbs - item.Value;
                temp.Quantity = numbs;
                _context.Update(temp);
            }

            var order = _context.Orders.SingleOrDefault(x => x.ID == id);
            order.StateID = _context.States.Single(x => x.Name == "Pradėta vykdyti").ID;
            order.TotalPrice = totalPrice;
            order.TotalProfit = totalProfit;
            order.StartedDate = DateTime.Now;

            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = id });
        }

        public async Task<IActionResult> MoveToFinish(int id)
        {
            var order = _context.Orders.SingleOrDefault(x => x.ID == id);
            order.StateID = _context.States.Single(x => x.Name == "Uždaryta").ID;
            order.FinishDate = DateTime.Now;

            _context.Update(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = id });
        }

        public async Task<decimal> GetProductPrice(int id)
        {
            var dem = await _context.Products.SingleOrDefaultAsync(x => x.ID == id);
            return dem.Price;
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
            if (ModelState.IsValid)
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
                return RedirectToAction(nameof(Clients));
            }
        }

        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.ID == id);

            if (order == null)
            {
                return RedirectToAction(nameof(Orders));
            }
            try
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Orders));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Orders));
            }

        }



        [HttpGet]
        public IActionResult Comments(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var _query = _context.Messages
                .Include(c => c.Comments)
                .SingleOrDefault(x => x.ID == id);

            return View(_query);
        }

        [HttpPost]
        public async Task<IActionResult> Comments([Bind("MessageID", "Comment")] Comments comments)
        {
            //if (ModelState.IsValid)
            //{
            //    int _id = Int32.Parse(User.Identities.First(u => u.IsAuthenticated && u.HasClaim(c => c.Type == "UserID")).FindFirst("UserID").Value);

            //    string firsName = _context.Employees.SingleOrDefault(x => x.ID == _id).Name;
            //    string secondName = _context.Employees.SingleOrDefault(x => x.ID == _id).Surname;

            //    comments.ComDate = DateTime.Now;
            //    comments.Fullname = firsName + " " + secondName;

            //    _context.Add(comments);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Comments));
            //}
            return View();
        }



        public IActionResult Reports()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TotalOrdersReport([Bind("EmployeeID", "BeginDate", "EndDate")] TotalOrdersViewModel totalOrdersViewModal)
        {
            //if(ModelState.IsValid)
            //{
            //    ViewData["employee"] = _context.Employees.Single(x => x.ID == totalOrdersViewModal.EmployeeID).FullName;
            //    ViewData["BeginDate"] = totalOrdersViewModal.BeginDate;
            //    ViewData["EndDate"] = totalOrdersViewModal.EndDate;
            //    var query = _context.Orders.Where(x => x.EmployeeID == totalOrdersViewModal.EmployeeID && (x.StateID == _context.States.Single(c => c.Name == "Uždaryta").ID) && (x.CreateDate >= totalOrdersViewModal.BeginDate && x.CreateDate <= totalOrdersViewModal.EndDate));
            //    return View(query.ToList());
            //}
            return RedirectToAction(nameof(Reports));
        }

        [HttpPost]
        public async Task<IActionResult> AllOrdersReport([Bind("BeginDate", "EndDate")] AllOrdersViewModel allOrdersViewModel)
        {
            //if(ModelState.IsValid)
            //{
            //    ViewData["BeginDate"] = allOrdersViewModel.BeginDate;
            //    ViewData["EndDate"] = allOrdersViewModel.EndDate;

            //    var items = await _context.Orders.Where(x => x.StateID == _context.States.Single(c => c.Name == "Uždaryta").ID && (x.Employee.DepartmentID == _context.Departments.Single(c => c.Name == "Pardavimai").ID) && (x.CreateDate >= allOrdersViewModel.BeginDate && x.CreateDate <= allOrdersViewModel.EndDate)).ToListAsync();

            //    var emps = items.Select(z => z.EmployeeID).Distinct();

            //    var query = new List<AllOrdersResultViewModel>();

            //    foreach (var item in emps)
            //    {
            //        var temp = new AllOrdersResultViewModel
            //        {
            //            EmployeeID = item,
            //            FullName = _context.Employees.Single(x => x.ID == item).FullName,
            //            OrdersNumber = items.Where(x => x.EmployeeID == item).Count(),
            //            TotalPrice = items.Where(x => x.EmployeeID == item).Select(a => a.TotalPrice).Sum() ?? 0,
            //            TotalProfit = items.Where(x => x.EmployeeID == item).Select(a => a.TotalProfit).Sum() ?? 0
            //        };
            //        query.Add(temp);
            //    }

            //    return View(query);
            //}
            return RedirectToAction(nameof(Reports));

        }


    }
}