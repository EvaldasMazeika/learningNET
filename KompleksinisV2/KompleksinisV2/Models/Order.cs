using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KompleksinisV2.Models
{
    public class Order
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Šis laukas yra privalomas")]
        [Display(Name = "Klientas")]
        public int ClientID { get; set; }

        public int EmployeeID { get; set; }

        [MinLength(5, ErrorMessage = "Ne mažiau kaip 5 ženklai")]
        [MaxLength(100, ErrorMessage = "Ne daugiau kaip 100 simbolių")]
        [Display(Name = "Pastabos")]
        public string Notes { get; set; }
        public DateTime CreateDate { get; set; }
        public int StateID { get; set; }

        public Employee Employee { get; set; }
        public Client Client { get; set; }
        public State State { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

    }
}
