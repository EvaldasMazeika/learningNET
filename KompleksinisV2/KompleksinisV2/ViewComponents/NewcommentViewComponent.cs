using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KompleksinisV2.Models;
using Microsoft.AspNetCore.Mvc;

namespace KompleksinisV2.ViewComponents
{
    [ViewComponent(Name = "Newcomment")]
    public class NewcommentViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int messageID)
        {
            var model = new Comments
            {
                MessageID = messageID
            };
           
            return View(model);
        }
    }
}
