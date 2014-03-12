using System;
using Ninject;

using twg.chk.DataService.api;

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
                    kernel.Bind<IUserServiceFactory>().To<UserServiceFactory>();
                    kernel.Bind<IAuthenticationService>().To<AuthenticationService>();
                    kernel.Bind<ICarrouselService>().To<CarrouselService>();

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