using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KompleksinisV2.Models;
using Microsoft.EntityFrameworkCore;

namespace KompleksinisV2.Data
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions<TestContext> options) :base(options)
        {}

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Mark>().ToTable("Mark");
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<Message>().ToTable("Message");
        }

    }
}
