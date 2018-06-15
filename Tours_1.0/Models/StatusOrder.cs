using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tours_1._0.Models
{
    public class StatusOrder
    {
        [Key]
        public int StatusOrderID { get; set; }
        [MaxLength(30), MinLength(5)]
        [Display(Name = "Статус заказа")]
        public string StatusOrderName { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}