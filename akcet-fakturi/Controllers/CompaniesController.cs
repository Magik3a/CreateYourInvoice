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
    public class CompaniesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Companies
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var companies = db.Companies.Include(c => c.Address).Where(m => m.UserId == userId);
            return View(companies.ToList());
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            ViewBag.IdAddress = new SelectList(db.Addresses, "IdAddress", "StreetName");
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "UserId", Include = "CompanyID,UserId,IdAddress,CompanyName,CompanyMol,DdsNumber,CompanyDescription,CompanyPhone,IsPrimary,DateCreated,DateModified")] Company company)
        {
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                company.DateCreated = DateTime.Now;
                company.DateModified = DateTime.Now;
                company.UserId = User.Identity.GetUserId();

                db.Companies.Add(company);
                db.SaveChanges();
                TempData["ResultSuccess"] = "Успешно добавихте компания!";
                return RedirectToAction("Index");
            }
            TempData["ResultError"] = "Грешка в добавяне на компания!";
            return RedirectToAction("Index");
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdAddress = new SelectList(db.Addresses, "IdAddress", "StreetName", company.IdAddress);
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company)
        {
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                var tbl = db.Companies.Find(company.CompanyID);
                
                company.DateModified = DateTime.Now;
                tbl = company;
               // db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();

                TempData["ResultSuccess"] = "Успешно редактирахте компания!";
                return RedirectToAction("Index");
            }

            ViewBag.IdAddress = new SelectList(db.Addresses, "IdAddress", "StreetName", company.IdAddress);
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();
            TempData["ResultSuccess"] = "Успешно изтрихте компания!";
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
