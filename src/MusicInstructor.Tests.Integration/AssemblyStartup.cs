using System;
using MusicInstructor.Web;
using MusicInstructor.Web.Domain;
using MusicInstructor.Web.Models;
using NUnit.Framework;
using Raven.Client.Embedded;
using Raven.Database.Server;
using SpecsFor.Mvc;

namespace MusicInstructor.Tests
{
    [SetUpFixture]
    public class AssemblyStartup
    {
        private SpecsForIntegrationHost _host;

        [SetUp]
        public void SetupTestRun()
        {
            var config = new SpecsForMvcConfig();
            //SpecsFor.Mvc can spin up an instance of IIS Express to host your app 
            //while the specs are executing.  
            config.UseIISExpress()
                //To do that, it needs to know the name of the project to test...
                .With(Project.Named("MusicInstructor.Web"))
                //And optionally, it can apply Web.config transformations if you want 
                //it to.
                .ApplyWebConfigTransformForConfig("Integration");

            //In order to leverage the strongly-typed helpers in SpecsFor.Mvc,
            //you need to tell it about your routes.  Here we are just calling
            //the infrastructure class from our MVC app that builds the RouteTable.
            config.BuildRoutesUsing(RouteConfig.RegisterRoutes);
            //SpecsFor.Mvc can use either Internet Explorer or Firefox.  Support
            //for Chrome is planned for a future release.
            config.UseBrowser(BrowserDriver.InternetExplorer);

            //Does your application send E-mails?  Well, SpecsFor.Mvc can intercept
            //those while your specifications are executing, enabling you to write
            //tests against the contents of sent messages.
            config.InterceptEmailMessagesOnPort(13565);

            config.Use<SeedDataConfig>();

            //The host takes our configuration and performs all the magic.  We
            //need to keep a reference to it so we can shut it down after all
            //the specifications have executed.
            _host = new SpecsForIntegrationHost(config);
            _host.Start();

        }

        //The TearDown method will be called once all the specs have executed.
        //All we need to do is stop the integration host, and it will take
        //care of shutting down the browser, IIS Express, etc. 
        [TearDown]
        public void TearDownTestRun()
        {
            _host.Shutdown();
        }
    }

    public class SeedDataConfig : SpecsForMvcConfig
    {
        private static EmbeddableDocumentStore _documentStore;

        public SeedDataConfig()
        {
            NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(8081);
            _documentStore = new EmbeddableDocumentStore
                                 {
                                     UseEmbeddedHttpServer = true,
                                     //RunInMemory = true,
                                     //ConnectionStringName = "RavenDb"
                                     DataDirectory = "Data"
                                 };
            _documentStore.Initialize();

            var session = _documentStore.OpenSession();
            session.Store(new Login { Id = new Guid("{C2D7C115-EDAA-471D-A43E-2B803AB7AF4B}"), UserName = "test@user.com", Password = "password"});
            session.Store(new ProfileViewModel());
            session.SaveChanges();
        }
    }
}