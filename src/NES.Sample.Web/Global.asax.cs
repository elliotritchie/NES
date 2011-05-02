using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NServiceBus;

namespace NES.Sample.Web
{
    public class MvcApplication : HttpApplication
    {
        public static IBus Bus { get; private set; }

        protected void Application_Start()
        {
            RegisterBus();
            RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        private void RegisterBus()
        {
            Bus = Configure.WithWeb()
                .Log4Net()
                .DefaultBuilder()
                .XmlSerializer()
                .MsmqTransport()
                .IsTransactional(false)
                .PurgeOnStartup(false)
                .UnicastBus()
                .ImpersonateSender(false)
                .CreateBus()
                .Start();
        }

        private static void RegisterAllAreas()
        {
            AreaRegistration.RegisterAllAreas();
        }

        private static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.IgnoreRoute("content/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Messages", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}