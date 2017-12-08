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

            var employees = new Employee[]
            {
                new Employee{Username="evaldas",Password="evaldas1",Email="lol@g.com"},
                new Employee{Username="jonas",Password="jonka",Email="ldl@g.com"},
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
