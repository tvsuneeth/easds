﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TW.CHKDataService.ClientApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "mediacontent",
               url: "mediacontent/{id}",
               defaults: new { controller = "Home", action = "GetMediaContent"}
           );

            routes.MapRoute(
               name: "serviceUrlRoute",
               url: "{*param}",
               defaults: new { controller = "Home", action = "GetService", param = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}