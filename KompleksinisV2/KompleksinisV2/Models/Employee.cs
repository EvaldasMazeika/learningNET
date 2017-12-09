using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompleksinisV2.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public string MobileNumber { get; set; }
        public int PositionID { get; set; }
        public int SectorID { get; set; }

        public Position Position { get; set; }
        public Sector Sector { get; set; }

        public ICollection<Mark> Marks { get; set; }
        //public ICollection<Enrollment> Enrollments { get; set; } // tipo cia sarasas ,kuris priklauso sitam vienetui
    }
}
