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
        public DbSet<Position> Positions { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Comments> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>().ToTable("Position");
            modelBuilder.Entity<Sector>().ToTable("Sector");
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<Message>().ToTable("Message");
            modelBuilder.Entity<Comments>().ToTable("Comments");
        }

    }
}
