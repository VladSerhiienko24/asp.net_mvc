using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tours_1._0.Models
{
    public class TypeTour
    {
        [Key]
        public int TypeTourID { get; set; }
        [MaxLength(30), MinLength(5)]
        [Display(Name = "Тип")]
        public string TypeTourName { get; set; }

        public ICollection<Tour> Tours { get; set; }
    }
}