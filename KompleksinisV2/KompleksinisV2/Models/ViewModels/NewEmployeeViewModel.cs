using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KompleksinisV2.Models.ViewModels
{
    public class NewEmployeeViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^([+])([0-9]{11})$", ErrorMessage = "Netinkamas formatas")]
        public string MobileNumber { get; set; }
        public Guid DepartmentId { get; set; }

    }
}
