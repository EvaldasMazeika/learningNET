using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KompleksinisV2.Data;
using KompleksinisV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KompleksinisV2.ViewComponents
{
    [ViewComponent(Name = "AddProduct")]
    public class AddProductViewComponent : ViewComponent
    {
        private TestContext _context;

        public AddProductViewComponent(TestContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int orderID)
        {
            var model = new OrderItem
            {
                OrderID = orderID
            };
            PopulateProductsDropDown();
            return View(model);
        }

        private void PopulateProductsDropDown(object selected = null)
        {
            var query = from d in _context.Products
                        orderby d.Name
                        select d;

            ViewBag.ProductID = new SelectList(query.AsNoTracking(), "ID", "Name", selected);
        }
    }
}
