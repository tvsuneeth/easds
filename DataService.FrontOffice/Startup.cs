using System;
using Owin;
using Ninject;

namespace twg.chk.DataService.FrontOffice
{
    public partial class Startup
    {
        static Startup()
        {
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