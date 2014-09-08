using System;
using System.Web.Http;
using Owin;
using Ninject;
using WebApiContrib.IoC.Ninject;
using System.Linq;

using TWG.EASDataService.Services;
using Newtonsoft.Json;

namespace TWG.EASDataService.Api
{
    public partial class Startup
    {
        private HttpConfiguration _config = new HttpConfiguration();

        public void ConfigureWebApi(IAppBuilder app)
        {
            _config.DependencyResolver = new NinjectResolver(DependencyKernel);

            // add specific formatter for contentType application/atom+xml
            _config.Formatters.Add(new AtomSyndicationFeedFormatter(DependencyKernel.Get<IStaticContentLinkService>()));

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


            //ELMAH.IO  exception logging
            _config.Filters.Add(new UnhandledExceptionFilter());

            var formatters = _config.Formatters;
            formatters.Remove(formatters.XmlFormatter);

            JsonSerializerSettings jsonSetting = new JsonSerializerSettings();
            jsonSetting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            _config.Formatters.JsonFormatter.SerializerSettings = jsonSetting;

            app.UseWebApi(_config);
        }
    }
}