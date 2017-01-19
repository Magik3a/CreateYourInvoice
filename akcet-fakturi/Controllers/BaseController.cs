using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using akcetDB;
using akcet_fakturi.Areas.InvoiceTemplates.Models;
using akcet_fakturi.Models;
using HtmlAgilityPack;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using Tools;
using Data;
using akcet_fakturi.Areas.WerkbriefTemplates.Models;

namespace akcet_fakturi.Controllers
{
    public class BaseController : Controller
    {

        private AkcetModel db = new AkcetModel();
        private AppDbContext dbUser = new AppDbContext();

        public List<string> FetchWeeks(int year)
        {
            List<string> weeks = new List<string>();
            DateTime startDate = new DateTime(year, 1, 1);
            startDate = startDate.AddDays(1 - (int)startDate.DayOfWeek);
            DateTime endDate = startDate.AddDays(6);
            while (startDate.Year < 1 + year)
            {
                weeks.Add(string.Format("От {0:dd/MM/yyyy} До {1:dd/MM/yyyy}", startDate, endDate));
                startDate = startDate.AddDays(7);
                endDate = endDate.AddDays(7);
            }
            return weeks;
        }

        public string RenderEmailBodyView(string templateName, object model)
        {
            templateName = "~/Areas/EmailTemplates/Views/Template/" + templateName + ".cshtml";

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

        public bool CheckUserDetails(string UserId, out string Error)
        {
            Error = " ";

            var user = dbUser.Users.Find(UserId);

            if (String.IsNullOrWhiteSpace(user.City))
                Error = "Не сте въвели град в профила ви.";

            if (String.IsNullOrWhiteSpace(user.Address))
                Error = "Не сте въвели адрес в профила ви.";

            if (String.IsNullOrWhiteSpace(user.ZipCode))
                Error = "Не сте въвели пощенски код в профила ви.";

            if (String.IsNullOrWhiteSpace(user.CompanyName))
                Error = "Не сте въвели име на компания в профила ви.";

            if (String.IsNullOrWhiteSpace(user.BankAcount))
                Error = "Не сте въвели банкова сметка в профила ви.";

            if (String.IsNullOrWhiteSpace(user.DdsNumber))
                Error = "Не сте въвели ддс номер в профила ви.";

            if (String.IsNullOrWhiteSpace(user.KwkNumber))
                Error = "Не сте въвели квк номер в профила ви.";

            return (string.IsNullOrWhiteSpace(Error));
        }


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
            var counter = db.Counters.OrderByDescending(s => s.CounterValue)
                .FirstOrDefault(c => c.Year == DateTime.Now.Year.ToString() && c.UserID == userId);

            if (counter != null)
            {
                model.InvoiceNumber = $"{DateTime.Now.Year}-{counter?.CounterValue:D3}";
            }
            else
            {
                var tempCounter = new Counter();
                tempCounter.CounterValue = 1;
                tempCounter.UserID = userId;
                tempCounter.Year = DateTime.Now.Year.ToString();
                db.Counters.Add(tempCounter);
                db.SaveChanges();
                model.InvoiceNumber = $"{DateTime.Now.Year}-{1:D3}";
            }




            // var firstOrDefault = db.Counters.OrderByDescending(s => s.CounterValue).FirstOrDefault(c => c.Year == DateTime.Now.Year.ToString());
            //  if (firstOrDefault != null)
            //    model.InvoiceNumber = String.Format("{0}-{1:D6}", DateTime.Now.Year, firstOrDefault.CounterValue);

            //var productsListTemp = new List<ProductInvoiceTemp>();

            // var productsListTemp = db.ProductInvoiceTemps.Where(p => p.InvoiceIDTemp == tblFakturiTemps.InvoiceIDTemp).ToList();

            var ddsList = new List<DD>();
            ddsList = db.DDS.ToList();
            model.ListDds = ddsList;
            model.ProductsListTemp = db.ProductInvoiceTemps.Where(p => p.InvoiceIDTemp == tblFakturiTemps.InvoiceIDTemp).ToList();


            var user = dbUser.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            model.UserAddress = String.Format("{0}, {1}, {2}", user.Address, user.ZipCode, user.City);
            model.UserBankAccount = user.BankAcount;
            model.UserBulstat = user.KwkNumber;
            model.UserCompanyName = user.CompanyName;
            model.UserPhone = user.PhoneNumber;
            model.UserDDsNumber = user.DdsNumber;

            var company = db.Companies.FirstOrDefault(c => c.CompanyID == tblFakturiTemps.CompanyID);
            model.CompanyID = tblFakturiTemps.CompanyID ?? 0;
            model.CompanyName = company.CompanyName;
            model.CompanyAddress = String.Format("{0}, {1}, {2}", company.Address.StreetName, company.Address.ZipCode, company.Address.City);
            model.CompanyDDSNumber = company.DdsNumber;

            model.Period = tblFakturiTemps.Period;
            var total = Decimal.Zero;
            total = model.ProductsListTemp.Sum(prize => (prize.ProductPrice * prize.Quanity));
            model.TotalWithoutDDS = total;

            //  model.ProductsListTemp.ForEach(p => total2 += Decimal.Parse(((p.ProductPrice??0 * p.Quanity??0) * (GetValueDDS(p.DdsID) / 100) ).ToString()));

            foreach (var product in model.ProductsListTemp)
            {
                var ddsValue = GetValueDDS(product.DdsID);
                product.ProductTotalPrice = ((product.ProductPrice * product.Quanity) * (ddsValue / 100));
                model.TotalWithDDS += product.ProductTotalPrice + (product.ProductPrice * product.Quanity);
                model.TotalDDS += product.ProductTotalPrice;
            }

            return model;
        }


