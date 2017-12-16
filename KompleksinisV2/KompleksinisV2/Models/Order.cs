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
        public int ClientID { get; set; }
        public int ProductID { get; set; }

        public Decimal Quantity { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public Decimal Price { get; set; }
        public string Notes { get; set; }

        public Client Client { get; set; }
        public Product Product { get; set; }

    }
}
