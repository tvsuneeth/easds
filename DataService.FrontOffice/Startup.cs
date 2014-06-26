using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin;
using Microsoft.Owin;
using Ninject;

using twg.chk.DataService.DbContext.Migrations;
using System.Configuration;
using System.Web.Configuration;
using System.Web.Http;

[assembly: OwinStartup(typeof(twg.chk.DataService.FrontOffice.Startup))]
namespace twg.chk.DataService.FrontOffice
{
    public partial class Startup
    {
        static Startup()
        {
            System.Data.Entity.Database.SetInitializer(new DataServiceSampleData());

            NinjectConfig.CreateKernel(new StandardKernel());
            RegisterAuth();
        }
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);         
            ConfigureWebApi(app);

            ConfigureErrorHandling();

           
        }

        private static void ConfigureErrorHandling()
        {

            var config = (CustomErrorsSection)
      ConfigurationManager.GetSection("system.web/customErrors");

            IncludeErrorDetailPolicy errorDetailPolicy;

            switch (config.Mode)
            {
                case CustomErrorsMode.RemoteOnly:
                    errorDetailPolicy
                        = IncludeErrorDetailPolicy.LocalOnly;
                    break;
                case CustomErrorsMode.On:
                    errorDetailPolicy
                        = IncludeErrorDetailPolicy.Never;
                    break;
                case CustomErrorsMode.Off:
                    errorDetailPolicy
                        = IncludeErrorDetailPolicy.Always;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy
                = errorDetailPolicy;
        }
    }
}