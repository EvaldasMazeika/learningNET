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
                new Employee{Name="Evaldas",Surname="Jonuos",Email="i@i.i",Password="evas",BirthDate=new DateTime(2017,01,01),MobileNumber="+37060000000",PositionID=1,SectorID=2},
                new Employee{Name="Jonas",Surname="Enoas",Email="a@a.a",Password="jonas",BirthDate=new DateTime(2010,11,11),MobileNumber="+37099900009",PositionID=2,SectorID=1},
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
                new Product{ProductGroupID=1,Name="Baravykai",Description="Paprastieji grybai",Price=1.25M},
                new Product{ProductGroupID=2,Name="Mėlynės",Description="nedidelės uogos",Price=4.54M,Quantity=12.4}
            };
            foreach (var item in prd)
            {
                context.Products.Add(item);
            }
            context.SaveChanges();

        }
    }
}
