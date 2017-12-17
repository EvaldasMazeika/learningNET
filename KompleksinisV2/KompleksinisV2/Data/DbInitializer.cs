using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KompleksinisV2.Models;

namespace KompleksinisV2.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TestContext context)
        { // seeding local db
            context.Database.EnsureCreated();

            if(context.Employees.Any()) // jeigu kazkas yra, nieko nedarom
            {
                return;
            }

            var positions = new Position[]
            {
                new Position{Name="Vadovas"},
                new Position{Name="Supirkėjas"}
            };
            foreach (var pos in positions)
            {
                context.Positions.Add(pos);
            }
            context.SaveChanges();

            var sectors = new Sector[]
            {
                new Sector{Name="Supirkimas"},
                new Sector{Name="Administracija"}
            };
            foreach (var sec in sectors)
            {
                context.Sectors.Add(sec);
            }
            context.SaveChanges();


            var employees = new Employee[]
            {
                new Employee{Name="Evaldas",Surname="Jonuos",Email="i@i.i",Password="evas",BirthDate=new DateTime(2017,01,01),MobileNumber="+37060000000",PositionID=positions.Single( i => i.Name == "Vadovas").ID,SectorID=sectors.Single( i => i.Name == "Administracija").ID},
                new Employee{Name="Jonas",Surname="Enoas",Email="a@a.a",Password="jonas",BirthDate=new DateTime(2010,11,11),MobileNumber="+37099900009",PositionID=positions.Single( i => i.Name == "Supirkėjas").ID,SectorID=sectors.Single( i => i.Name == "Supirkimas").ID},
            };
            foreach (Employee emp in employees)
            {
                context.Employees.Add(emp);
            }
            context.SaveChanges();

            var clients = new Client[]
            {
                new Client{Name="Petras",Surname="Petrauskas", Email="za@z.z", PhoneNum="+37055555555", Adress="Vilniaus g. 25 LT-65421"},
                new Client{Name="Asta",Surname="Petrauskaitė", Email="ba@b.b", PhoneNum="+37077777777", Adress="Kauno g. 40-25 LT-65421"},
            };
            foreach (var item in clients)
            {
                context.Clients.Add(item);
            }
            context.SaveChanges();

            var grp = new ProductGroup[]
            {
                new ProductGroup{Name="Grybai"},
                new ProductGroup{Name="Uogos"}
            };
            foreach (var item in grp)
            {
                context.ProductGroups.Add(item);
            }
            context.SaveChanges();
            
            var prd = new Product[]
            {
                new Product{ProductGroupID=grp.Single( i => i.Name == "Grybai").ID,Name="Baravykai",Description="Paprastieji grybai",Price=1.25M,Quantity=10},
                new Product{ProductGroupID=grp.Single( i => i.Name == "Uogos").ID,Name="Mėlynės",Description="nedidelės uogos",Price=4.54M,Quantity=12.4M}
            };
            foreach (var item in prd)
            {
                context.Products.Add(item);
            }
            context.SaveChanges();

            var states = new State[]
            {
                new State{Name="Sukurta"},
                new State{Name="Laukiama"},
                new State{Name="Pradėta vykdyti"},
                new State{Name="Uždaryta"}
            };

            foreach (var item in states)
            {
                context.States.Add(item);
            }
            context.SaveChanges();

            var order = new Order[]
            {
                new Order{ClientID=clients.Single( i => i.Name == "Petras").ID,CreateDate=new DateTime(2010,11,11),Notes="Nori greitai",StateID=states.Single( i => i.Name == "Sukurta").ID,EmployeeID=employees.Single( i => i.Name == "Evaldas").ID}
            };

            foreach (var item in order)
            {
                context.Orders.Add(item);
            }
            context.SaveChanges();

            var orderItems = new OrderItem[]
            {
                new OrderItem{OrderID=order.Single( i => i.Notes == "Nori greitai").ID,ProductID=prd.Single( i => i.Name == "Baravykai").ID,Quantity=10,Price=5.4M}
            };

            foreach (var item in orderItems)
            {
                context.OrderItems.Add(item);
            }
            context.SaveChanges();


        }
    }
}
