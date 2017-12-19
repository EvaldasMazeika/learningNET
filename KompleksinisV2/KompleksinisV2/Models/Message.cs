using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KompleksinisV2.Models
{
    public class Message
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Šis laukas yra privalomas")]
        [MinLength(5, ErrorMessage = "Ne mažiau kaip 5 ženklai")]
        [MaxLength(20, ErrorMessage ="Ne daugiau kaip 20 ženklų")]
        [Display(Name = "Pavadinimas")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Šis laukas yra privalomas")]
        [MinLength(5, ErrorMessage = "Ne mažiau kaip 5 ženklai")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Tekstas")]
        public string WrittenText { get; set; }
        public DateTime WriteDate { get; set; }

        [Display(Name = "Skyrius")]
        public int DepartmentID { get; set; }

        public Employee Employee { get; set; }
        public ICollection<Comments> Comments { get; set; }
        public Department Department { get; set; }
    }
}
