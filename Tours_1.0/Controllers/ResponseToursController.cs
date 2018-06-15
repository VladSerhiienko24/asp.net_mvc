using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tours_1._0.Models;

namespace Tours_1._0.Controllers
{
    public class ResponseToursController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ResponseTours
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var responseTours = db.ResponseTours.Include(r => r.ApplicationUser).Include(r => r.Tour);
            return View(responseTours.ToList());
        }

        // GET: ResponseTours/Create
        [Authorize(Roles = "user")]
        public ActionResult Create(int id)
        {
            Tour hostel = db.Tours.FirstOrDefault(h => h.TourID == id);
            ResponseTour response = new ResponseTour
            {
                TourID = id,
                UserId = User.Identity.GetUserId(),
                DateTime = DateTime.Now,
                Mark = 1,
                ResponseName = ""
            };

            return View(response);
        }

        // POST: ResponseTours/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public ActionResult Create([Bind(Include = "ResponseID,TourID,UserId,ResponseName,Mark,DateTime")] ResponseTour responseTour)
        {
            if (ModelState.IsValid)
            {
                db.ResponseTours.Add(responseTour);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToRoute(new
                {
                    controller = "Tours",
                    action = "Details",
                    id = responseTour.TourID.ToString()
                });
            }
            
            return View(responseTour);
        }

        // GET: ResponseTours/Edit/5
        [Authorize(Roles = "user")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResponseTour responseTour = db.ResponseTours.Find(id);
            if (responseTour == null)
            {
                return HttpNotFound();
            }

            return View(responseTour);
        }

        // POST: ResponseTours/Edit/5
        [HttpPost]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ResponseID,TourID,UserId,ResponseName,Mark,DateTime")] ResponseTour responseTour)
        {
            if (ModelState.IsValid)
            {
                responseTour.DateTime = DateTime.Now;
                db.Entry(responseTour).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToRoute(new
                {
                    controller = "Tours",
                    action = "Details",
                    id = responseTour.TourID.ToString()
                });
            }
            
            return View(responseTour);
        }

        // GET: ResponseTours/Delete/5
        [AuthorizeAdminOrUser]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResponseTour responseTour = db.ResponseTours.Find(id);
            if (responseTour == null)
            {
                return HttpNotFound();
            }
            return View(responseTour);
        }

        // POST: ResponseTours/Delete/5
        [HttpPost, ActionName("Delete")]
        [AuthorizeAdminOrUser]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ResponseTour responseTour = db.ResponseTours.Find(id);
            var pageId = responseTour.TourID.ToString();
            db.ResponseTours.Remove(responseTour);
            db.SaveChanges();

            if (User.IsInRole("user"))
            {
                return RedirectToRoute(new
                {
                    controller = "Tours",
                    action = "Details",
                    id = pageId
                });
            }

            return RedirectToAction("Index", "ResponseTours");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
