using System;
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

            _config.Routes.MapHttpRoute(
                name: "Article content",
                routeTemplate: "article/{id}",
                defaults: new { controller = "Article" },
                constraints: new { id = @"^\d+" }
            );

            _config.Routes.MapHttpRoute(
                name: "Static page content",
                routeTemplate: "content/{id}",
                defaults: new { controller = "StaticPage" },
                constraints: new { id = @"^\d+" }
            );

            _config.Routes.MapHttpRoute(
                name: "Topic content",
                routeTemplate: "topic/{topic}/{page}",
                defaults: new { controller = "Taxonomy", action = "GetTopic", page = RouteParameter.Optional },
                constraints: new { page = @"^\d*" }
            );

            _config.Routes.MapHttpRoute(
                name: "Sector content",
                routeTemplate: "sector/{sector}/{page}",
                defaults: new { controller = "Taxonomy", action = "GetSector", page = RouteParameter.Optional },
                constraints: new { page = @"^\d*" }
            );

            _config.Routes.MapHttpRoute(
                name: "Taxonomy Article Section content",
                routeTemplate: "{articleSection}/{page}",
                defaults: new { controller = "Taxonomy", action = "GetArticleSection", page = RouteParameter.Optional },
                constraints: new { page = @"^\d*" }
            );

            _config.Routes.MapHttpRoute(
                name: "Taxonomy Article Section + Sector content",
                routeTemplate: "{articleSection}/{sector}/{page}",
                defaults: new { controller = "Taxonomy", action = "GetArticleSectionAndSector", page = RouteParameter.Optional },
                constraints: new { page = @"^\d*" }
            );

            _config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseWebApi(_config);
        }
    }
}