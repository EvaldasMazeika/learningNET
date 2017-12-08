using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompleksinisV2.Models
{
    public class Message
    {
        public int ID { get; set; }
        public string WrittenText { get; set; }
        public DateTime WriteDate { get; set; }

        public Employee Employee { get; set; }

    }
}
