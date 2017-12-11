using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KompleksinisV2.Models
{
    public class Comments
    {
        public int ID { get; set; }
        public int MessageID { get; set; }
        public string Fullname { get; set; }
        public DateTime ComDate { get; set; }

        [Required(ErrorMessage = "Šis laukas yra privalomas")]
        [MinLength(5, ErrorMessage = "Ne mažiau kaip 5 ženklai")]
        [Display(Name ="Komentaras")]
        public string Comment { get; set; }

        public Message Message { get; set; }
    }
}
