using System.Collections.Generic;

namespace KompleksinisV2.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string Title { get; set; }

        public ICollection<Mark> Mark { get; set; }
    }
}