using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using akcetDB;
using akcet_fakturi.Areas.InvoiceTemplates.Models;
using akcet_fakturi.Models;
using HtmlAgilityPack;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Kendo.Mvc.Extensions;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace akcet_fakturi.Controllers
{
    public class HomeController : BaseController
    {
        private AkcetModel db = new AkcetModel();
        public ActionResult Index()
        {

            return View();
        }

        #region Testing

        public ActionResult TestPage()
        {
            var userId = User.Identity.GetUserId();

            ViewBag.Html = db.Fakturis.OrderByDescending(o => o.DateCreated).Where(u => u.UserID == userId).FirstOrDefault().FakturaHtml;

            return View();
        }
        #endregion

        [HttpPost]
        public ActionResult Index(ContactFormModel Model)
        {

            if (!ModelState.IsValid)
                return View(Model);

            TempData["MessageIsSent"] = "Съобщението е изпратено успешно.";
            return View(Model);
        }
        [Authorize]
        public ActionResult Invoices()
        {
            ViewBag.formNumber = 1;
            var userId = User.Identity.GetUserId();
            string error = "";

            if(!CheckUserDetails(userId, out error))
                TempData["ResultError"] = error;

            ViewBag.IdAddress = new SelectList(db.Addresses.Where(a => a.UserName == User.Identity.Name), "IdAddress", "StreetName");
            ViewBag.Dds = new SelectList(db.DDS, "DdsId", "DdsName");
            ViewBag.Companies = new SelectList(db.Companies.Where(m => m.UserId == userId), "CompanyID", "CompanyName");
            ViewBag.Projects = new SelectList(db.Projects.Where(m => m.UserID == userId), "ProjectID", "ProjectName");
            ViewBag.Products = new SelectList(db.Products.Where(p => p.UserId == userId), "ProductID", "ProductName");
            return View();
        }

        public ActionResult CreateCompanyAjax([Bind(Exclude = "UserId", Include = "CompanyID,UserId,IdAddress,CompanyName,CompanyMol,DdsNumber,CompanyDescription,CompanyPhone,IsPrimary,DateCreated,DateModified")] Company company)
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
            if (company.IdAddress == 0)
            {
                TempData["ResultError"] = "Не сте избрали адрес на фирмата!";
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

        public JsonResult CreateAddressAjax(Address modelAddress)
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
                address.UserID = User.Identity.GetUserId();

                context.Addresses.Add(address);

                context.SaveChanges();

                TempData["ResultSuccess"] = "Успешно добавихте адрес!";

                return Json(new { id = address.IdAddress, value = address.StreetName });

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

        public JsonResult CreateInvoiceAjax(string Companies, FakturiTemp Fakturi)
        {
            if (Companies == "0")
                return Json(false);

            if (String.IsNullOrWhiteSpace(Fakturi.InvoiceDate))
                return Json(false);

            if (String.IsNullOrWhiteSpace(Fakturi.InvoiceEndDate))
                return Json(false);

            Fakturi.CompanyID = Int32.Parse(Companies);

            var userId = User.Identity.GetUserId();

            using (var context = new AkcetModel())
            {
                var temp = new List<ProductInvoiceTemp>();
                Fakturi.ProductInvoiceTemps = temp;
                Fakturi.UserId = userId;
                Fakturi.DateCreated = DateTime.Now;
                Fakturi.DateModified = DateTime.Now;
                Fakturi.UserName = User.Identity.Name;
                context.FakturiTemps.Add(Fakturi);
                context.SaveChanges();

            }
            return Json(Fakturi);
        }

        public ActionResult SaveProductAjax(string Products, string Dds, string Projects, string ProductPrice, string Quanity)
        {
            if (String.IsNullOrWhiteSpace(Products))
                return Json(false);
            if (String.IsNullOrWhiteSpace(ProductPrice))
                return Json(false);
            if (String.IsNullOrWhiteSpace(Quanity))
                return Json(false);

            //TODO: If ProductPrice has ',' do something
            //TODO: If Quantity has ',' do something

            var userId = User.Identity.GetUserId();
            ViewBag.Dds = new SelectList(db.DDS, "DdsId", "DdsName");
            ViewBag.Products = new SelectList(db.Products.Where(p => p.UserId == userId), "ProductID", "ProductName");
            ViewBag.Projects = new SelectList(db.Projects.Where(m => m.UserID == userId), "ProjectID", "ProjectName");
            ViewBag.IsInsertedProduct = true;


            var model = new ProductInvoiceTemp();
            var firstOrDefault = db.FakturiTemps.Where(s => s.UserId == userId).OrderByDescending(x => x.DateCreated).FirstOrDefault();
            if (firstOrDefault != null)
            {
               // var orDefault = db.DDS.FirstOrDefault(s => s.Value == Dds);
               // if (orDefault != null)
             //   {
                    var tbl = new ProductInvoiceTemp()
                    {
                        InvoiceIDTemp = firstOrDefault.InvoiceIDTemp,
                        DdsID = Int32.Parse(Dds),
                        ProjectID = Int32.Parse(Projects),
                        ProductID = Int32.Parse(Products),
                        ProductPrice = Decimal.Parse(ProductPrice, CultureInfo.InvariantCulture),
                        Quanity = Decimal.Parse(Quanity, CultureInfo.InvariantCulture)
                    };
                    using (var context = new AkcetModel())
                    {
                        db.ProductInvoiceTemps.Add(tbl);
                        db.SaveChanges();
                    }

                    model.ProductInvoiceID = tbl.ProductInvoiceID;
             //   }
            }

            //  model.ProductInvoiceID = Int32.Parse(Products);
            return PartialView("~/Views/Shared/InvoicesPartials/_TabProductsPartial.cshtml", model);
        }

        public ActionResult DeleteProductInvoiceTemp(int id)
        {
            //var userId = User.Identity.GetUserId();
            //var invoiceId = db.FakturiTemps.Where(s => s.UserId == userId).OrderByDescending(x => x.DateCreated).FirstOrDefault().InvoiceIDTemp;

            var product = db.ProductInvoiceTemps.Find(id);

            db.ProductInvoiceTemps.Remove(product);
            db.SaveChanges();

            return Json(id, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateProjectAjax(Project modelProject)
        {
            ModelState.Remove("UserID");

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
                modelProject.DateCreated = DateTime.Now;
                modelProject.DateModified = DateTime.Now;
                modelProject.UserID = User.Identity.GetUserId();
                modelProject.UserName = User.Identity.Name;

                db.Projects.Add(modelProject);
                db.SaveChanges();


                return Json(new { id = modelProject.ProjectID, value = modelProject.ProjectName });
            }
        }

        public string RenderViewToString(string templateName, object model)
        {
            templateName = "~/Areas/InvoiceTemplates/Views/InvoiceTemplate/" + templateName + ".cshtml";

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
                SendExceptionToAdmin(ex);
                TempData["ResultErrors"] = "There was a problem with rendering template for email!";
                return "Error in register form! Email with the problem was send to aministrator.";
            }
        }

        public JsonResult SaveInvoiceConfirmed(bool value)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var model = GetInvoiceTempModel(userId);

                // TODO: Set Counter for each user for every year 

                var tempCounter =
                    db.Counters.Where(c => c.UserID == userId && c.Year == DateTime.Now.Year.ToString())
                        .FirstOrDefault();
                tempCounter.CounterValue++;


                //var counter = db.Counters.OrderByDescending(s => s.CounterValue).FirstOrDefault(c => c.Year == DateTime.Now.Year.ToString());
                //counter.CounterValue++;
                //db.Counters.Add(counter);
                // TODO: Set Counter for each user for every year 


                var faktura = new Fakturi();
                faktura.CompanyID = model.CompanyID;
                faktura.InvoiceDate = DateTime.Parse(model.InvoiceDate);
                faktura.InvoiceEndDate = DateTime.Parse(model.InvoiceEndDate);

                faktura.TotalPrice = model.TotalWithDDS;
                faktura.Period = model.Period ?? " ";
                faktura.FakturaNumber = model.InvoiceNumber;
                faktura.FakturaHtml = RenderViewToString("Index", model);
                faktura.UserID = userId;
                faktura.UserName = User.Identity.Name;
                faktura.DateCreated = DateTime.Now;
                faktura.DateModified = DateTime.Now;
                db.Fakturis.Add(faktura);

                var products = new List<ProductInvoice>();
                foreach (var productTemp in model.ProductsListTemp)
                {
                    products.Add(new ProductInvoice()
                    {
                        InvoiceID = faktura.InvoiceID,
                        DdsID = productTemp.DdsID,
                        ProductID = productTemp.ProductID,
                        ProjectID = productTemp.ProjectID,
                        Quantity = productTemp.Quanity,
                        TotalPrice = productTemp.ProductTotalPrice
                    });
                }
                products.ForEach(s => db.ProductInvoices.Add(s));
                db.SaveChanges();
                return Json(faktura.FakturaHtml, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SendExceptionToAdmin(ex);
                return Json(false, JsonRequestBehavior.AllowGet);

            }

        }


        public ActionResult SendIvoiceToEmail(string EmailReciever)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(EmailReciever);
                if (addr.Address != EmailReciever)
                    return Json(false, JsonRequestBehavior.AllowGet);
                var userID = User.Identity.GetUserId();
                var html =
                    db.Fakturis.OrderByDescending(o => o.DateCreated)
                        .Where(s => s.UserID == userID)
                        .FirstOrDefault()
                        .FakturaHtml;


                var model = new akcet_fakturi.Areas.EmailTemplates.Models.InvoiceTemplate();

                model.html = html;

                var emailBody = RenderEmailViewToString("InvoiceEmailTemplate", model);
                //    emailBody = emailBody.Replace("\r\n", "");
                if (!SendEmail(EmailReciever, "Нова фактура", emailBody))
                    return Json(false, JsonRequestBehavior.AllowGet);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadInvoice(int? idInvoice)
        {
            //TODO: Generate PDF from invoice HTML 

            var invoiceId = idInvoice ?? 0;
            var html = "";
            var userId = User.Identity.GetUserId();
            if (invoiceId == 0)
            {
                html = db.Fakturis.OrderByDescending(o => o.DateCreated).Where(u => u.UserID == userId).FirstOrDefault().FakturaHtml;
            }
            html = html.Replace("\r\n", string.Empty);
            MemoryStream msOutput = new MemoryStream();
            TextReader reader = new StringReader(html);
            try
            {
                Document document = new Document(PageSize.A4, 30, 30, 30, 30);

                PdfWriter writer = PdfWriter.GetInstance(document, msOutput);
                var worker = new HTMLWorker(document);
                document.Open();
                worker.StartDocument();
                worker.Parse(reader);
                worker.EndDocument();

                worker.Close();
                document.Close();

                Response.Buffer = false;
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                //Set the appropriate ContentType.
                Response.ContentType = "Application/pdf";
                //Write the file content directly to the HTTP content output stream.
                Response.BinaryWrite(msOutput.ToArray());
                Response.Flush();
                Response.End();
                return File(msOutput, "application/pdf", DateTime.Now.ToString(CultureInfo.InvariantCulture));

            }
            catch (Exception ex)
            {
                SendExceptionToAdmin(ex);
                return Json(false);
            }
        }
    }
}