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
    public class HostelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Hostels
        [AuthorizeAdminOrManager]
        public ActionResult Index()
        {
            return View(db.Hostels.ToList());
        }

        //[Authorize(Roles = "user")]
        [AuthorizeManagerOrUser]
        public ActionResult IndexUser()
        {
            return View(db.Hostels.ToList());
        }

        // GET: Hostels/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hostel hostel = db.Hostels.Find(id);
            if (hostel == null)
            {
                return HttpNotFound();
            }
            List<ResponseHostel> responses = db.ResponseHostels.Include(u => u.ApplicationUser).Where(t => t.HostelID == id).ToList();
            var userId = User.Identity.GetUserId();

            dynamic model = new ExpandoObject();
            model.Hostel = hostel;
            model.Responses = responses;
            model.Id = userId;

            return View(model);
        }

        // GET: Hostels/Create
        [Authorize(Roles ="admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Hostels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Create([Bind(Include = "HostelID,HostelName,HostelMark,Website")] Hostel hostel)
        {
            if (ModelState.IsValid)
            {
                db.Hostels.Add(hostel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hostel);
        }

        // GET: Hostels/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hostel hostel = db.Hostels.Find(id);
            if (hostel == null)
            {
                return HttpNotFound();
            }
            return View(hostel);
        }

        // POST: Hostels/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HostelID,HostelName,HostelMark,Website")] Hostel hostel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hostel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hostel);
        }

        // GET: Hostels/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hostel hostel = db.Hostels.Find(id);
            if (hostel == null)
            {
                return HttpNotFound();
            }
            return View(hostel);
        }

        // POST: Hostels/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Hostel hostel = db.Hostels.Find(id);
            db.Hostels.Remove(hostel);
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
        public FileResult HostelsPdf()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            ////file name to be created   
            string strPDFFileName = string.Format("HostelsPDF_" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 5 columns  
            PdfPTable tableLayout = new PdfPTable(3);
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

            float[] headers = { 50, 24, 45}; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;

            //Add Title to the PDF file at the top  
            List<Hostel> hostels = db.Hostels.ToList<Hostel>();
            BaseFont baseFont = BaseFont.CreateFont(HostingEnvironment.MapPath("/fonts/arial.ttf"), BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            tableLayout.AddCell(new PdfPCell(new Phrase("Список отелей", new Font(baseFont, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            ////Add header  
            AddCellToHeader(tableLayout, "Отель");
            AddCellToHeader(tableLayout, "Комфорт");
            AddCellToHeader(tableLayout, "Сайт");

            ////Add body  

            foreach (var hostel in hostels)
            {
                AddCellToBody(tableLayout, hostel.HostelName);
                AddCellToBody(tableLayout, hostel.HostelMark.ToString());
                AddCellToBody(tableLayout, hostel.Website);
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
