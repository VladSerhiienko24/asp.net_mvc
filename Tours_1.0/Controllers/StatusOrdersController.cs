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
    public class StatusOrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StatusOrders
        public ActionResult Index()
        {
            return View(db.StatusOrders.ToList());
        }

        // GET: StatusOrders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StatusOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StatusOrderID,StatusOrderName")] StatusOrder statusOrder)
        {
            if (ModelState.IsValid)
            {
                db.StatusOrders.Add(statusOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(statusOrder);
        }

        // GET: StatusOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StatusOrder statusOrder = db.StatusOrders.Find(id);
            if (statusOrder == null)
            {
                return HttpNotFound();
            }
            return View(statusOrder);
        }

        // POST: StatusOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StatusOrderID,StatusOrderName")] StatusOrder statusOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(statusOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(statusOrder);
        }

        // GET: StatusOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StatusOrder statusOrder = db.StatusOrders.Find(id);
            if (statusOrder == null)
            {
                return HttpNotFound();
            }
            return View(statusOrder);
        }

        // POST: StatusOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StatusOrder statusOrder = db.StatusOrders.Find(id);
            db.StatusOrders.Remove(statusOrder);
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
