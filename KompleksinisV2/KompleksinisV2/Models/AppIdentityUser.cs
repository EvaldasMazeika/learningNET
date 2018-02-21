using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace KompleksinisV2.Models
{
    public class AppIdentityUser : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public int DepartmentID { get; set; }
        public Department Department { get; set; }

        public string FullName
        {
            get
            {
                return Name + " " + Surname;
            }
        }
    }
}
