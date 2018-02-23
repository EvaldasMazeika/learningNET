using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KompleksinisV2.Data;
using KompleksinisV2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KompleksinisV2.ViewComponents
{
    [ViewComponent(Name = "TotalOrders")]
    public class TotalOrdersViewComponent : ViewComponent
    {
        private AppDbContext _context;
        private readonly UserManager<AppIdentityUser> _userManager;

        public TotalOrdersViewComponent(AppDbContext context, UserManager<AppIdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            PopulateEmployeeDropDown();
            return View();
        }
        private void PopulateEmployeeDropDown(object selected = null)
        {
            //TODO: SHOW ONLY EMPLOYEES WHO WORKS IN SALES, NOT ALL (reik keisti modeli, kad priskirtu skyriu, o ne teises)

            var query = (from d in _userManager.Users
                         orderby d.Name
                         select d);

            ViewBag.EmployeeID = new SelectList(query.AsNoTracking(), "Id", "FullName", selected);
        }
    }
}
