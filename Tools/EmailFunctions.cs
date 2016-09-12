using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public static class EmailFunctions
    {

        public static Boolean SendEmail(string reciever, string subject, string body, byte[] FileByte, string NameAtachemnt)
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

 
        public static Boolean SendEmail(string reciever, string subject, string body)
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


        public static void SendExceptionToAdmin(Exception ex)
        {
            string emailAdmin = ConfigurationManager.AppSettings["TechnicalSupportEmail"];
            var sb = new StringBuilder();
            sb.AppendLine("Message: ");
            sb.AppendLine(ex.Message.ToString());
            sb.AppendLine("================================");
            sb.AppendLine("Inner exception: ");
            sb.AppendLine(ex.InnerException?.ToString());
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
