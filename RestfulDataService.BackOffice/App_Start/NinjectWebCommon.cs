[assembly: WebActivator.PreApplicationStartMethod(typeof(twg.chk.DataService.BackOffice.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(twg.chk.DataService.BackOffice.App_Start.NinjectWebCommon), "Stop")]

namespace twg.chk.DataService.BackOffice.App_Start
{
    using System;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Extensions.Conventions;

    using twg.chk.DataService.api;
    using twg.chk.DataService.DbContext;
    using twg.chk.DataService.DbContext.Repository;
    using twg.chk.DataService.DbContext.Intrastructure;
    using twg.chk.DataService.chkData.Repository;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind(
                x => x.From(typeof(IStaticPageService).Assembly)
                    .Select(c => c.IsClass && c.Name.EndsWith("Service"))
                    .BindDefaultInterface()
                    .Configure(y => y.InRequestScope()));

            kernel.Bind(
                x => x.From(typeof(IStaticPageRepository).Assembly)
                    .Select(c => c.IsClass && c.Name.EndsWith("Repository"))
                    .BindDefaultInterface()
                    .Configure(y => y.InRequestScope()));

            kernel.Bind(
                x => x.From(typeof(IStaticContentLinkRepository).Assembly)
                    .Select(c => c.IsClass && c.Name.EndsWith("Repository"))
                    .BindDefaultInterface()
                    .Configure(y => y.InRequestScope()));

            kernel.Bind<IDatabaseFactory>().To<DatabaseFactory>().InRequestScope();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
        }
    }
}
