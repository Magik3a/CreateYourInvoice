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

namespace akcet_fakturi.Controllers
{
    public class BaseController : Controller
    {

        private AkcetModel db = new AkcetModel();
        private ApplicationDbContext dbUser = new ApplicationDbContext();

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

            if (db.Counters.Any(c => c.Year == DateTime.Now.Year.ToString()) && db.Counters.Any(c => c.UserID == userId))
            {
                var firstOrDefault = db.Counters.OrderByDescending(s => s.CounterValue)
                    .FirstOrDefault(c => c.Year == DateTime.Now.Year.ToString() && c.UserID == userId);
                model.InvoiceNumber = String.Format("{0}-{1:D3}", DateTime.Now.Year, firstOrDefault.CounterValue);

            }
            else
            {
                var tempCounter = new Counter();
                tempCounter.CounterValue = 1;
                tempCounter.UserID = userId;
                tempCounter.Year = DateTime.Now.Year.ToString();
                db.Counters.Add(tempCounter);
                db.SaveChanges();
                model.InvoiceNumber = String.Format("{0}-{1:D3}", DateTime.Now.Year, 1);
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


        private decimal GetValueDDS(int? ddsId)
        {
            var dds = db.DDS.FirstOrDefault(m => m.DdsID == ddsId);
            if (dds.IsNullValue)
                return decimal.Zero;
            else
                return Decimal.Parse(dds.Value);
        }

        [ChildActionOnly]
        public string RenderEmailViewToString(string templateName, object model)
        {
            templateName = "~/Areas/EmailTemplates/Views/Template/" + templateName + ".cshtml";
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
            var closedTags = hDocument.DocumentNode.WriteTo();
            var example_html = closedTags;
            var example_css = System.IO.File.ReadAllText(Server.MapPath("~/Content/invoicePrint.css"));
            var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css));
            var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html));
            string fontPath = Server.MapPath("~/fonts/arialuni.ttf");
            iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss, Encoding.UTF8, new UnicodeFontFactory(fontPath));
            doc.Close();
            bytes = ms.ToArray();

            //var testFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.pdf");
            //  System.IO.File.WriteAllBytes(testFile, bytes);
            #endregion

            return bytes;
        }

        public bool SendInvoiceToMail(string EmailReciever)
        {

            string strEmailResult = "Body of the email";


            var strResult = "";
            var userId = User.Identity.GetUserId();

            strResult = db.Fakturis.OrderByDescending(o => o.DateCreated).Where(u => u.UserID == userId).FirstOrDefault().FakturaHtml;

            strResult = strResult.Replace("\r\n", string.Empty);
            byte[] bytes =  GeneratePDF(strResult);
            
            SendEmail(EmailReciever, "Invoice", strEmailResult, bytes, DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf");

            return true;
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

        public Boolean SendEmail(string reciever, string subject, string body, byte[] FileByte, string NameAtachemnt)
        {
            try
            {
                SmtpClient SmtpServer = new SmtpClient();
                MailMessage mail = new MailMessage();
                mail.To.Add(reciever);
                mail.Subject = subject;
                mail.Body = body;
                Attachment file = new Attachment(new MemoryStream(FileByte), NameAtachemnt);
                mail.Attachments.Add(file);
                mail.IsBodyHtml = true;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        [ChildActionOnly]
        public Boolean SendEmail(string reciever, string subject, string body)
        {
            try
            {
                SmtpClient SmtpServer = new SmtpClient();
                MailMessage mail = new MailMessage();
                mail.To.Add(reciever);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

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