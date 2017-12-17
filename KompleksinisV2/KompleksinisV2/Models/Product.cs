using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompleksinisV2.Models
{
    public class Product
    {
        public int ID { get; set; }
        public int ProductGroupID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public Decimal Quantity { get; set; }

        public ProductGroup ProductGroup { get; set; }

    }
}
