using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using akcetDB;
using akcet_fakturi.Models;
using Microsoft.AspNet.Identity;

namespace akcet_fakturi.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Invoices()
        {
            ViewBag.IdAddress = new SelectList(db.Addresses, "IdAddress", "StreetName");
            return View();
        }

        public ActionResult CreateCompanyAjax([Bind(Exclude = "UserId", Include = "CompanyID,UserId,IdAddress,CompanyName,CompanyMol,CompanyBulsatat,CompanyDescription,CompanyPhone,IsPrimary,DateCreated,DateModified")] Company company)
        {

            ModelState.Remove("UserId");

            if (!Request.IsAjaxRequest())
            {
                return Json(false);
            }
            if (!ModelState.IsValid)
            {
                return Json(false);
            }

            using (var context = new AkcetModel())
            {
                company.DateCreated = DateTime.Now;
                company.DateModified = DateTime.Now;
                company.UserId = User.Identity.GetUserId();

                db.Companies.Add(company);
                db.SaveChanges();


                return Json(new { id = company.CompanyID, value = company.CompanyName });
            }
        }

        public  JsonResult CreateAddressAjax(Address modelAddress)
        {
            if (!Request.IsAjaxRequest())
            {
                TempData["ResultError"] = "Грешка в добавяне на адрес!";
                return Json(false);
            }
            if (!ModelState.IsValid)
            {
                TempData["ResultError"] = "Грешка в добавяне на адрес!";
                return Json(false);
            }

            using (var context = new AkcetModel())
            {
                var address = new Address();
                address = modelAddress;
                address.DateCreated = DateTime.Now;
                address.DateModified = DateTime.Now;
                address.UserName = User.Identity.Name;
                context.Addresses.Add(address);

                context.SaveChanges();
                
                TempData["ResultSuccess"] = "Успешно добавихте адрес!";

                return Json(new {id = address.IdAddress, value = address.StreetName});

            }
        }

        public JsonResult CreateProductAjax(Product modelProduct)
        {
            ModelState.Remove("UserId");

            if (!Request.IsAjaxRequest())
            {
                TempData["ResultError"] = "Грешка в добавяне на адрес!";
                return Json(false);
            }
            if (!ModelState.IsValid)
            {
                TempData["ResultError"] = "Грешка в добавяне на адрес!";
                return Json(false);
            }

            using (var context = new AkcetModel())
            {
                modelProduct.DateCreated = DateTime.Now;
                modelProduct.DateModified = DateTime.Now;
                modelProduct.UserId = User.Identity.GetUserId();

                db.Products.Add(modelProduct);
                db.SaveChanges();


                return Json(new { id = modelProduct.ProductID, value = modelProduct.ProductName });
            }
        }
    }
}