using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompleksinisV2.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public ICollection<Mark> Marks { get; set; }
        //public ICollection<Enrollment> Enrollments { get; set; } // tipo cia sarasas ,kuris priklauso sitam vienetui
    }
}
