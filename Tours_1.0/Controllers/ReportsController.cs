using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using Tours_1._0.Models;
using Tours_1._0.Services;

namespace Tours_1._0.Controllers
{
    public class ReportsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reports
        [AuthorizeAdminOrManager]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeAdminOrManager]
        public ActionResult ReportChart()
        {
            List<RenderChart> orders = db.Orders.GroupBy(o => o.DateOrder).Select(o => new RenderChart { DateId = o.Key, Orders = (o.ToList()).Count() }).ToList();
            List<string> listDate = new List<string>();
            List<string> listCount = new List<string>();
            foreach (var item in orders)
            {
                listDate.Add(item.DateId);
                listCount.Add(item.Orders.ToString());
            }

            var myChart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
            .AddTitle("Отчет о ежедневных заказах")
            .AddSeries(
            name: "Отчет",
            xValue: listDate,
            yValues: listCount)
            .Write();

            return null;
        }

        [AuthorizeAdminOrManager]
        public ActionResult ReportChartPie()
        {
            var orders = db.Orders.OrderBy(o => o.TourID).GroupBy(o => o.TourID)
                .Select(o => new RenderPie { MenuId = o.Key.Value, Orders = o.ToList().Count().ToString() }).ToList();

            List<string> listMenuName = new List<string>();
            List<string> listCount = new List<string>();

            foreach (var item in orders)
            {
                int id = item.MenuId;
                Tour t = db.Tours.FirstOrDefault(d => d.TourID == id);
                listMenuName.Add(t.TourName);
                listCount.Add(item.Orders);
            }

            var myChart = new Chart(width: 600, height: 400, theme: ChartTheme.Blue)
            .AddTitle("Отчет о заказах туров")
            .AddSeries(
            name: "Отчет",
            chartType: "Pie",
            xValue: listMenuName,
            yValues: listCount)
            .Write();

            return null;
        }
    }
}