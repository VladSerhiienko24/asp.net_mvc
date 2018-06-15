using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Tours_1._0.Models;

namespace Tours_1._0.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        [AuthorizeAdminOrManager]
        public ActionResult Index()
        {
            List<Order> orders = db.Orders.Include(o => o.StatusOrder).Include(o => o.Ticket).Include(o => o.Tour).Include(o => o.ApplicationUser).ToList();
            return View(orders);
        }

        // GET: Orders
        [Authorize(Roles = "user")]
        public ActionResult IndexUser()
        {
            var userId = User.Identity.GetUserId();
            var orders = db.Orders.Include(o => o.StatusOrder).Include(o => o.Ticket).Include(o => o.Tour).Include(o => o.ApplicationUser).Where(o => o.UserId == userId).ToList();
            return View(orders);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            Tour tour = db.Tours.FirstOrDefault(t => t.TourID == order.TourID.Value);
            StatusOrder statusOrder = db.StatusOrders.FirstOrDefault(s => s.StatusOrderID == order.StatusOrderID.Value);
            Hostel hostel = db.Hostels.FirstOrDefault(h => h.HostelID == tour.HostelID.Value);

            dynamic model = new ExpandoObject();
            model.Order = order;
            model.Tour = tour;
            model.Status = statusOrder.StatusOrderName;
            model.Hostel = hostel;

            return View(model);
        }

        // GET: Orders/Create
        [AuthorizeAdminOrManager]
        public ActionResult Create()
        {
            ViewBag.StatusOrderID = new SelectList(db.StatusOrders, "StatusOrderID", "StatusOrderName");
            ViewBag.OrderID = new SelectList(db.Tickets, "OrderId", "OrderId");
            ViewBag.TourID = new SelectList(db.Tours, "TourID", "TourName");
            //var users = db.Users
            //    .Where(u => u.Email != "admin@gmail.com")
            //    .Where(u => u.Email != "manager@gmail.com")
            //    .Select(
            //    s => new
            //    {
            //        Text = s.FirstName + " " + s.LastName + " " + s.UserName,
            //        Value = s.Id
            //    }).ToList();
            //ViewBag.Id = new SelectList(db.Users.Where(u => u.Email != "admin@gmail.com").Where(u => u.Email != "manager@gmail.com"), "Id", "FullName");
            ViewBag.Id = new SelectList(db.Users.Where(u => u.Email != "admin@gmail.com").Where(u => u.Email != "manager@gmail.com"), "Id", "FirstName");
            //ViewBag.Value = new SelectList(users, "Value", "Text");
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [AuthorizeAdminOrManager]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,UserId,TourID,DataTour,Price,StatusOrderID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StatusOrderID = new SelectList(db.StatusOrders, "StatusOrderID", "StatusOrderName", order.StatusOrderID);
            ViewBag.OrderID = new SelectList(db.Tickets, "OrderId", "OrderId", order.OrderID);
            ViewBag.TourID = new SelectList(db.Tours, "TourID", "TourName", order.TourID);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.StatusOrderID = new SelectList(db.StatusOrders, "StatusOrderID", "StatusOrderName", order.StatusOrderID);
            //ViewBag.OrderID = new SelectList(db.Tickets, "OrderId", "OrderId", order.OrderID);
            //ViewBag.TourID = new SelectList(db.Tours, "TourID", "TourName", order.TourID);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,UserId,TourID,DateOrder,Price,StatusOrderID")] Order order)
        {
            if (ModelState.IsValid)
            {
                StatusOrder status = db.StatusOrders.FirstOrDefault(s => s.StatusOrderID == order.StatusOrderID.Value);
                if (status.StatusOrderName == "Отменен")
                {
                    Order delOrder = db.Orders.Find(order.OrderID);
                    Ticket ticket = db.Tickets.FirstOrDefault(t => t.OrderId == order.OrderID);
                    db.Tickets.Remove(ticket);
                    db.Orders.Remove(delOrder);
                    db.SaveChanges();
                }
                //db.Entry(order).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StatusOrderID = new SelectList(db.StatusOrders, "StatusOrderID", "StatusOrderName", order.StatusOrderID);
            //ViewBag.OrderID = new SelectList(db.Tickets, "OrderId", "OrderId", order.OrderID);
            //ViewBag.TourID = new SelectList(db.Tours, "TourID", "TourName", order.TourID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            Tour tour = db.Tours.FirstOrDefault(t => t.TourID == order.TourID.Value);
            StatusOrder statusOrder = db.StatusOrders.FirstOrDefault(s => s.StatusOrderID == order.StatusOrderID.Value);
            Hostel hostel = db.Hostels.FirstOrDefault(h => h.HostelID == tour.HostelID.Value);
            var userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.FirstOrDefault(u => u.Id == order.UserId);

            dynamic model = new ExpandoObject();
            model.Order = order;
            model.Tour = tour;
            model.Status = statusOrder.StatusOrderName;
            model.Hostel = hostel;
            model.UserName = user.FirstName + " " + user.LastName;

            return View(model);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            Ticket ticket = db.Tickets.FirstOrDefault(t => t.OrderId == id);
            db.Tickets.Remove(ticket);
            db.Orders.Remove(order);
            db.SaveChanges();
            if (this.User.IsInRole("user"))
            {
                return RedirectToAction("IndexUser");
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [AuthorizeAdminOrManager]
        public FileResult OrdersPdf()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            ////file name to be created   
            string strPDFFileName = string.Format("OrdersPDF_" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 5 columns  
            PdfPTable tableLayout = new PdfPTable(5);
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table  

            //file will created in this path  
            string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);


            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            //Add Content to PDF   
            doc.Add(Add_Content_To_PDF(tableLayout));

            // Closing the document  
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return File(workStream, "application/pdf", strPDFFileName);
        }

        [AuthorizeAdminOrManager]
        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {

            float[] headers = { 50, 35, 35, 30, 35 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;

            //Add Title to the PDF file at the top  
            List<Order> orders = db.Orders.Include(o => o.Tour).Include(o => o.ApplicationUser).Include(o => o.StatusOrder).ToList<Order>();
            BaseFont baseFont = BaseFont.CreateFont(HostingEnvironment.MapPath("/fonts/arial.ttf"), BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            tableLayout.AddCell(new PdfPCell(new Phrase("Заказы", new Font(baseFont, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            ////Add header  
            AddCellToHeader(tableLayout, "Клиент");
            AddCellToHeader(tableLayout, "Тур");
            AddCellToHeader(tableLayout, "Цена");
            AddCellToHeader(tableLayout, "Статус заказа");
            AddCellToHeader(tableLayout, "Дата заказа");

            ////Add body  

            foreach (var order in orders)
            {

                AddCellToBody(tableLayout, order.ApplicationUser.FullName);
                AddCellToBody(tableLayout, order.Tour.TourName);
                AddCellToBody(tableLayout, order.Price.ToString());
                AddCellToBody(tableLayout, (order.StatusOrder.StatusOrderName));
                AddCellToBody(tableLayout, order.DateOrder);
            }

            return tableLayout;
        }

        // Method to add single cell to the Header  
        [AuthorizeAdminOrManager]
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            BaseFont baseFont = BaseFont.CreateFont(HostingEnvironment.MapPath("/fonts/arial.ttf"), BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(baseFont, 8, 1, iTextSharp.text.BaseColor.YELLOW)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0)
            });
        }

        // Method to add single cell to the body  
        [AuthorizeAdminOrManager]
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            BaseFont baseFont = BaseFont.CreateFont(HostingEnvironment.MapPath("/fonts/arial.ttf"), BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(baseFont, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }
    }
}
