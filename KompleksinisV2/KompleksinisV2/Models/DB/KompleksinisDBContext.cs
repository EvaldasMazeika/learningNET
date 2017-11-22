using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KompleksinisV2.Models.DB
{
    public partial class KompleksinisDBContext : DbContext
    {
        public virtual DbSet<TestT> TestT { get; set; }


        public KompleksinisDBContext(DbContextOptions<KompleksinisDBContext> options) : base(options)
        {}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestT>(entity =>
            {
                entity.HasKey(e => e.C1);

                entity.Property(e => e.C1)
                    .HasColumnName("c1")
                    .ValueGeneratedNever();

                entity.Property(e => e.C2)
                    .HasColumnName("c2")
                    .HasColumnType("nchar(10)");
            });
        }
    }
}
