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

            var courses = new Course[]
            {
            new Course{Title="Chemistry"},
            new Course{Title="Microeconomics"},
            new Course{Title="Macroeconomics"},
            new Course{Title="Calculus"},

            };
            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();

            var marks = new Mark[]
            {
                new Mark{CourseID=1,EmployeeID=1,Grade=5},
                new Mark{CourseID=2,EmployeeID=1,Grade=2},
                new Mark{CourseID=3,EmployeeID=1,Grade=10},
                new Mark{CourseID=1,EmployeeID=2,Grade=7},
                new Mark{CourseID=2,EmployeeID=2,Grade=4}
            };

            foreach (var c in marks)
            {
                context.Marks.Add(c);
            }
            context.SaveChanges();

        }
    }
}
