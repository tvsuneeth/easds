﻿using System;
using Ninject;
using Ninject.Web.Common;
using Ninject.Extensions.Conventions;

using twg.chk.DataService.api;
using twg.chk.DataService.chkData.Repository;
using twg.chk.DataService.DbContext.Repository;
using twg.chk.DataService.DbContext.Intrastructure;

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

                    kernel.Bind(
                       x => x.From(typeof(Helpers.IUrlHelper).Assembly)
                           .Select(c => c.IsClass && c.Name.EndsWith("Helper"))
                           .BindDefaultInterface()
                           .Configure(y => y.InRequestScope()));
                                                                                                                              
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