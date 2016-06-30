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

            //var productsListTemp = new List<ProductInvoiceTemp>();

            // var productsListTemp = db.ProductInvoiceTemps.Where(p => p.InvoiceIDTemp == tblFakturiTemps.InvoiceIDTemp).ToList();

            var ddsList = new List<DD>();
            ddsList = db.DDS.ToList();
            model.ListDds = ddsList;
            model.ProductsListTemp = db.ProductInvoiceTemps.Where(p => p.InvoiceIDTemp == tblFakturiTemps.InvoiceIDTemp).ToList();


            var user = dbUser.Users.FirstOrDefault(m => m.UserName == User.Identity.Name);
            model.UserAddress = user.Address;
            model.UserBankAccount = user.BankAcount;
            model.UserBulstat = user.KwkNumber;
            model.UserCompanyName = user.CompanyName;
            model.UserPhone = user.PhoneNumber;
            model.UserDDsNumber = user.DdsNumber;

            var company = db.Companies.FirstOrDefault(c => c.CompanyID == tblFakturiTemps.CompanyID);
            model.CompanyID = tblFakturiTemps.CompanyID ?? 0;
            model.CompanyName = company.CompanyName;
            model.CompanyAddress = company.Address.StreetName;
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


        private decimal GetValueDDS(int? ddsId)
        {
            var ddsValue = Decimal.Parse(db.DDS.FirstOrDefault(m => m.DdsID == ddsId).Value);
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

        public MemoryStream CreatePDF(string html)
        {
            html = html.Replace("\r\n", string.Empty);
            MemoryStream msOutput = new MemoryStream();
            TextReader reader = new StringReader(html);
            try
            {
                // step 1: creation of a document-object
                Document document = new Document(PageSize.A4, 30, 30, 30, 30);

                // step 2:
                // we create a writer that listens to the document
                // and directs a XML-stream to a file
                PdfWriter writer = PdfWriter.GetInstance(document, msOutput);

                // step 3: we create a worker parse the document
                var worker = new HTMLWorker(document);

                // step 4: we open document and start the worker on the document
                document.Open();
                //worker.StartDocument();

                // step 5: parse the html into the document
                worker.Parse(reader);

                // step 6: close the document and the worker
                worker.EndDocument();
             //   worker.Close();
              //  document.Close();
            }
            catch (Exception ex)
            {
               SendExceptionToAdmin(ex);
            }
            return msOutput;
        }

        // this assumes there is a MyPdf.cshtml/MyPdf.aspx as the view
        //return this.PdfFromHtml(myModel);
        public class PdfFromHtmlResult : ViewResult
        {
            public override void ExecuteResult(ControllerContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException("context");
                }
                if (string.IsNullOrEmpty(this.ViewName))
                {
                    this.ViewName = context.RouteData.GetRequiredString("action");
                }

                if (this.View == null)
                {
                    this.View = this.FindView(context).View;
                }

                // First get the html from the Html view
                using (var writer = new StringWriter())
                {
                    var vwContext = new ViewContext(context, this.View, this.ViewData, this.TempData, writer);
                    this.View.Render(vwContext, writer);

                    // Convert to pdf

                    var response = context.HttpContext.Response;

                    using (var pdfStream = new MemoryStream())
                    {
                        var pdfDoc = new Document();
                        var pdfWriter = PdfWriter.GetInstance(pdfDoc, pdfStream);

                        pdfDoc.Open();

                        using (var htmlRdr = new StringReader(writer.ToString()))
                        {

                            var parsed = HTMLWorker.ParseToList(htmlRdr, null);

                            foreach (var parsedElement in parsed)
                            {
                                pdfDoc.Add(parsedElement);
                            }
                        }

                        pdfDoc.Close();

                        response.ContentType = "application/pdf";
                        response.AddHeader("Content-Disposition", this.ViewName + ".pdf");
                        byte[] pdfBytes = pdfStream.ToArray();
                        response.OutputStream.Write(pdfBytes, 0, pdfBytes.Length);
                    }
                }
            }
        }

        [ChildActionOnly]
        public void SendEmail(string reciever, string subject, string body)
        {
            SmtpClient SmtpServer = new SmtpClient();
            MailMessage mail = new MailMessage();
            mail.To.Add(reciever);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpServer.Send(mail);
        }


        public void SendExceptionToAdmin(Exception ex)
        {
            string emailAdmin = ConfigurationManager.AppSettings["TechnicalSupportEmail"];
            var sb = new StringBuilder();
            sb.AppendLine("Message: ");
            sb.AppendLine(ex.Message.ToString());
            sb.AppendLine("================================");
            sb.AppendLine("Inner exception: ");
            sb.AppendLine(ex.InnerException.ToString());
            sb.AppendLine("================================");

            SmtpClient smtpServer = new SmtpClient();
            MailMessage mail = new MailMessage();
            mail.To.Add(emailAdmin);
            mail.Subject = "Exception occurred in fakturi.nl!";
            mail.Body = sb.ToString();
            smtpServer.Send(mail);
        }
    }
}