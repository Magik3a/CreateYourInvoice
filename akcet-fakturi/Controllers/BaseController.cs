using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using akcetDB;
using akcet_fakturi.Areas.InvoiceTemplates.Models;
using akcet_fakturi.Models;

namespace akcet_fakturi.Controllers
{
    public class BaseController : Controller
    {

        private AkcetModel db = new AkcetModel();
        private ApplicationDbContext dbUser = new ApplicationDbContext();

        [ChildActionOnly]
        [OutputCache(Duration = 2 * 60)]
        public InvoiceTemplateModels GetInvoiceTempModel(string userId)
        {
            var model = new InvoiceTemplateModels();
            var tblFakturiTemps = db.FakturiTemps.Where(s => s.UserId == userId).OrderByDescending(x => x.DateCreated).FirstOrDefault();

            if (tblFakturiTemps != null)
            {
                model.InvoiceEndDate = tblFakturiTemps.InvoiceEndDate;
                model.InvoiceDate = tblFakturiTemps.InvoiceDate;
            }
            var firstOrDefault = db.Counters.OrderByDescending(s => s.CounterValue).FirstOrDefault(c => c.Year == DateTime.Now.Year.ToString());
            if (firstOrDefault != null)
                model.InvoiceNumber = String.Format("{0}-{1:D6}", DateTime.Now.Year, firstOrDefault.CounterValue);

            var productsListTemp = new List<ProductInvoiceTemp>();

            productsListTemp =
                db.ProductInvoiceTemps.Where(p => p.InvoiceIDTemp == tblFakturiTemps.InvoiceIDTemp).ToList();

            var ddsList = new List<DD>();
            ddsList = db.DDS.ToList();
            model.ListDds = ddsList;
            model.ProductsListTemp = productsListTemp;


            var user = dbUser.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            model.UserAddress = user.Address;
            model.UserBankAccount = user.BankAcount;
            model.UserBulstat = user.KwkNumber;
            model.UserCompanyName = user.CompanyName;
            model.UserPhone = user.PhoneNumber;
            model.UserDDsNumber = user.DdsNumber;

            var company = db.Companies.FirstOrDefault(c => c.CompanyID == tblFakturiTemps.CompanyID);
            model.CompanyID = tblFakturiTemps.CompanyID??0;
            model.CompanyName = company.CompanyName;
            model.CompanyAddress = company.Address.StreetName;
            model.CompanyDDSNumber = company.DdsNumber;

            var total = 0;
            total = model.ProductsListTemp.Sum(prize => Int32.Parse(prize.ProductPrice.ToString()) * Int32.Parse(prize.Quanity.ToString()));
            model.TotalWithoutDDS = total;
            decimal total2 = Decimal.Zero;
          //  model.ProductsListTemp.ForEach(p => total2 += Decimal.Parse(((p.ProductPrice??0 * p.Quanity??0) * (GetValueDDS(p.DdsID) / 100) ).ToString()));

            foreach (var product  in model.ProductsListTemp)
            {
                var totalPriceProduct = Decimal.Parse(((product.ProductPrice??0 * product.Quanity??0) * (GetValueDDS(product.DdsID) / 100)).ToString());
                
            }

            return model;
        }


        private int GetValueDDS(int? ddsId)
        {
            var ddsValue = Int32.Parse(db.DDS.FirstOrDefault(m => m.DdsID == ddsId).Value);
            return ddsValue;
        }

        [ChildActionOnly]
        [OutputCache(Duration = 1 * 60)]
        public string RenderViewToString(string templateName, object model)
        {
            templateName = "~/Areas/InvoiceTemplates/Views/InvoiceTemplate/" + templateName + ".cshtml";
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