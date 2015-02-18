using System.Web;
using System.Web.Optimization;

namespace TestProject
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

            bundles.Add(new ScriptBundle("~/bundles/bootstrapjs").Include(
                 "~/Scripts/bootstrap.js",
                 "~/Scripts/angular-ui/ui-bootstrap-tpls.js"
                 ));

            bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
                                 "~/Scripts/angular.js",
                                 "~/Scripts/angular-route.js",
                                 "~/Scripts/angular-sanitize.js",
                                 "~/Scripts/ng-table.js",
                                 "~/Scripts/moment.js",
                                 "~/Scripts/daterangepicker.js",
                                 "~/Scripts/ng-bs-daterangepicker.js"
                                 ));

            // our controller, services, directives and main app js files
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                                 "~/Scripts/Script.js"
                          ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/ng-table.css",
                      "~/Content/bootstrap.css",
                      "~/Content/daterangepicker-bs3.css",
                      "~/Content/site.css"));
        }
    }
}
