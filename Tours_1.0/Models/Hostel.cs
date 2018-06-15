using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tours_1._0.Models
{
    public class Hostel
    {
        [Key]
        public int HostelID { get; set; }
        [MaxLength(30), MinLength(5)]
        [Display(Name = "Отель")]
        public string HostelName { get; set; }
        [Range(1, 5)]
        [Display(Name = "Комфорт")]
        public int HostelMark { get; set; }
        [StringLength(60)]
        [Display(Name = "Сайт")]
        [RegularExpression(@"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", ErrorMessage = "Not a valid website URL")]
        public string Website { get; set; }

        public ICollection<ResponseHostel> ResponseHostels { get; set; }
        public ICollection<Tour> Tours { get; set; }
    }
}