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
    public class ToursController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AuthorizeAdminOrManager]
        public ActionResult Index(string searchString)
        {
            var tours = db.Tours.Include(t => t.Hostel).Include(t => t.TypeTour).OrderByDescending(t => t.StatusHot).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                tours = db.Tours.Where(t => t.TourName.ToUpper().Contains(searchString.ToUpper())).ToList();
            }
            return View(tours);
        }

        [Authorize(Roles = "user")]
        public ActionResult IndexUser(string searchString)
        {
            var tours = db.Tours.Include(t => t.Hostel).Include(t => t.TypeTour).OrderByDescending(t => t.StatusHot).ToList();
            if (!String.IsNullOrEmpty(searchString))
            {
                tours = db.Tours.Where(t => t.TourName.ToUpper().Contains(searchString.ToUpper())).ToList();
            }
            return View(tours);
        }
        
        [HttpGet]
        [Authorize(Roles = "user")]
        public ActionResult UserOrder(int id)
        {
            string userId = User.Identity.GetUserId();
            Order checkorder = db.Orders.Where(t => t.UserId == userId).Where(t => t.TourID == id).FirstOrDefault();
            if (checkorder != null)
            {
                return RedirectToAction("OrderError");
            }
            Tour tour = db.Tours.FirstOrDefault(t => t.TourID == id);
            Order order = new Order
            {
                TourID = id,
                StatusOrderID = 1,
                DateOrder = DateTime.Now.ToShortDateString(),
                Price = tour.Price
            };
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public ActionResult UserOrder(Order order)
        {
            order.UserId = User.Identity.GetUserId();
            db.Orders.Add(order);
            db.SaveChanges();
            Ticket ticket = new Ticket { OrderId = order.OrderID };
            db.Tickets.Add(ticket);
            db.SaveChanges();
            return RedirectToAction("IndexUser");
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public ActionResult OrderError()
        {
            return View();
        }
        
        // GET: Tours/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);            
            if (tour == null)
            {
                return HttpNotFound();
            }
            TypeTour typeTour = db.TypeTours.Where(t => t.TypeTourID == tour.TypeTourID).FirstOrDefault();
            Hostel hostel = db.Hostels.Where(h => h.HostelID == tour.HostelID).FirstOrDefault();
            List<ResponseTour> responses = db.ResponseTours.Include(u => u.ApplicationUser).Where(t => t.TourID == id).ToList();
            var userId = User.Identity.GetUserId();

            dynamic model = new ExpandoObject();
            model.tour = tour;
            model.typeTour = typeTour.TypeTourName;
            model.hostel = hostel;
            model.Responses = responses;
            model.Id = userId;

            return View(model);
        }

        // GET: Tours/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.HostelID = new SelectList(db.Hostels, "HostelID", "HostelName");
            ViewBag.TypeTourID = new SelectList(db.TypeTours, "TypeTourID", "TypeTourName");
            return View();
        }

        // POST: Tours/Create
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TourID,TourName,TypeTourID,TourSights,HostelID,DateStart,DateEnd,StatusHot,Price")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                db.Tours.Add(tour);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HostelID = new SelectList(db.Hostels, "HostelID", "HostelName", tour.HostelID);
            ViewBag.TypeTourID = new SelectList(db.TypeTours, "TypeTourID", "TypeTourName", tour.TypeTourID);
            return View(tour);
        }

        // GET: Tours/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            ViewBag.HostelID = new SelectList(db.Hostels, "HostelID", "HostelName", tour.HostelID);
            ViewBag.TypeTourID = new SelectList(db.TypeTours, "TypeTourID", "TypeTourName", tour.TypeTourID);
            return View(tour);
        }

        // POST: Tours/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TourID,TourName,TypeTourID,TourSights,HostelID,DateStart,DateEnd,StatusHot,Price")] Tour tour)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tour).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HostelID = new SelectList(db.Hostels, "HostelID", "HostelName", tour.HostelID);
            ViewBag.TypeTourID = new SelectList(db.TypeTours, "TypeTourID", "TypeTourName", tour.TypeTourID);
            return View(tour);
        }

        // GET: Tours/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            TypeTour typeTour = db.TypeTours.Where(t => t.TypeTourID == tour.TypeTourID).FirstOrDefault();
            Hostel hostel = db.Hostels.Where(h => h.HostelID == tour.HostelID).FirstOrDefault();
            if (tour == null)
            {
                return HttpNotFound();
            }
            dynamic model = new ExpandoObject();
            model.tour = tour;
            model.typeTour = typeTour.TypeTourName;
            model.hostel = hostel;
            return View(model);
        }

        // POST: Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tour tour = db.Tours.Find(id);
            db.Tours.Remove(tour);
            db.SaveChanges();
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
        public FileResult ToursPdf()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            ////file name to be created   
            string strPDFFileName = string.Format("ToursPDF_" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 5 columns  
            PdfPTable tableLayout = new PdfPTable(4);
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

            float[] headers = { 50, 24, 45, 35 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;

            //Add Title to the PDF file at the top  
            List<Tour> tours = db.Tours.ToList<Tour>();
            BaseFont baseFont = BaseFont.CreateFont(HostingEnvironment.MapPath("/fonts/arial.ttf"), BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            tableLayout.AddCell(new PdfPCell(new Phrase("Актуальные туры", new Font(baseFont, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            ////Add header  
            AddCellToHeader(tableLayout, "Название");
            AddCellToHeader(tableLayout, "Дата начала");
            AddCellToHeader(tableLayout, "Дата конца");
            AddCellToHeader(tableLayout, "Горящий тур");

            ////Add body  

            foreach (var tour in tours)
            {
                AddCellToBody(tableLayout, tour.TourName);
                AddCellToBody(tableLayout, tour.DateStart);
                AddCellToBody(tableLayout, tour.DateEnd);
                AddCellToBody(tableLayout, (tour.StatusHot ? "Горящий":"Обычный"));
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