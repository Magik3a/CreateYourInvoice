using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace akcet_fakturi.Controllers
{
    public class AllWerkbriefsController : Controller
    {
        // GET: AllWerkbriefs
        public ActionResult AllWerkbriefs()
        {
            return View();
        }

        public ActionResult AllUserWerkbriefs()
        {
            return View();
        }


    }
}