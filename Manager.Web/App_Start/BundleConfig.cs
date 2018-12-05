using System.Web.Optimization;

namespace Manager.Web
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            //VENDOR RESOURCES

            //~/Bundles/vendor/css
            bundles.Add(
                new StyleBundle("~/Bundles/vendor/css")
                    .Include("~/Content/themes/base/base.css", new CssRewriteUrlTransform())
                    .Include("~/Content/themes/base/theme.css", new CssRewriteUrlTransform())
                    .Include("~/Content/themes/bootstrap-paper.min.css", new CssRewriteUrlTransform())
                    .Include("~/Content/toastr/toastr.min.css", new CssRewriteUrlTransform())
                    .Include("~/Content/sweetalert/sweet-alert.min.css", new CssRewriteUrlTransform())
                    .Include("~/Content/flags/famfamfam-flags.css", new CssRewriteUrlTransform())
                    .Include("~/Content/fonts/font-awesome.min.css", new CssRewriteUrlTransform())
                );

            //~/Bundles/vendor/js/top (These scripts should be included in the head of the page)
            bundles.Add(
                new ScriptBundle("~/Bundles/vendor/js/top")
                    .Include(
                        "~/Abp/Framework/scripts/utils/ie10fix.js",
                        "~/Content/scripts/others/modernizr-2.8.3.js"
                    )
                );

            //~/Bundles/vendor/bottom (Included in the bottom for fast page load)
            bundles.Add(
                new ScriptBundle("~/Bundles/vendor/js/bottom")
                    .Include(
                        "~/Content/scripts/others/json2.min.js",

                        "~/Content/scripts/others/jquery-2.2.0.min.js",
                        "~/Content/scripts/others/jquery-ui-1.11.4.min.js",

                        "~/Content/scripts/others/bootstrap.min.js",

                        "~/Content/scripts/others/moment-with-locales.min.js",
                        "~/Content/scripts/others/jquery.validate.min.js",
                        "~/Content/scripts/others/jquery.blockUI.js",
                        "~/Content/toastr/toastr.min.js",
                        "~/Content/sweetalert/sweet-alert.min.js",
                        "~/Content/scripts/others/spinjs/spin.js",
                        "~/Content/scripts/others/spinjs/jquery.spin.js",

                        "~/Abp/Framework/scripts/abp.js",
                        "~/Abp/Framework/scripts/libs/abp.jquery.js",
                        "~/Abp/Framework/scripts/libs/abp.toastr.js",
                        "~/Abp/Framework/scripts/libs/abp.blockUI.js",
                        "~/Abp/Framework/scripts/libs/abp.spin.js",
                        "~/Abp/Framework/scripts/libs/abp.sweet-alert.js",

                        "~/Content/scripts/others/jquery.signalR-2.2.0.min.js"
                    )
                );

            //APPLICATION RESOURCES

            //~/Bundles/css
            bundles.Add(
                new StyleBundle("~/Bundles/css")
                    .Include("~/Content/css/main.css",
                    "~/Content/css/jsgrid/jsgrid-1.5.2.min.css",
                    "~/Content/jsgrid/jsgrid-theme-1.5.2.min.css",
                    "~/Content/jsgrid/jsgrid-custom.min.css",
                    "~/Content/scrollable/asScrollable.min.css",
                    "~/Content/floatlabel/floatlabel.min.css",
                    "~/Content/kartik-v/fileinput/css/fileinput.min.css")
                );

            //~/Bundles/js
            bundles.Add(
                new ScriptBundle("~/Bundles/js")
                    .Include("~/Content/scripts/main.js",
                    "~/Content/jsgrid/jsgrid-1.5.2.min.js",
                    "~/Content/jsgrid/i18n/es.js",
                    "~/Content/scrollable/jquery-asScrollable.min.js",
                    "~/Content/floatlabel/floatlabel.min.js",
                    "~/Content/kartik-v/fileinput/plugins/canvas-to-blob.min.js",
                    "~/Content/kartik-v/fileinput/plugins/sortable.js",
                    "~/Content/kartik-v/fileinput/plugins/purify.js",
                    "~/Content/kartik-v/fileinput/fileinput.min.js",
                    "~/Content/kartik-v/fileinput/themes/fa/theme.js",
                    "~/Content/kartik-v/fileinput/locales/es.js")
                );
        }
    }
}