using KompleksinisV2.Models;
using Microsoft.EntityFrameworkCore;

namespace KompleksinisV2.Data
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions<TestContext> options) :base(options)
        {}
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<Message>().ToTable("Message");
            modelBuilder.Entity<Comments>().ToTable("Comments");
            modelBuilder.Entity<Client>().ToTable("Clients");
            modelBuilder.Entity<ProductGroup>().ToTable("ProductGroup");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<State>().ToTable("State");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItem");
        }

    }
}
