using System.Web.Mvc;
using System.Web.Routing;

namespace Truco
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                 name: "Default",
                 url: "{controller}/{action}/{id}",
                 defaults: new { controller = "Inicial", action = "Indice", id = UrlParameter.Optional },
                 namespaces: new string[] { "Truco.Controllers" }
             );
        }
    }
}