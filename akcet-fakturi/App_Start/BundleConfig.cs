using System.Web;
using System.Web.Optimization;

namespace akcet_fakturi
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/pace.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // Bundle for Kendo UI script
            bundles.Add(new ScriptBundle("~/Scripts/kendoJS").Include(

                "~/Scripts/kendo/kendo.all.js",
                "~/Scripts/kendo/kendo.aspnetmvc.js",
                      "~/Scripts/kendo/cultures/kendo.culture.bg-BG.min.js"));

            // Bundle for Kendo UI css
            bundles.Add(new StyleBundle("~/Content/Css/kendo/kendoCss").Include(
                "~/Content/kendo/kendo.common-bootstrap.css",
                       "~/Content/kendo/kendo.silver.min.css",
                       "~/Content/pace/themes/orange/pace-theme-flash.css"
                ));
        }
    }
}
