using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tours_1._0.Services
{
    public class RenderChart
    {
        public string DateId { get; set; }
        public int Orders { get; set; }
    }

    public class RenderPie
    {
        public int MenuId { get; set; }
        public string Orders { get; set; }
    }
}