using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace akcet_fakturi.Areas.EmailTemplates.Controllers
{
    public class TemplateController : Controller
    {
        // GET: EmailTemplates/Template
        public ActionResult InvoiceEmailTemplate()
        {
            return View();
        }
    }
}