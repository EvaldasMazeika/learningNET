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
            PopulateRolesDropDown();
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
            PopulateRolesDropDown(employee.DepartmentId);
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(Guid id)
        {
            if (!String.IsNullOrEmpty(id.ToString()))
            {
                AppIdentityUser user = await _userManager.FindByIdAsync(id.ToString());

                if (user != null)
                {
                    var wa = _userManager.GetRolesAsync(user).Result.Single();

                    var model = new NewEmployeeViewModel
                    {
                        Email = user.Email,
                        Name = user.Name,
                        UserName = user.UserName,
                        Surname = user.Surname,
                        BirthDate = user.BirthDate,
                        MobileNumber = user.PhoneNumber,
                        DepartmentId = _roleManager.Roles.Single(r => r.Name == wa).Id
                    };


                    PopulateRolesDropDown(model.DepartmentId);
                    return View(model);
                }
                return RedirectToAction("Employees");
            }
            return RedirectToAction("Employees");
        }

        [HttpPost, ActionName("EditEmployee")]
        public async Task<IActionResult> EditEmployee(Guid id, NewEmployeeViewModel model)
        {
            if(!String.IsNullOrEmpty(id.ToString()))
            {
                AppIdentityUser user = await _userManager.FindByIdAsync(id.ToString());
                if(user != null)
                {
                    var existingRole = _userManager.GetRolesAsync(user).Result.Single();
                    var roleId = _roleManager.Roles.Single(r => r.Name == existingRole).Id;

                    user.Name = model.Name;
                    user.Surname = model.Surname;
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.BirthDate = model.BirthDate;
                    user.PhoneNumber = model.MobileNumber;

                    IdentityResult result = await _userManager.UpdateAsync(user);

                    if(result.Succeeded)
                    {
                        if (roleId != model.DepartmentId)
                        {
                            IdentityResult resultt = await _userManager.RemoveFromRoleAsync(user, existingRole);
                            if (resultt.Succeeded)
                            {
                                AppIdentityRole role = await _roleManager.FindByIdAsync(model.DepartmentId.ToString());
                                if (role != null)
                                {
                                    IdentityResult newRoleResult = await _userManager.AddToRoleAsync(user, role.Name);
                                    if (newRoleResult.Succeeded)
                                    {
                                        return RedirectToAction("Employees");
                                    }
                                }
                            }
                        }
                    }

                }
                return RedirectToAction("Employees");
            }
            return RedirectToAction("Employees");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            if(!String.IsNullOrEmpty(id.ToString()))
            {
                AppIdentityUser user = await _userManager.FindByIdAsync(id.ToString());
                if (user != null)
                {
                    IdentityResult result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Employees");
                    }
                }
                return RedirectToAction("Employees");
            }
            return RedirectToAction("Employees");
        }

        private void PopulateDepartmentsDropDown(object selected = null)
        {
            var query = from d in _context.Departments
                        orderby d.Name
                        select d;
            ViewBag.DepartmentId = new SelectList(query.AsNoTracking(), "ID", "Name", selected);
        }

        private void PopulateRolesDropDown(object selected = null)
        {
            var query = from d in _roleManager.Roles
                        orderby d.Name
                        select d;
            ViewBag.RoleId = new SelectList(query.AsNoTracking(), "Id", "Name",selected);
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
                AppIdentityUser user = await _userManager.GetUserAsync(HttpContext.User);

                var _id = user.Id;

                comments.ComDate = DateTime.Now;
                comments.Fullname = user.FullName;
               
                _context.Add(comments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Comments));
            }
            return View();
        }



        [HttpGet]
        public IActionResult Message()
        {
            PopulateDepartmentsDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Message(Message message)
        {
            AppIdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
            try
            {
                if(ModelState.IsValid)
                {
                    message.Employee = user;
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
        public async Task<IActionResult> NewRole(NewRoleViewModel model)
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

        [HttpPost]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            if(!String.IsNullOrEmpty(id.ToString()))
            {
                AppIdentityRole appIdentityRole = await _roleManager.FindByIdAsync(id.ToString()); 
                if (appIdentityRole != null)
                {
                    IdentityResult result = _roleManager.DeleteAsync(appIdentityRole).Result;

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Roles));
                    }
                    return RedirectToAction(nameof(Roles));
                }
            }
            return RedirectToAction(nameof(Roles));
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(Guid id)
        {
            if(!String.IsNullOrEmpty(id.ToString()))
            {
                AppIdentityRole appIdentityRole = await _roleManager.FindByIdAsync(id.ToString());
                if (appIdentityRole != null)
                {
                    NewRoleViewModel model = new NewRoleViewModel
                    {
                        RoleName = appIdentityRole.Name,
                        Description = appIdentityRole.Description
                    };
                    return View(model);
                }
                return NotFound();
            }

            return BadRequest();
        }

        public async Task<IActionResult> EditRole(Guid id, NewRoleViewModel model)
        {
            if(ModelState.IsValid)
            {
                AppIdentityRole role = await _roleManager.FindByIdAsync(id.ToString());

                role.Name = model.RoleName;
                role.Description = model.Description;

                IdentityResult result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
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