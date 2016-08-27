using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
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

            ViewBag.IdAddress = new SelectList(db.Addresses.Where(a => a.UserName == User.Identity.Name), "IdAddress", "StreetName");
            ViewBag.Dds = new SelectList(db.DDs, "DdsId", "DdsName");
            ViewBag.Companies = new SelectList(db.Companies.Where(m => m.UserId == userId && m.IsDeleted == false), "CompanyID", "CompanyName");
            ViewBag.Projects = new SelectList(db.Projects.Where(m => m.UserID == userId && m.IsDeleted == false), "ProjectID", "ProjectName");
            ViewBag.Products = new SelectList(db.Products.Where(p => p.UserId == userId && p.IsDeleted == false), "ProductID", "ProductName");
            return View();
        }
    }
}