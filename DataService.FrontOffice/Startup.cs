using System;
using Owin;
namespace twg.chk.DataService.FrontOffice
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureWebApi(app);
        }
    }
}