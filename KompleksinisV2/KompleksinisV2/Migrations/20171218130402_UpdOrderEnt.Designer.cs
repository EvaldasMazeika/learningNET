﻿// <auto-generated />
using KompleksinisV2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace KompleksinisV2.Migrations
{
    [DbContext(typeof(Data.AppDbContext))]
    [Migration("20171218130402_UpdOrderEnt")]
    partial class UpdOrderEnt
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KompleksinisV2.Models.Client", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Adress")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("PhoneNum")
                        .IsRequired();

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("ID");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("KompleksinisV2.Models.Comments", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ComDate");

                    b.Property<string>("Comment")
                        .IsRequired();

                    b.Property<string>("Fullname");

                    b.Property<int>("MessageID");

                    b.HasKey("ID");

                    b.HasIndex("MessageID");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("KompleksinisV2.Models.Employee", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("BirthDate");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("MobileNumber")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<int>("PositionID");

                    b.Property<int>("SectorID");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.HasKey("ID");

                    b.HasIndex("PositionID");

                    b.HasIndex("SectorID");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("KompleksinisV2.Models.Message", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("EmployeeID");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<DateTime>("WriteDate");

                    b.Property<string>("WrittenText")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("EmployeeID");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("KompleksinisV2.Models.Order", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ClientID");

                    b.Property<DateTime>("CreateDate");

                    b.Property<int>("EmployeeID");

                    b.Property<DateTime?>("FinishDate");

                    b.Property<string>("Notes")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("StartedDate");

                    b.Property<int>("StateID");

                    b.HasKey("ID");

                    b.HasIndex("ClientID");

                    b.HasIndex("EmployeeID");

                    b.HasIndex("StateID");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("KompleksinisV2.Models.OrderItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("OrderID");

                    b.Property<decimal>("Price");

                    b.Property<int>("ProductID");

                    b.Property<decimal>("Quantity");

                    b.HasKey("ID");

                    b.HasIndex("OrderID");

                    b.HasIndex("ProductID");

                    b.ToTable("OrderItem");
                });

            modelBuilder.Entity("KompleksinisV2.Models.Position", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Position");
                });

            modelBuilder.Entity("KompleksinisV2.Models.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<decimal>("Price");

                    b.Property<int>("ProductGroupID");

                    b.Property<decimal>("Quantity");

                    b.HasKey("ID");

                    b.HasIndex("ProductGroupID");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("KompleksinisV2.Models.ProductGroup", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("ProductGroup");
                });

            modelBuilder.Entity("KompleksinisV2.Models.Sector", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Sector");
                });

            modelBuilder.Entity("KompleksinisV2.Models.State", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("State");
                });

            modelBuilder.Entity("KompleksinisV2.Models.Comments", b =>
                {
                    b.HasOne("KompleksinisV2.Models.Message", "Message")
                        .WithMany("Comments")
                        .HasForeignKey("MessageID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KompleksinisV2.Models.Employee", b =>
                {
                    b.HasOne("KompleksinisV2.Models.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KompleksinisV2.Models.Sector", "Sector")
                        .WithMany()
                        .HasForeignKey("SectorID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KompleksinisV2.Models.Message", b =>
                {
                    b.HasOne("KompleksinisV2.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID");
                });

            modelBuilder.Entity("KompleksinisV2.Models.Order", b =>
                {
                    b.HasOne("KompleksinisV2.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KompleksinisV2.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KompleksinisV2.Models.State", "State")
                        .WithMany()
                        .HasForeignKey("StateID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KompleksinisV2.Models.OrderItem", b =>
                {
                    b.HasOne("KompleksinisV2.Models.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KompleksinisV2.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KompleksinisV2.Models.Product", b =>
                {
                    b.HasOne("KompleksinisV2.Models.ProductGroup", "ProductGroup")
                        .WithMany()
                        .HasForeignKey("ProductGroupID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
