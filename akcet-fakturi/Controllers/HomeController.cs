using System;
using System.Collections.Generic;
using System.IO;
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
        private AkcetModel db = new AkcetModel();
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Index(ContactFormModel Model)
        {

            if(!ModelState.IsValid)
                return View(Model);

            TempData["MessageIsSent"] = "Съобщението е изпратено успешно.";
            return View(Model);
        }
        [Authorize]
        public ActionResult Invoices()
        {
            ViewBag.formNumber = 1;
            var userId = User.Identity.GetUserId();
            ViewBag.IdAddress = new SelectList(db.Addresses, "IdAddress", "StreetName");
            ViewBag.Dds = new SelectList(db.DDS, "Value", "DdsName");
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

        public JsonResult CreateInvoiceAjax(string Companies, FakturiTemp Fakturi)
        {
            if (Companies == "0")
                return Json(false);

            if(String.IsNullOrWhiteSpace(Fakturi.InvoiceDate))
                return Json(false);

            if (String.IsNullOrWhiteSpace(Fakturi.InvoiceEndDate))
                return Json(false);

            //if (String.IsNullOrWhiteSpace(Fakturi.Period))
            //    return Json(false);

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

        public ActionResult SaveProductAjax(string Products,string Dds, string Projects, ProductInvoiceTemp modelProductInvoiceTemp)
        {
            if(String.IsNullOrWhiteSpace(Products))
                return Json(false);
            
           var userId = User.Identity.GetUserId();
            ViewBag.Dds = new SelectList(db.DDS, "Value", "DdsName");
            ViewBag.Products = new SelectList(db.Products.Where(p => p.UserId == userId), "ProductID", "ProductName");
            ViewBag.Projects = new SelectList(db.Projects.Where(m => m.UserID == userId), "ProjectID", "ProjectName");
            ViewBag.IsInsertedProduct = true;


            var firstOrDefault = db.FakturiTemps.Where(s => s.UserId == userId).OrderByDescending(x=>x.DateCreated).FirstOrDefault();
            if (firstOrDefault != null)
            {
                var orDefault = db.DDS.FirstOrDefault(s => s.Value == Dds);
                if (orDefault != null)
                {
                    var tbl = new ProductInvoiceTemp()
                    {
                        InvoiceIDTemp = firstOrDefault.InvoiceIDTemp,
                        DdsID = orDefault.DdsID,
                        ProjectID = Int32.Parse(Projects),
                        ProductID = Int32.Parse(Products),
                        ProductPrice = modelProductInvoiceTemp.ProductPrice,
                        Quanity = modelProductInvoiceTemp.Quanity
                    };
                    using (var context = new AkcetModel())
                    {
                        db.ProductInvoiceTemps.Add(tbl);
                        db.SaveChanges();
                    }
                }
            }
            var model = new ProductInvoiceTemp();

            model.ProductInvoiceID = Int32.Parse(Products);
            return PartialView("~/Views/Shared/InvoicesPartials/_TabProductsPartial.cshtml", model);
        }

        public ActionResult DeleteProductInvoiceTemp(int id)
        {
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
            templateName = "~/Areas/InvoiceTemplates/Views/Email/" + templateName + ".cshtml";
            // var controller = new EmailController();

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
                TempData["ResultErrors"] = "There was a problem with rendering template for email!";
                return "Error in register form! Email with the problem was send to aministrator.";
            }
        }
    }
}