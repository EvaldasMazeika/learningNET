using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KompleksinisV2.Models
{
    public class OrderItem
    {
        public int ID { get; set; }
        public int OrderID { get; set; }

        [Display(Name = "Prekė")]
        public int ProductID { get; set; }

        [Range(0.01, 9999.99,ErrorMessage ="Turi būti intervale nuo 0.01 iki 9999.99")]
        [Display(Name = "Kiekis (kg)")]
        public Decimal Quantity { get; set; }

        [Range(0.01, 9999.99, ErrorMessage ="Turi būti intervale nuo 0.01 iki 9999.99")]
        [Display(Name = "Kaina (kg)")]
        public Decimal Price { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
