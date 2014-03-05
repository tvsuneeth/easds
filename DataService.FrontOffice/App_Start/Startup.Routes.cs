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
            _config.DependencyResolver = new NinjectResolver(NinjectConfig.CreateKernel(new StandardKernel()));
            _config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            app.UseWebApi(_config);
        }


        public static class NinjectConfig
        {
            public static IKernel CreateKernel(IKernel kernel)
            {
                try
                {
                    kernel.Bind<ICarrouelManager>().To<CarrouselManager>();
                    return kernel;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}