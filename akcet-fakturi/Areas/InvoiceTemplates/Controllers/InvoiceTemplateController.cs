using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using akcetDB;
using akcet_fakturi.Areas.InvoiceTemplates.Models;
using akcet_fakturi.Models;
using Microsoft.AspNet.Identity;

namespace akcet_fakturi.Areas.InvoiceTemplates.Controllers
{
    public class InvoiceTemplateController : Controller
    {

        private AkcetModel db = new AkcetModel();
        private ApplicationDbContext dbUser = new ApplicationDbContext();
        // GET: InvoiceTemplates/InvoiceTemplate
       // [OutputCache(Duration = 60)]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var model = new InvoiceTemplateModels();
            var tblFakturiTemps = db.FakturiTemps.Where(s => s.UserId == userId).OrderByDescending(x => x.DateCreated).FirstOrDefault();

            if (tblFakturiTemps != null)
            {
                model.InvoiceEndDate = tblFakturiTemps.InvoiceEndDate;
                model.InvoiceDate = tblFakturiTemps.InvoiceDate;
            }
            var firstOrDefault = db.Counters.FirstOrDefault(c => c.Year == DateTime.Now.Year.ToString());
            if (firstOrDefault != null)
                model.InvoiceNumber =
                    firstOrDefault.CounterValue.ToString();

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
            model.CompanyName = company.CompanyName;
            model.CompanyAddress = company.Address.StreetName;
            model.CompanyDDSNumber = company.DdsNumber;

            var total = 0;
            total = model.ProductsListTemp.Sum(prize => Int32.Parse(prize.ProductPrice.ToString()) * Int32.Parse(prize.Quanity.ToString()));
            model.TotalWithoutDDS = total;

            
            return View(model);
        }

    }
    }
