using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KompleksinisV2.Data;
using KompleksinisV2.Models;
using KompleksinisV2.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KompleksinisV2.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        // private readonly KompleksinisDBContext _context;
        private readonly AppDbContext _context;
        private readonly RoleManager<AppIdentityRole> _roleManager;
        private readonly UserManager<AppIdentityUser> _userManager;

        public DashboardController(AppDbContext context, RoleManager<AppIdentityRole> roleManager,
            UserManager<AppIdentityUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var text = _context.Messages.Where(x=>x.DepartmentID == _context.Departments.Single(z=>z.Name == "Administracija").ID)
                .OrderByDescending(x=> x.WriteDate)
                .Include(c => c.Employee)
                .AsNoTracking();

            return View(await text.ToListAsync());
        }

        public IActionResult Employees()
        {
            var model = new List<EmployeeListViewModel>();
            model = _userManager.Users.Select( u => new EmployeeListViewModel
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Role = "what?"  // TODO: ADD NORMBAL ROLE STRING
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult NewEmployee()
        {
            PopulateDepartmentDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewEmployee(NewEmployeeViewModel employee)
        {

            if (ModelState.IsValid)
            {
                var user = new AppIdentityUser
                {
                    UserName = employee.UserName,
                    Name = employee.Name,
                    Surname = employee.Surname,
                    Email = employee.Email,
                    BirthDate = employee.BirthDate,
                    PhoneNumber = employee.MobileNumber
                };

                IdentityResult result = await _userManager.CreateAsync(user, employee.Password);
                if (result.Succeeded)
                {
                    AppIdentityRole appIdentityRole = await _roleManager.FindByIdAsync(employee.DepartmentId.ToString());
                    if (appIdentityRole != null)
                    {
                        IdentityResult roleResult = await _userManager.AddToRoleAsync(user, appIdentityRole.Name);
                        if (roleResult.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }

            }
            PopulateDepartmentDropDown(employee.DepartmentId);
            return View(employee);
          /*  try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Employees));
                }
            }
            catch (Exception)
            {throw;}

            
            PopulateDepartmentDropDown(employee.DepartmentId);
            return View(employee);*/
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var query = await _context.Employees
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (query == null)
            {
                return NotFound();
            }
            
            PopulateDepartmentDropDown(query.DepartmentID);
            return View(query);
        }

        [HttpPost, ActionName("EditEmployee")]
        public async Task<IActionResult> EditEmp(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var whatToUpdat = await _context.Employees.SingleOrDefaultAsync(s => s.ID == id);

            if (await TryUpdateModelAsync<Employee>(
                whatToUpdat, "", i=> i.Name, i=> i.Surname, i=> i.Email, i=> i.Password, i=> i.BirthDate, i=> i.MobileNumber, i=> i.DepartmentID))

            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Employees));
                }
                catch (DbUpdateException)
                {
                }
            }
            return View(whatToUpdat);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var emp = await _context.Employees
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);
            if (emp == null)
            {
                return RedirectToAction(nameof(Employees));
            }

            try
            {
                _context.Employees.Remove(emp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Employees));
            }
            catch (DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Employees));
            }
        }



        private void PopulateDepartmentDropDown(object selected = null)
        {
            var query = from d in _roleManager.Roles
                        orderby d.Name
                        select d;
            ViewBag.DepartmentId = new SelectList(query.AsNoTracking(), "Id", "Name",selected);
        }

        [HttpGet]
        public IActionResult Comments(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var _query = _context.Messages
                .Include(c=> c.Comments)
                .SingleOrDefault(x => x.ID == id);

            return View(_query);
        }

        [HttpPost]
        public async Task<IActionResult> Comments([Bind("MessageID", "Comment")] Comments comments)
        {
            if (ModelState.IsValid)
            {
                int _id = Int32.Parse(User.Identities.First(u => u.IsAuthenticated && u.HasClaim(c => c.Type == "UserID")).FindFirst("UserID").Value);

                string firsName = _context.Employees.SingleOrDefault(x => x.ID == _id).Name;
                string secondName = _context.Employees.SingleOrDefault(x => x.ID == _id).Surname;

                comments.ComDate = DateTime.Now;
                comments.Fullname = firsName + " " + secondName;

                _context.Add(comments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Comments));
            }
            return View();
        }



        [HttpGet]
        public IActionResult Message()
        {
            PopulateDepartmentDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Message([Bind("Title", "WrittenText", "DepartmentID")] Message message)
        {
            Employee emp = _context.Employees.Single(x => x.ID == Int32.Parse(User.Identities.First(u => u.IsAuthenticated && u.HasClaim(c => c.Type == "UserID")).FindFirst("UserID").Value) );
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

        [Authorize(Roles ="Administrator")]
        public IActionResult Roles()
        {
            List<RoleListViewModel> model = new List<RoleListViewModel>();
            model = _roleManager.Roles.Select(r => new RoleListViewModel
            {
                RoleName = r.Name,
                Id = r.Id,
                Description = r.Description
            }).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult NewRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewRole(RoleListViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppIdentityRole applicationRole = new AppIdentityRole { DateCreated = DateTime.Now };

                applicationRole.Name = model.RoleName;
                applicationRole.Description = model.Description;

                IdentityResult roleRuslt = await _roleManager.CreateAsync(applicationRole);

                if (roleRuslt.Succeeded)
                {
                    return RedirectToAction("Roles");
                }

            }
            return View(model);
        }


        public IActionResult Error()
        {
            return View();
        }

    }
}