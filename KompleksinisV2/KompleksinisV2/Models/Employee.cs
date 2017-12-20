using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KompleksinisV2.Models
{
    public class Employee
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Šis laukas yra privalomas")]
        [MinLength(4, ErrorMessage = "Ne mažiau kaip 4 ženklai")]
        [MaxLength(20, ErrorMessage = "Ne daugiau kaip 20 simbolių")]
        [Display(Name = "Vardas")]
        public string Name { get; set; }

        [MinLength(4, ErrorMessage = "Ne mažiau kaip 4 ženklai")]
        [MaxLength(20, ErrorMessage = "Ne daugiau kaip 20 simbolių")]
        [Required(ErrorMessage = "Šis laukas yra privalomas")]
        [Display(Name = "Pavardė")]
        public string Surname { get; set; }

        [EmailAddress(ErrorMessage ="Netinkamas formatas")]
        [Required(ErrorMessage = "Šis laukas yra privalomas")]
        [Display(Name = "El. paštas")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Šis laukas yra privalomas")]
        [MinLength(4, ErrorMessage = "Ne mažiau kaip 4 ženklai")]
        [MaxLength(20, ErrorMessage = "Ne daugiau kaip 20 simbolių")]
        [DataType(DataType.Password)]
        [Display(Name = "Slaptažodis")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Šis laukas yra privalomas")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Gimimo data")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Šis laukas yra privalomas")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^([+])([0-9]{11})$", ErrorMessage = "Netinkamas formatas")]
        [Display(Name = "Telefono numeris")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Šis laukas yra privalomas")]
        [Display(Name = "Skyrius")]
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
