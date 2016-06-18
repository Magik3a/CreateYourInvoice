using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}