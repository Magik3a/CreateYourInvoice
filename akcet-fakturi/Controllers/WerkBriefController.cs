using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using akcetDB;

namespace akcet_fakturi.Controllers
{
    public class WerkBriefController : BaseController
    {
        private AppDbContext db = new AppDbContext();
        // GET: WerkBrief
        public ActionResult Index()
        {
            ViewBag.formNumber = 1;
            var userId = User.Identity.GetUserId();
            string error = "";

            if (!CheckUserDetails(userId, out error))
                TempData["ResultError"] = error;

            ViewBag.DaysOFWeek = new List<string>
            {
                "Ma",
                "Di",
                 "Wo",
                "Do",
                 "Fr",
                "Za",
                "Zo"
            };

            var weeks = FetchWeeks(DateTime.Now.Year);
            ViewBag.Weeks = new SelectList(weeks);

            ViewBag.Products = new SelectList(db.Products.Where(p => p.UserId == userId && p.IsDeleted == false), "ProductID", "ProductName");
            ViewBag.IdAddress = new SelectList(db.Addresses.Where(a => a.UserName == User.Identity.Name), "IdAddress", "StreetName");
            ViewBag.Companies = new SelectList(db.Companies.Where(m => m.UserId == userId && m.IsDeleted == false), "CompanyID", "CompanyName");
            ViewBag.Projects = new SelectList(db.Projects.Where(m => m.UserID == userId && m.IsDeleted == false), "ProjectID", "ProjectName");
            return View();
        }
        // Step 1
        public JsonResult CreateWerkbriefAjax(string Companies, WerkbriefTemp Werkbrief)
        {
            if (Companies == "0")
                return Json(false);

            if (String.IsNullOrWhiteSpace(Werkbrief.WerkbriefDate))
                return Json(false);

            if (String.IsNullOrWhiteSpace(Werkbrief.WerkbriefEndDate))
                return Json(false);

            Werkbrief.CompanyID = Int32.Parse(Companies);

            var userId = User.Identity.GetUserId();

            using (var context = new AkcetModel())
            {
                var temp = new List<WerkbriefHoursTemp>();
                Werkbrief.WerkbriefHoursTemps = temp;
                Werkbrief.UserId = userId;
                Werkbrief.DateCreated = DateTime.Now;
                Werkbrief.DateModified = DateTime.Now;
                Werkbrief.UserName = User.Identity.Name;
                // context.Werkbrief.Add(Werkbrief);
                // context.SaveChanges();

            }
            return Json(Werkbrief);
        }


        // Step 2
        public ActionResult SaveWerkbriefAjax(WerkbriefHoursTemp Model)
        {
            var model = new WerkbriefHoursTemp();
            ViewBag.DaysOFWeek = new List<string>
            {
                "Ma",
                "Di",
                 "Wo",
                "Do",
                 "Fr",
                "Za",
                "Zo"
            };
            var userId = User.Identity.GetUserId();
            var weeks = FetchWeeks(DateTime.Now.Year);
            ViewBag.Weeks = new SelectList(weeks);

            ViewBag.Products = new SelectList(db.Products.Where(p => p.UserId == userId && p.IsDeleted == false), "ProductID", "ProductName");
            ViewBag.IdAddress = new SelectList(db.Addresses.Where(a => a.UserName == User.Identity.Name), "IdAddress", "StreetName");
            ViewBag.Companies = new SelectList(db.Companies.Where(m => m.UserId == userId && m.IsDeleted == false), "CompanyID", "CompanyName");
            ViewBag.Projects = new SelectList(db.Projects.Where(m => m.UserID == userId && m.IsDeleted == false), "ProjectID", "ProjectName");
            //  model.ProductInvoiceID = Int32.Parse(Products);
            return PartialView("~/Views/Shared/WerkbriefsPartials/_TabWerkbriefsPartial.cshtml", model);
        }







        public ActionResult DeleteWerkbriefHoursTempTemp(int id)
        {
            //var userId = User.Identity.GetUserId();
            //var invoiceId = db.FakturiTemps.Where(s => s.UserId == userId).OrderByDescending(x => x.DateCreated).FirstOrDefault().InvoiceIDTemp;

            //var product = db.WerkbriefHoursTemps.Find(id);

            //db.WerkbriefHoursTemps.Remove(product);
            //db.SaveChanges();

            return Json(id, JsonRequestBehavior.AllowGet);
        }
    }
}