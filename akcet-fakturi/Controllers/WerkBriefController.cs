using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;
using akcetDB;
using System.IO;
using Tools;
using System.Globalization;

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

            var weeks = FetchWeeks(DateTime.Now.Year);
            ViewBag.Weeks = new SelectList(weeks);

            ViewBag.Products = new SelectList(db.Products.Where(p => p.UserId == userId && p.IsDeleted == false), "ProductID", "ProductName");
            ViewBag.IdAddress = new SelectList(db.Addresses.Where(a => a.UserName == User.Identity.Name), "IdAddress", "StreetName");
            ViewBag.Companies = new SelectList(db.Companies.Where(m => m.UserId == userId && m.IsDeleted == false), "CompanyID", "CompanyName");
            ViewBag.Projects = new SelectList(db.Projects.Where(m => m.UserID == userId && m.IsDeleted == false), "ProjectID", "ProjectName");
            return View();
        }

        // Step 1
        public JsonResult CreateWerkbriefAjax(string Companies, WerkbriefTemp Werkbrief)
        {
            if (Companies == "0")
                return Json(false);

            //if (String.IsNullOrWhiteSpace(Werkbrief.WerkbriefDate))
            //    return Json(false);

            //if (String.IsNullOrWhiteSpace(Werkbrief.WerkbriefEndDate))
            //    return Json(false);

            Werkbrief.CompanyID = Int32.Parse(Companies);

            var userId = User.Identity.GetUserId();

            var temp = new List<WerkbriefHoursTemp>();
            Werkbrief.WerkbriefHoursTemps = temp;
            Werkbrief.UserId = userId;
            Werkbrief.DateCreated = DateTime.Now;
            Werkbrief.DateModified = DateTime.Now;
            Werkbrief.UserName = User.Identity.Name;
            db.WerkbriefTemps.Add(Werkbrief);
            db.SaveChanges();


            return Json(Werkbrief);
        }


        // Step 2
        public ActionResult SaveWerkbriefAjax(string Products, string Projects, WerkbriefHoursTemp Model)
        {
            var model = new WerkbriefHoursTemp();

            var userId = User.Identity.GetUserId();
            var weeks = FetchWeeks(DateTime.Now.Year);
            ViewBag.Weeks = new SelectList(weeks);

            ViewBag.Products = new SelectList(db.Products.Where(p => p.UserId == userId && p.IsDeleted == false), "ProductID", "ProductName");
            ViewBag.IdAddress = new SelectList(db.Addresses.Where(a => a.UserName == User.Identity.Name), "IdAddress", "StreetName");
            ViewBag.Companies = new SelectList(db.Companies.Where(m => m.UserId == userId && m.IsDeleted == false), "CompanyID", "CompanyName");
            ViewBag.Projects = new SelectList(db.Projects.Where(m => m.UserID == userId && m.IsDeleted == false), "ProjectID", "ProjectName");
            //  model.ProductInvoiceID = Int32.Parse(Products);

            var werkbriefTempId = db.WerkbriefTemps.Where(w => w.UserId == userId).OrderByDescending(w => w.DateCreated).FirstOrDefault().WerkbriefIDTemp;
            Model.TotalHours = GetTotalHours(Model).ToString();
            Model.ProductID = Int32.Parse(Products);
            Model.WerkbriefIDTemp = werkbriefTempId;
            Model.ProjectID = Int32.Parse(Projects);
            db.WerkbriefHoursTemps.Add(Model);
            db.SaveChanges();
            return PartialView("~/Views/Shared/WerkbriefsPartials/_TabWerkbriefsPartial.cshtml", Model);
        }


        private decimal GetTotalHours(WerkbriefHoursTemp model)
        {
            var temp = decimal.Zero;
            var total = decimal.Zero;

            if (model.Monday != null && model.Monday.Contains('.'))
                model.Monday = model.Monday.Replace('.', ',');
            decimal.TryParse(model.Monday, out temp);
            total += temp;

            if (model.Tuesday != null && model.Tuesday.Contains('.'))
                model.Tuesday = model.Tuesday.Replace('.', ',');
            decimal.TryParse(model.Tuesday, out temp);
            total += temp;

            if (model.Wednesday != null && model.Wednesday.Contains('.'))
                model.Wednesday = model.Wednesday.Replace('.', ',');
            decimal.TryParse(model.Wednesday, out temp);
            total += temp;

            if (model.Thursday != null && model.Thursday.Contains('.'))
                model.Thursday = model.Thursday.Replace('.', ',');
            decimal.TryParse(model.Thursday, out temp);
            total += temp;

            if (model.Friday != null && model.Friday.Contains('.'))
                model.Friday = model.Friday.Replace('.', ',');
            decimal.TryParse(model.Friday, out temp);
            total += temp;

            if (model.Saturday != null && model.Saturday.Contains('.'))
                model.Saturday = model.Saturday.Replace('.', ',');
            decimal.TryParse(model.Saturday, out temp);
            total += temp;

            if (model.Sunday != null && model.Sunday.Contains('.'))
                model.Sunday = model.Sunday.Replace('.', ',');
            decimal.TryParse(model.Sunday, out temp);
            total += temp;

            return total;
        }


        public ActionResult SaveWerkbriefConfirmed(bool value)
        {
            var userId = User.Identity.GetUserId();
            var model = GetWerkbriefTempModel(userId);

            var werkbrief = new Werkbrief();
            werkbrief.CompanyID = model.CompanyID;
            werkbrief.Period = model.Period;
            werkbrief.UserId = userId;

            werkbrief.DateCreated = DateTime.Now;
            werkbrief.DateModified = DateTime.Now;
            werkbrief.UserName = User.Identity.Name;
            werkbrief.WerkbriefHTML = RenderViewToString("index", model);
            var totalHours = Decimal.Zero;

            var werkbriefHours = new List<WerkbriefHours>();
            foreach (var hours in model.WerkbriefHoursTemps)
            {
                totalHours += GetTotalHours(hours);
                werkbriefHours.Add(new WerkbriefHours()
                {
                    ProductID = hours.ProductID,
                    ProjectID = hours.ProjectID,
                    TotalHours = hours.TotalHours,
                    Week = hours.Week,
                    Monday = hours.Monday,
                    Tuesday = hours.Tuesday,
                    Wednesday = hours.Wednesday,
                    Thursday = hours.Thursday,
                    Friday = hours.Friday,
                    Saturday = hours.Saturday,
                    Sunday = hours.Sunday
                });
            }
            werkbrief.TotalHours = totalHours.ToString();
            db.Werkbriefs.Add(werkbrief);

            werkbriefHours.ForEach(w => db.WerkbriefHours.Add(w));
            db.SaveChanges();


            return Json(werkbrief.WerkbriefHTML, JsonRequestBehavior.AllowGet);
        }
        public string RenderViewToString(string templateName, object model)
        {
            templateName = "~/Areas/WerkbriefTemplates/Views/WerkbriefTemplate/" + templateName + ".cshtml";

            //TODO: Make enumeration for variable templateName. 

            ViewData.Model = model;

            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindView(ControllerContext, templateName, null);
                    ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                    viewResult.View.Render(viewContext, sw);
                    viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);

                    return sw.ToString();
                }
            }
            catch (Exception ex)
            {
                EmailFunctions.SendExceptionToAdmin(ex);
                TempData["ResultErrors"] = "There was a problem with rendering template for email!";
                return "Error in register form! Email with the problem was send to aministrator.";
            }
        }


        public ActionResult DeleteWerkbriefHoursTempTemp(int id)
        {
            //var userId = User.Identity.GetUserId();
            //var invoiceId = db.WerkbriefTemps.Where(s => s.UserId == userId).OrderByDescending(x => x.DateCreated).FirstOrDefault().WerkbriefIDTemp;

            var product = db.WerkbriefHoursTemps.Find(id);

            db.WerkbriefHoursTemps.Remove(product);
            db.SaveChanges();

            return Json(id, JsonRequestBehavior.AllowGet);
        }




        public ActionResult SendWerkbriefToEmail(string EmailReciever)
        {
            try
            {
                string strEmailResult = "<img src=\"www.fakturi.nl/images/logo.png\">";


                var strResult = "";
                var userId = User.Identity.GetUserId();

                strResult = db.Fakturis.OrderByDescending(o => o.DateCreated).Where(u => u.UserID == userId).FirstOrDefault().FakturaHtml;

                strResult = strResult.Replace("\r\n", string.Empty);
                byte[] bytes = GeneratePDF(strResult);

                EmailFunctions.SendEmail(EmailReciever, "Werkbrief", strEmailResult, bytes, DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");

                return Json(true, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadWerkbrief(int? idWerkbrief)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                var strResult = db.Werkbriefs.OrderByDescending(o => o.DateCreated).Where(u => u.UserId == userId).FirstOrDefault().WerkbriefHTML;

                byte[] bytes = GeneratePDF(strResult);

                Response.Buffer = false;
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                //Set the appropriate ContentType.
                Response.ContentType = "Application/pdf";
                //Write the file content directly to the HTTP content output stream.
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
                return File(bytes, "application/pdf", DateTime.Now.ToString(CultureInfo.InvariantCulture));

            }
            catch (Exception ex)
            {
                EmailFunctions.SendExceptionToAdmin(ex);
                return Json(false);
            }
        }
    }
}