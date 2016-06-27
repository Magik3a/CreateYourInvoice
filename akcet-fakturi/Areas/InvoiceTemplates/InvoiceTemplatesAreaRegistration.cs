using System.Web.Mvc;

namespace akcet_fakturi.Areas.InvoiceTemplates
{
    public class InvoiceTemplatesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "InvoiceTemplates";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "InvoiceTemplates_default",
                "InvoiceTemplates/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}