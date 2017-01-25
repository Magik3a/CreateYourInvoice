using Services.WebClient.MultiLanguage.Contracts;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(akcet_fakturi.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(akcet_fakturi.App_Start.NinjectWebCommon), "Stop")]

namespace akcet_fakturi.App_Start
{
    using System;
    using System.Web.Mvc;
    using System.Web;
    using Ninject.Extensions.Conventions;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject.Web.Mvc;
    using Ninject;
    using Ninject.Web.Common;
    using Services.WebClient.MultiLanguage;

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
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind(b => b.From("Services")
          .SelectAllClasses()
          .BindDefaultInterface().Configure(y => y.InSingletonScope()));


        }
    }
}
