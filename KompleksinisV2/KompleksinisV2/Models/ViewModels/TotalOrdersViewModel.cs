using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KompleksinisV2.Models.ViewModels
{
    public class TotalOrdersViewModel : IValidatableObject
    {
        [Required]
        [Display(Name ="Darbuotojas")]
        public int EmployeeID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Laikotarpio pradžia")]
        [Required(ErrorMessage = "Šis laukas yra privalomas")]
        public DateTime BeginDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Laikotarpio pabaiga")]
        [Required(ErrorMessage = "Šis laukas yra privalomas")]
        public DateTime EndDate { get; set; }

        public Employee Employee { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int result = DateTime.Compare(BeginDate,EndDate);
            if (result <= 0)
            {
                yield return new ValidationResult("Blogi laikotarpiai");
            }
        }

    }
}
