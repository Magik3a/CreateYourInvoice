using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using akcetDB;
using akcet_fakturi.Models;

namespace akcet_fakturi.Controllers
{
    [Authorize]
    public class DDsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DDs
        public ActionResult Index()
        {
            return View(db.DDs.ToList());
        }

        // GET: DDs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DD dD = db.DDs.Find(id);
            if (dD == null)
            {
                return HttpNotFound();
            }
            return View(dD);
        }

        // GET: DDs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DDs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DdsID,Name,Value,DateCreated,DateModified,UserName")] DD dD)
        {
            if (ModelState.IsValid)
            {
                dD.DateCreated = DateTime.Now;
                dD.DateModified = DateTime.Now;
                dD.UserName = User.Identity.Name;
                db.DDs.Add(dD);
                db.SaveChanges();
                TempData["ResultSuccess"] = "Успешно добавихте нова стойност!";
                return RedirectToAction("Index");

            }
            return View(dD);
        }

        // GET: DDs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DD dD = db.DDs.Find(id);
            if (dD == null)
            {
                return HttpNotFound();
            }
            return View(dD);
        }

        // POST: DDs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "DateCreated", Include = "DdsID,Name,Value,DateModified,UserName")] DD dD)
        {
    
            if (ModelState.IsValid)
            {
                var dds = db.DDs.Find(dD.DdsID);
                dds.DdsName = dD.DdsName;
                dds.DateModified = DateTime.Now;
                dds.UserName = User.Identity.Name;
                dds.Value = dD.Value;
                db.SaveChanges();
                TempData["ResultSuccess"] = "Успешно редактирахте стойност!";
                return RedirectToAction("Index");
            }
            return View(dD);
        }

        // GET: DDs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DD dD = db.DDs.Find(id);
            if (dD == null)
            {
                return HttpNotFound();
            }
            return View(dD);
        }

        // POST: DDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DD dD = db.DDs.Find(id);
            db.DDs.Remove(dD);
            db.SaveChanges();
            TempData["ResultSuccess"] = "Успешно изтрихте стойност!";
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
