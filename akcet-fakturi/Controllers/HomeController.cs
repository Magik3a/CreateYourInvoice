using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using akcetDB;

namespace akcet_fakturi.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Invoices()
        {
          var some =  User.IsInRole("user");
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

            }

            return Json(true);
        }
    }
}