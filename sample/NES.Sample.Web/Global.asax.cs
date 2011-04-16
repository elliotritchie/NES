using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NES.Sample.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
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