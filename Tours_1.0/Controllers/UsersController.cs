using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Web.Hosting;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Tours_1._0.Models;

namespace Tours_1._0.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Users
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var users = db.Users.Where(u => u.Email != "admin@gmail.com").Where(u => u.Email != "manager@gmail.com").ToList();
            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Block(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult Block(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var userBlocked = db.Users.Where(a => a.Id == user.Id).FirstOrDefault();
                if (!userBlocked.LockoutEnabled)
                {
                    userBlocked.LockoutEnabled = true;
                    userBlocked.LockoutEndDateUtc = DateTime.MaxValue;
                }
                else
                {
                    userBlocked.LockoutEnabled = false;
                    userBlocked.LockoutEndDateUtc = DateTime.Now;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [Authorize(Roles = "admin")]
        public FileResult UsersPdf()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;
            ////file name to be created   
            string strPDFFileName = string.Format("UsersPDF_" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
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

        [Authorize(Roles = "admin")]
        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {

            float[] headers = { 50, 35, 35, 30, 35 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;

            //Add Title to the PDF file at the top  
            List<ApplicationUser> users = db.Users.ToList<ApplicationUser>();
            BaseFont baseFont = BaseFont.CreateFont(HostingEnvironment.MapPath("/fonts/arial.ttf"), BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            tableLayout.AddCell(new PdfPCell(new Phrase("Пользователи", new Font(baseFont, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            ////Add header  
            AddCellToHeader(tableLayout, "Идентификатор");
            AddCellToHeader(tableLayout, "Имя");
            AddCellToHeader(tableLayout, "Фамилия");
            AddCellToHeader(tableLayout, "Почта");
            AddCellToHeader(tableLayout, "Доступ");

            ////Add body  

            foreach (var user in users)
            {

                AddCellToBody(tableLayout, user.Id.ToString());
                AddCellToBody(tableLayout, user.FirstName);
                AddCellToBody(tableLayout, user.LastName);
                AddCellToBody(tableLayout, user.Email);
                AddCellToBody(tableLayout, (user.LockoutEnabled ? "Заблокирован":""));
            }

            return tableLayout;
        }

        // Method to add single cell to the Header  
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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