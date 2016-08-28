using System.Web.Mvc;

namespace akcet_fakturi.Areas.WerkbriefTemplates
{
    public class WerkbriefTemplatesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "WerkbriefTemplates";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "WerkbriefTemplates_default",
                "WerkbriefTemplates/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}