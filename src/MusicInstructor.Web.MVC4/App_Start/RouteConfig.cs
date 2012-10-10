using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MusicInstructor.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("login", "Login", new {controller = "Login"});
            routes.MapRoute("dashboard", "Dashboard", new {controller = "Dashboard"});

            routes.MapRoute("videoSearch", "Videos/Search", new {controller = "VideoBrowser", action = "Search"});
            routes.MapRoute("videosByInstrument", "Videos/{instrument}",
                            new {controller = "VideoBrowser", action = "Index", instrument = UrlParameter.Optional});
            //routes.MapRoute("videos", "Videos", new { controller = "VideoBrowser" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );

        }
    }
}