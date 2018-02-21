using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KompleksinisV2.Controllers;
using KompleksinisV2.Data;
using KompleksinisV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Kompleksinis.XUnitTests
{
    public class DashboardControllerTests
    {
        private DashboardController _controller;
        private KompleksinisV2.Data.AppDbContext _context;
        [Fact]
        public async Task Test1()
        {
            /*var builder = new DbContextOptionsBuilder<TestContext>().UseInMemoryDatabase();

            var _context = new TestContext(builder.Options);

            _context.Messages.Add(new Message()
            {
                Title = "John",
                WrittenText = "Dofsdfsdfsdfe",
                WriteDate = DateTime.Now,
                DepartmentID = 1
            });
            _context.SaveChanges();

            _controller = new DashboardController(_context);

            var result = await _controller.Index();
            var model = result.

            Assert.IsType<ViewResult>(result);*/
        }
        [Fact]
        public void PassTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }
    }
}
