using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompleksinisV2.Models
{
    public class Mark
    {
        public int MarkID { get; set; }
        public int CourseID { get; set; }
        public int EmployeeID { get; set; }
        public int Grade { get; set; }

        public Course Course { get; set; }
        public Employee Employee { get; set; }
    }
}
