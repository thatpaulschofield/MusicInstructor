using MusicInstructor.Web.Domain;
using MusicInstructor.Web.Models;
using Ninject.Activation;

[assembly: WebActivator.PreApplicationStartMethod(typeof(MusicInstructor.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(MusicInstructor.Web.App_Start.NinjectWebCommon), "Stop")]

namespace MusicInstructor.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Raven.Client;
    using Raven.Client.Document;
    using System.Reflection;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();
        private static DocumentStore Store;

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
            Store = new DocumentStore { ConnectionStringName = "RavenDB" };
            Store.Initialize();

            Raven.Client.Indexes.IndexCreation.CreateIndexes(System.Reflection.Assembly.GetCallingAssembly(), Store);
            kernel.Bind<IDocumentSession>().ToMethod(x => Store.OpenSession()).InRequestScope();
            kernel.Bind<IAuthenticationSession>().To<CookieAuthenticationSession>();
            kernel.Bind<ProfileViewModel>().ToMethod(ResolveProfile).InRequestScope();
        }

        private static ProfileViewModel ResolveProfile(IContext context)
        {
            var session = context.Kernel.Get<IDocumentSession>();
            var authenticatedUserIdCookie = HttpContext.Current.Request.Cookies["authenticatedUserId"];
            if (authenticatedUserIdCookie == null)
                return ProfileViewModel.NullProfile;

            var profile = session.Load<ProfileViewModel>(authenticatedUserIdCookie.Value);
            return profile;
        }
    }
}
