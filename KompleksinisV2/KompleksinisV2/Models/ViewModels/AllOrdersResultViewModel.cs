using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompleksinisV2.Models.ViewModels
{
    public class AllOrdersResultViewModel
    {
        public Guid EmployeeID { get; set; }
        public string FullName { get; set; }
        public int OrdersNumber { get; set; }
        public Decimal TotalPrice { get; set; }
        public Decimal TotalProfit { get; set; }

    }
}
