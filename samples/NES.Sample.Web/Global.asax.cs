using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NServiceBus;

namespace NES.Sample.Web
{
    using NServiceBus.Logging;

    public class MvcApplication : HttpApplication
    {
        public static ISendOnlyBus Bus { get; private set; }

        protected void Application_Start()
        {
            RegisterBus();
            RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        private void RegisterBus()
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.UseSerialization<JsonSerializer>();
            busConfiguration.UseTransport<MsmqTransport>();
            busConfiguration.Transactions().Disable();
            busConfiguration.PurgeOnStartup(false);
            
            LogManager.Use<NServiceBus.Log4Net.Log4NetFactory>();

            Bus = NServiceBus.Bus.CreateSendOnly(busConfiguration);
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
                new { controller = "Messages", action = "Index", id = UrlParameter.Optional });
        }
    }
}