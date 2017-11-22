using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KompleksinisV2.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KompleksinisV2.Controllers
{
    public class DashboardController : Controller
    {
        private readonly KompleksinisDBContext _context;

        public DashboardController(KompleksinisDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.TestT.ToListAsync());
        }
    }
}