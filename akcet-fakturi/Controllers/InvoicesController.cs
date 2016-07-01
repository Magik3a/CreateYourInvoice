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
using Microsoft.AspNet.Identity;

namespace akcet_fakturi.Controllers
{
    [Authorize]
    public class InvoicesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Invoices
        public ActionResult Index()
        {
            var fakturis = db.Fakturis.Include(f => f.Company);
            return View(fakturis.ToList());
        }

        public ActionResult UserInvoices()
        {

            var userId = User.Identity.GetUserId();
            var fakturis = db.Fakturis.Include(f => f.Company).Where(f => f.UserID == userId);
            return View(fakturis.ToList());
        }

        // GET: Invoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fakturi fakturi = db.Fakturis.Find(id);
            if (fakturi == null)
            {
                return HttpNotFound();
            }
            return View(fakturi);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "UserId");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceID,CompanyID,ProductInvoiceID,InvoiceDate,InvoiceEndDate,Project,TotalPrice,DateCreated,DateModified,UserName")] Fakturi fakturi)
        {
            if (ModelState.IsValid)
            {
                db.Fakturis.Add(fakturi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "UserId", fakturi.CompanyID);
            return View(fakturi);
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fakturi fakturi = db.Fakturis.Find(id);
            if (fakturi == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "UserId", fakturi.CompanyID);
            return View(fakturi);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceID,CompanyID,ProductInvoiceID,InvoiceDate,InvoiceEndDate,Project,TotalPrice,DateCreated,DateModified,UserName")] Fakturi fakturi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fakturi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(db.Companies, "CompanyID", "UserId", fakturi.CompanyID);
            return View(fakturi);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fakturi fakturi = db.Fakturis.Find(id);
            if (fakturi == null)
            {
                return HttpNotFound();
            }
            return View(fakturi);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fakturi fakturi = db.Fakturis.Find(id);
            db.Fakturis.Remove(fakturi);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetInvoice(int IdInvoice)
        {
            
            var html = db.Fakturis.Find(IdInvoice).FakturaHtml;
            
            return Json(html);
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
