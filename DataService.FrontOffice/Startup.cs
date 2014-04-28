using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin;
using Microsoft.Owin;
using Ninject;

using twg.chk.DataService.DbContext.Migrations;

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
        }
    }
}