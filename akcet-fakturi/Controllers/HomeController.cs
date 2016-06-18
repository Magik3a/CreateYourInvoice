using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using akcetDB;
using akcet_fakturi.Models;

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

        public ActionResult CreateCompanyAjax()
        {
            return Json("success");
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
    }
}