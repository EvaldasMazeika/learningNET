﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class DashboardController : Controller
    {
        // private readonly KompleksinisDBContext _context;
        private readonly TestContext _context;

        public DashboardController(TestContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var text = _context.Messages
                .OrderByDescending(x=> x.WriteDate)
                .Include(c => c.Employee)
                .AsNoTracking();

            return View(await text.ToListAsync());
        }

        public IActionResult Employees()
        {
            var _list = _context.Employees.Include(c => c.Position).Include(x => x.Sector).AsNoTracking();
            return View(_list.ToList());
        }

        public IActionResult Reports()
        {
            return View();
        }

        [HttpGet]
        public IActionResult NewEmployee()
        {
            PopulatePositionDropDown();
            PopulateSectorDropDown();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewEmployee(Employee employee)
        {
            try
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

            PopulatePositionDropDown(employee.PositionID);
            PopulateSectorDropDown(employee.SectorID);
            return View(employee);
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
            PopulatePositionDropDown(query.PositionID);
            PopulateSectorDropDown(query.SectorID);
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
                whatToUpdat, "", i=> i.Name, i=> i.Surname, i=> i.Email, i=> i.Password, i=> i.BirthDate, i=> i.MobileNumber, i=> i.PositionID, i=> i.SectorID))

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
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Employees));
            }
        }



        private void PopulateSectorDropDown(object selected = null)
        {
            var query = from d in _context.Sectors
                        orderby d.Name
                        select d;
            ViewBag.SectorID = new SelectList(query.AsNoTracking(), "ID", "Name",selected);
        }

        private void PopulatePositionDropDown(object selected = null)
        {
            var query = from d in _context.Positions
                        orderby d.Name
                        select d;

            ViewBag.PositionID = new SelectList(query.AsNoTracking(), "ID", "Name",selected);
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
                .SingleOrDefault(x => x.ID == id);// TODO: PABAIGTI SITA REIKALA SU KOMENTARAIS


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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Message([Bind("Title", "WrittenText")] Message message)
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
    }
}