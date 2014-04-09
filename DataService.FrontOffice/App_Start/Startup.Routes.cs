﻿using System;
using System.Web.Http;
using Owin;
using Ninject;
using WebApiContrib.IoC.Ninject;

namespace twg.chk.DataService.FrontOffice
{
    public partial class Startup
    {
        private HttpConfiguration _config = new HttpConfiguration();

        public void ConfigureWebApi(IAppBuilder app)
        {
            _config.DependencyResolver = new NinjectResolver(DependencyKernel);

            // add specific formatter for contentType application/atom+xml
            _config.Formatters.Add(new AtomSyndicationFeedFormatter());

            _config.MapHttpAttributeRoutes();

            _config.Routes.MapHttpRoute(
                name: "GetArticleByArticleSection",
                routeTemplate: "{articleSection}/{page}",
                defaults: new { controller = "Taxonomy", action = "GetArticleSection", page = RouteParameter.Optional },
                constraints: new { articleSection = @"^[a-zA-Z0-9- ,&]+", page = @"^\d*" }
            );

            _config.Routes.MapHttpRoute(
                name: "GetArticleByArticleSectionAndSector",
                routeTemplate: "{articleSection}/{sector}/{page}",
                defaults: new { controller = "Taxonomy", action = "GetArticleSectionAndSector", page = RouteParameter.Optional },
                constraints: new { articleSection = @"^[a-zA-Z0-9- ,&]+", sector = @"^[a-zA-Z0-9- ,&]+", page = @"^\d*" }
            );

            app.UseWebApi(_config);
        }
    }
}