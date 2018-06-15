using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tours_1._0.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public int? TourID { get; set; }
        
        //[Display(Name = "Кол-во человек")]
        //public int QuantityMembers { get; set; }
        //[Display(Name = "Кол-во дней")]
        //public int QuantityDays { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Дата заказа")]
        public string DateOrder { get; set; }

        
        public Tour Tour { get; set; }
        
        public int? StatusOrderID { get; set; }
        public StatusOrder StatusOrder { get; set; }

        public Ticket Ticket { get; set; }
    }
}