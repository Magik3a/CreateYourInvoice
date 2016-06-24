using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using akcetDB;
using akcet_fakturi.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;

namespace akcet_fakturi.Controllers
{
    [Authorize]
    public class AddInvoiceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AddInvoice
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            ViewBag.IdAddress = new SelectList(db.Addresses, "IdAddress", "StreetName");
            ViewBag.Dds = new SelectList(db.DDs, "DdsID", "Value");
            ViewBag.Companies = new SelectList(db.Companies.Where(m => m.UserId == userId), "CompanyID", "CompanyName");

            ViewBag.Products = new SelectList(db.Products.Where(p => p.UserId == userId), "ProductID", "ProductName");
            return View();
        }

        public ActionResult Products_Read([DataSourceRequest]DataSourceRequest request)
        {
            using (var context = new AkcetModel())
            {
                var products = context.ProductInvoiceTemps;
                DataSourceResult result = products.ToDataSourceResult(request);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

    }
}