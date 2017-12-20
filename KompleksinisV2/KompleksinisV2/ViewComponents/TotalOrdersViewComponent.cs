using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KompleksinisV2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KompleksinisV2.ViewComponents
{
    [ViewComponent(Name = "TotalOrders")]
    public class TotalOrdersViewComponent : ViewComponent
    {
        private TestContext _context;

        public TotalOrdersViewComponent(TestContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            PopulateEmployeeDropDown();
            return View();
        }
        private void PopulateEmployeeDropDown(object selected = null)
        {
            var query = (from d in _context.Employees
                        orderby d.Name
                        select d).Where(x => x.DepartmentID == _context.Departments.Single(c=>c.Name == "Pardavimai").ID);

            ViewBag.EmployeeID = new SelectList(query.AsNoTracking(), "ID", "FullName", selected);
        }
    }
}
