using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KompleksinisV2.Models.ViewModels
{
    public class EditMySelfViewModel
    {
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
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telefono numeris")]
        public string MobileNumber { get; set; }
    }
}
