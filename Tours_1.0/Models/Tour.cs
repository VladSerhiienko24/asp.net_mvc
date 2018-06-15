using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Tours_1._0.Models
{
    public class Tour
    {
        [Key]
        public int TourID { get; set; }
        [MaxLength(30), MinLength(2)]
        [Display(Name = "Тур")]
        public string TourName { get; set; }
        public int? TypeTourID { get; set; }

        [MaxLength(100), MinLength(2)]
        [Display(Name = "Достопримечательности")]
        public string TourSights { get; set; }

        [StringLength(25)]
        [Display(Name = "Дата отъезда")]
        public string DateStart { get; set; }

        [StringLength(25)]
        [Display(Name = "Дата приезда")]
        public string DateEnd { get; set; }

        public int? HostelID { get; set; }
        
        [Display(Name = "Горящая путевка")]
        public bool StatusHot { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        public TypeTour TypeTour { get; set; }
        public Hostel Hostel { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<ResponseTour> ResponseTours { get; set; }
    }
}