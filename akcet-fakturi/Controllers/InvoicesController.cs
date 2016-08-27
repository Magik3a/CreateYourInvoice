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
using Data;

namespace akcet_fakturi.Controllers
{
    [Authorize]
    public class InvoicesController : Controller
    {
        private AppDbContext db = new AppDbContext();
        private AkcetModel dbAkcet = new AkcetModel();
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
            Fakturi fakturi = dbAkcet.Fakturis.Find(id);

            foreach(var product in dbAkcet.ProductInvoices.Where(p => p.InvoiceID == id))
            {
                dbAkcet.ProductInvoices.Remove(product);
            }
            var counter = Int32.Parse(fakturi.FakturaNumber.Split('-')[1]) + 1;
            var counterTbl = dbAkcet.Counters.Where(c => c.CounterValue == counter && c.UserID == fakturi.UserID).FirstOrDefault();
           // TODO: When invoice is deleted counter is still in max value
            dbAkcet.Fakturis.Remove(fakturi);
            dbAkcet.SaveChanges();
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
