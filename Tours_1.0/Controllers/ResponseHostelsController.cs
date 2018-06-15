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
    public class ResponseHostelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ResponseHostels
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var responseHostels = db.ResponseHostels.Include(r => r.ApplicationUser).Include(r => r.Hostel);
            return View(responseHostels.ToList());
        }

        // GET: ResponseHostels/Create
        [Authorize(Roles = "user")]
        public ActionResult Create(int id)
        {
            Hostel hostel = db.Hostels.FirstOrDefault(h => h.HostelID == id);
            ResponseHostel response = new ResponseHostel
            {
                HostelID = id,
                UserId = User.Identity.GetUserId(),
                DateTime = DateTime.Now,
                Mark = 1,
                ResponseName = ""
            };

            return View(response);
        }

        // POST: ResponseHostels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "user")]
        public ActionResult Create([Bind(Include = "ResponseID,HostelID,UserId,ResponseName,Mark,DateTime")] ResponseHostel responseHostel)
        {
            if (ModelState.IsValid)
            {
                db.ResponseHostels.Add(responseHostel);
                db.SaveChanges();

                return RedirectToRoute(new
                {
                    controller = "Hostels",
                    action = "Details",
                    id = responseHostel.HostelID.ToString()
                });

            }

            return View(responseHostel);
        }

        // GET: ResponseHostels/Edit/5
        [Authorize(Roles = "user")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResponseHostel responseHostel = db.ResponseHostels.Find(id);
            if (responseHostel == null)
            {
                return HttpNotFound();
            }

            return View(responseHostel);
        }

        // POST: ResponseHostels/Edit/5
        [HttpPost]
        [Authorize(Roles = "user")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ResponseID,HostelID,UserId,ResponseName,Mark,DateTime")] ResponseHostel responseHostel)
        {
            if (ModelState.IsValid)
            {
                responseHostel.DateTime = DateTime.Now;
                db.Entry(responseHostel).State = EntityState.Modified;
                db.SaveChanges();

                //return RedirectToAction("Index");
                return RedirectToRoute(new
                {
                    controller = "Hostels",
                    action = "Details",
                    id = responseHostel.HostelID.ToString()
                });
            }

            return View(responseHostel);
        }

        // GET: ResponseHostels/Delete/5
        [AuthorizeAdminOrUser]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResponseHostel responseHostel = db.ResponseHostels.Find(id);
            if (responseHostel == null)
            {
                return HttpNotFound();
            }
            return View(responseHostel);
        }

        // POST: ResponseHostels/Delete/5
        [HttpPost, ActionName("Delete")]
        [AuthorizeAdminOrUser]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ResponseHostel responseHostel = db.ResponseHostels.Find(id);
            var pageId = responseHostel.HostelID.ToString();
            db.ResponseHostels.Remove(responseHostel);
            db.SaveChanges();

            if (User.IsInRole("user"))
            {
                return RedirectToRoute(new
                {
                    controller = "Hostels",
                    action = "Details",
                    id = pageId
                });
            }

            return RedirectToAction("Index","ResponseHostels");
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