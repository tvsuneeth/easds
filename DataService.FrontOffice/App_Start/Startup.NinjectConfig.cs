using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ninject;

using twg.chk.DataService.api;
using twg.chk.DataService.chkData.Repository;

namespace twg.chk.DataService.FrontOffice
{
    public partial class Startup
    {
        public static IKernel DependencyKernel { get; private set; }

        public static class NinjectConfig
        {
            public static void CreateKernel(IKernel kernel)
            {
                try
                {
                    //kernel.Bind<IUserServiceFactory>().To<UserServiceFactory>();
                    //kernel.Bind<IAuthenticationService>().To<AuthenticationService>();
                    kernel.Bind<IStaticPageRepository>().To<StaticPageRepository>();
                    kernel.Bind<IStaticPageService>().To<StaticPageService>();
                    kernel.Bind<IArticleRepository>().To<ArticleRepository>();
                    kernel.Bind<IArticleService>().To<ArticleService>();
                    kernel.Bind<UserManager<IdentityUser>>().ToConstructor<UserManager<IdentityUser>>(c => new UserManager<IdentityUser>(new UserStore<IdentityUser>(new twg.chk.DataService.DbContext.DataServiceEntities())));

                    DependencyKernel = kernel;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}