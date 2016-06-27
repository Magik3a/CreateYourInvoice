using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using akcetDB;
using akcet_fakturi.Areas.InvoiceTemplates.Models;
using akcet_fakturi.Controllers;
using akcet_fakturi.Models;
using Microsoft.AspNet.Identity;

namespace akcet_fakturi.Areas.InvoiceTemplates.Controllers
{
    public class InvoiceTemplateController : BaseController
    {

        private AkcetModel db = new AkcetModel();
        private ApplicationDbContext dbUser = new ApplicationDbContext();
        // GET: InvoiceTemplates/InvoiceTemplate
       // [OutputCache(Duration = 60)]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var model = GetInvoiceTempModel(userId);
            
            
            return View(model);
        }

        
    }
    }
