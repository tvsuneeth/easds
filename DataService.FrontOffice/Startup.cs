using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin;
using Microsoft.Owin;
using Ninject;

using twg.chk.DataService.DbContext;

[assembly: OwinStartup(typeof(twg.chk.DataService.FrontOffice.Startup))]
namespace twg.chk.DataService.FrontOffice
{
    public partial class Startup
    {
        static Startup()
        {
            NinjectConfig.CreateKernel(new StandardKernel());
            RegisterAuth();

            var sampleDataInit = new DataServiceSampleData(Startup.DependencyKernel.Get<UserManager<IdentityUser>>());
            System.Data.Entity.Database.SetInitializer(sampleDataInit);
        }
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureWebApi(app);
        }
    }
}