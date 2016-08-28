using akcet_fakturi.Controllers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace akcet_fakturi.Areas.WerkbriefTemplates.Controllers
{
    public class WerkbriefTemplateController : BaseController
    {
        // GET: WerkbriefTemplates/WerkbriefTemplate
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var model = GetWerkbriefTempModel(userId);
            return View(model);
        }
    }
}