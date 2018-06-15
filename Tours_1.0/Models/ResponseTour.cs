using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tours_1._0.Models
{
    public class ResponseTour
    {
        [Key]
        public int ResponseID { get; set; }

        public int? TourID { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }

        [MaxLength(70), MinLength(5)]
        [Display(Name = "Отзыв")]
        public string ResponseName { get; set; }
        [Range(1, 5)]
        [Display(Name = "Оценка")]
        public int Mark { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Дата")]
        public DateTime DateTime { get; set; }

        //public ApplicationUser ApplicationUser { get; set; }
        public Tour Tour { get; set; }
    }
}