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

        [EmailAddress]
        [Required(ErrorMessage = "Šis laukas yra privalomas")]
        [Display(Name = "E-paštas")]
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
        [Display(Name = "Telefono numeris")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Šis laukas yra privalomas")]
        [Display(Name = "Pozicija")]
        public int PositionID { get; set; }

        [Required(ErrorMessage = "Šis laukas yra privalomas")]
        [Display(Name = "Skyrius")]
        public int SectorID { get; set; }

        public Position Position { get; set; }
        public Sector Sector { get; set; }

       // public ICollection<Mark> Marks { get; set; }
        //public ICollection<Enrollment> Enrollments { get; set; } // tipo cia sarasas ,kuris priklauso sitam vienetui
    }
}