        [ChildActionOnly]
        [OutputCache(Duration = 2 * 60)]
        public WerkbriefTemplateViewModel GetWerkbriefTempModel(string userId)
        {
            var model = new WerkbriefTemplateViewModel();
            var werkbriefTemps = dbUser.WerkbriefTemps.Where(s => s.UserId == userId).OrderByDescending(x => x.DateCreated).FirstOrDefault();


            model.WerkbriefHoursTemps = dbUser.WerkbriefHoursTemps.Where(p => p.WerkbriefIDTemp == werkbriefTemps.WerkbriefIDTemp).ToList();


            var user = dbUser.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            model.UserAddress = String.Format("{0}, {1}, {2}", user.Address, user.ZipCode, user.City);
            model.UserBankAccount = user.BankAcount;
            model.UserBulstat = user.KwkNumber;
            model.UserCompanyName = user.CompanyName;
            model.UserPhone = user.PhoneNumber;
            model.UserDDsNumber = user.DdsNumber;

            var company = db.Companies.FirstOrDefault(c => c.CompanyID == werkbriefTemps.CompanyID);
            model.CompanyID = werkbriefTemps.CompanyID ?? 0;
            model.CompanyName = company.CompanyName;
            model.CompanyAddress = String.Format("{0}, {1}, {2}", company.Address.StreetName, company.Address.ZipCode, company.Address.City);
            model.CompanyDDSNumber = company.DdsNumber;

            model.Period = werkbriefTemps.Period;

            return model;
        }

        private decimal GetValueDDS(int? ddsId)
        {
            var dds = db.DDS.FirstOrDefault(m => m.DdsID == ddsId);
            if (dds.IsNullValue)
                return decimal.Zero;
            else
                return Decimal.Parse(dds.Value);
        }



        public byte[] GeneratePDF(string html)
        {
            #region Generate PDF
            Byte[] bytes;
            var ms = new MemoryStream();
            //Create an iTextSharp Document wich is an abstraction of a PDF but **NOT** a PDF
            var doc = new Document();
            var writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();
            doc.NewPage();
            var hDocument = new HtmlDocument()
            {
                OptionWriteEmptyNodes = true,
                OptionAutoCloseOnEnd = true
            };

            hDocument.LoadHtml(html);
            var footer = hDocument.GetElementbyId("footer").InnerText.Trim();
            hDocument.GetElementbyId("footer").Remove();
            var closedTags = hDocument.DocumentNode.WriteTo();

            var example_html = closedTags;
            var example_css = System.IO.File.ReadAllText(Server.MapPath("~/Content/invoicePrint.css"));
            var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css));
            var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html));
            string fontPath = Server.MapPath("~/fonts/arialuni.ttf");
            iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss, Encoding.UTF8, new UnicodeFontFactory(fontPath));
            writer.PageEvent = new MyFooterEvent(footer.ToString());
            doc.Close();
            bytes = ms.ToArray();

            //var testFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf");
            //  System.IO.File.WriteAllBytes(testFile, bytes);
            #endregion

            return bytes;
        }

        class MyFooterEvent : PdfPageEventHelper
        {
            private string footer;
            public MyFooterEvent(string footer)
            {
                this.footer = footer;
            }
            Font FONT = new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD);

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                PdfContentByte canvas = writer.DirectContent;
                //ColumnText.ShowTextAligned(
                //  canvas, Element.ALIGN_LEFT,
                //  new Phrase("Header", FONT), 10, 810, 0
                ////);
                //ColumnText.ShowTextAligned(
                //  canvas, Element.ALIGN_LEFT,
                //  new Phrase(new Chunk(footer.Split(new string[] { "IBAN" }, StringSplitOptions.None)[0])), 20, 40, 0
                //);
                ColumnText.ShowTextAligned(
                  canvas, Element.ALIGN_LEFT,
                  new Phrase(new Chunk(footer)), 250, 25, 0
                );
            }
        }
        public class UnicodeFontFactory : FontFactoryImp, IUnicodeFontFactory
        {

            private readonly BaseFont _baseFont;

            public UnicodeFontFactory(string FontPath)
            {
                _baseFont = BaseFont.CreateFont(FontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            }

            public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color,
              bool cached)
            {
                return new Font(_baseFont, size, style, color);
            }
        }

    }
}