using System.Web.Mvc;
using MusicInstructor.Web;
using MusicInstructor.Web.App_Start;
using MusicInstructor.Web.Models;
using Raven.Client;
using SpecsFor;
using StructureMap;

namespace MusicInstructor.Tests.LoggingIn
{
    public abstract class AbstractSpecsFor<T> : SpecsFor<T> where T : class
    {
        protected IDocumentSession Session;
        protected ActionResult Result;
        protected IAuthenticationSession AuthenticationSession;

        protected override void ConfigureContainer(IContainer container)
        {
            AutoMapperConfig.Configure();
            container.Configure(x =>
                                    {
                                        x.AddRegistry<DefaultRegistry>();
                                        x.AddRegistry<UnitTestEnvironment>();
                                    });
            base.ConfigureContainer(container);
            container.SetDefaultsToProfile("UnitTests");
            Session = container.GetInstance<IDocumentSession>();
            AuthenticationSession = container.GetInstance<IAuthenticationSession>();
        }        
    }
}