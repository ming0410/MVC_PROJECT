using System.Web;
using System.Web.Optimization;

namespace CameoMvc
{
    public class BundleConfig
    {
        // 如需統合的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好可進行生產時，請使用 https://modernizr.com 的建置工具，只挑選您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //<!-- jQuery UI 1.11.4 -->
            bundles.Add(new ScriptBundle("~/bundles/jqueryUI").Include(
                        "~/Scripts/jquery-ui/jquery-ui.min.js"));
            //
            //<!-- Bootstrap 4 -->
            //
            //<!-- AdminLTE App -->
            //<!-- AdminLTE for demo purposes -->
            //<!-- Chart.js -->
            //<!-- OverlayScrollbars -->
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap/js/bootstrap.bundle.min.js",
                      "~/Scripts/AdminLTE/dist/js/adminlte.min.js",
                      "~/Scripts/AdminLTE/dist/js/demo.js",
                      "~/Scripts/datetimepicker/js/bootstrap-datetimepicker.js",
                      "~/Scripts/datetimepicker/js/locales/bootstrap-datetimepicker.zh-TW.js",
                      "~/Scripts/moment.min.js",
                      "~/Scripts/chart.min.js",
                      "~/Scripts/overlayScrollbars/js/jquery.overlayScrollbars.min.js"));

            

            //<!-- Bootstrap Theme style -->
            //
            //<!-- jQuery-UI Theme style -->
            //<!-- Google Font: Source Sans Pro -->
            //<!-- Font Awesome style-->
            //<!-- AdminLTE Theme style-->
            //<!-- Bootstrap datetimepicker style-->
            //<!-- OverlayScrollbars style-->
            //
            //
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Scripts/jquery-ui/jquery-ui.min.css",
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/font.google.css",
                      "~/Content/fontawesome/css/all.min.css",
                      "~/Scripts/AdminLTE/dist/css/adminlte.min.css",
                      "~/Scripts/datetimepicker/css/bootstrap-datetimepicker.min.css",
                      "~/Scripts/overlayScrollbars/css/OverlayScrollbars.min.css"));
        }
    }
}
