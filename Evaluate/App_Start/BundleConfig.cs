using System.Web;
using System.Web.Optimization;

namespace Evaluate
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
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/dashboard.css"));

            bundles.Add(new ScriptBundle("~/miminium-master/asset/js/js").Include(
                      "~/miminium-master/asset/js/jquery.min.js",
                      "~/miminium-master/asset/js/jquery.ui.min.js",
                      "~/miminium-master/asset/js/bootstrap.min.js",
                      "~/miminium-master/asset/js/plugins/moment.min.js",
                      "~/miminium-master/asset/js/plugins/fullcalendar.min.js",
                      "~/miminium-master/asset/js/plugins/jquery.nicescroll.js",
                      "~/miminium-master/asset/js/plugins/jquery.vmap.min.js",
                      "~/miminium-master/asset/js/plugins/maps/jquery.vmap.world.js",
                      "~/miminium-master/asset/js/plugins/jquery.vmap.sampledata.js",
                      "~/miminium-master/asset/js/plugins/chart.min.js",
                      "~/miminium-master/asset/js/main.js"));

            bundles.Add(new StyleBundle("~/miminium-master/asset/css/css").Include(
                      "~/miminium-master/asset/css/bootstrap.min.css",
                      "~/miminium-master/asset/css/plugins/font-awesome.min.css",
                      "~/miminium-master/asset/css/plugins/simple-line-icons.css",
                      "~/miminium-master/asset/css/plugins/animate.min.css",
                      "~/miminium-master/asset/css/plugins/fullcalendar.min.css",
                      "~/miminium-master/asset/css/style.css",
                      "~/Content/custome.css"));

            bundles.Add(new StyleBundle("~/Content/css/custome/css").Include(
                      "~/Content/custome.css"));

            bundles.Add(new StyleBundle("~/miminium-master/css/authenticate").Include(
                      "~/miminium-master/asset/css/bootstrap.min.css",
                      "~/miminium-master/asset/css/plugins/font-awesome.min.css",
                      "~/miminium-master/asset/css/plugins/simple-line-icons.css",
                      "~/miminium-master/asset/css/plugins/animate.min.css",
                      "~/miminium-master/asset/css/plugins/icheck/skins/flat/aero.css",
                      "~/miminium-master/asset/css/style.css"));

            bundles.Add(new ScriptBundle("~/miminium-master/js/authenticate").Include(
                      "~/miminium-master/asset/js/jquery.min.js",
                      "~/miminium-master/asset/js/jquery.ui.min.js",
                      "~/miminium-master/asset/js/bootstrap.min.js",
                      "~/miminium-master/asset/js/plugins/moment.min.js",
                      "~/miminium-master/asset/js/plugins/icheck.min.js",
                      "~/miminium-master/asset/js/main.js"));
        }
    }
}
