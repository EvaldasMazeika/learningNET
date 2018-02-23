using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KompleksinisV2.Models.ViewModels
{
    public class ChangePasswordViewModel : IValidatableObject
    {
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Šis laukas yra privalomas")]
        [MinLength(4, ErrorMessage ="Ne mažiau kaip 4 simboliai")]
        [MaxLength(20, ErrorMessage ="Ne daugiau kaip 20 simbolių")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Šis laukas yra privalomas")]
        [MinLength(4, ErrorMessage = "Ne mažiau kaip 4 simboliai")]
        [MaxLength(20, ErrorMessage = "Ne daugiau kaip 20 simbolių")]
        public string RepeatPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (RepeatPassword != NewPassword)
            {
                yield return new ValidationResult("Slaptažodžiai turi sutapti");
            }
        }
    }
}
