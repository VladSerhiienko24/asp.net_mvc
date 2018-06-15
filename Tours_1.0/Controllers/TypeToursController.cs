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
    public class TypeToursController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TypeTours
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            return View(db.TypeTours.ToList());
        }

        // GET: TypeTours/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: TypeTours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TypeTourID,TypeTourName")] TypeTour typeTour)
        {
            if (ModelState.IsValid)
            {
                db.TypeTours.Add(typeTour);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(typeTour);
        }

        // GET: TypeTours/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeTour typeTour = db.TypeTours.Find(id);
            if (typeTour == null)
            {
                return HttpNotFound();
            }
            return View(typeTour);
        }

        // POST: TypeTours/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TypeTourID,TypeTourName")] TypeTour typeTour)
        {
            if (ModelState.IsValid)
            {
                db.Entry(typeTour).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(typeTour);
        }

        // GET: TypeTours/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeTour typeTour = db.TypeTours.Find(id);
            if (typeTour == null)
            {
                return HttpNotFound();
            }
            return View(typeTour);
        }

        // POST: TypeTours/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TypeTour typeTour = db.TypeTours.Find(id);
            db.TypeTours.Remove(typeTour);
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
    }
}
