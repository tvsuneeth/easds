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

            _config.MapHttpAttributeRoutes();

            _config.Routes.MapHttpRoute(
                name: "Taxonomy Article Section content",
                routeTemplate: "{articleSection}/{page}",
                defaults: new { controller = "Taxonomy", action = "GetArticleSection", page = RouteParameter.Optional },
                constraints: new { articleSection = @"^[a-zA-Z- ]+", page = @"^\d*" }
            );

            _config.Routes.MapHttpRoute(
                name: "Taxonomy Article Section + Sector content",
                routeTemplate: "{articleSection}/{sector}/{page}",
                defaults: new { controller = "Taxonomy", action = "GetArticleSectionAndSector", page = RouteParameter.Optional },
                constraints: new { articleSection = @"^[a-zA-Z- ]+", sector = @"^[a-zA-Z- ]+", page = @"^\d*" }
            );

            app.UseWebApi(_config);
        }
    }
}