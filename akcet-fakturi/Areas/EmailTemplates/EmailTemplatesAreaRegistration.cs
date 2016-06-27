using System.Web.Mvc;

namespace akcet_fakturi.Areas.EmailTemplates
{
    public class EmailTemplatesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "EmailTemplates";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "EmailTemplates_default",
                "EmailTemplates/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}