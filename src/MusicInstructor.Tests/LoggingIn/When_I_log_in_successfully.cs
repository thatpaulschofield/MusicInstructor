using System;
using System.Collections.Generic;
using MusicInstructor.Web;
using MusicInstructor.Web.Controllers;
using MusicInstructor.Web.Domain;
using MusicInstructor.Web.Models;
using MvcContrib.ActionResults;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Linq;
using Should;
using StructureMap;
using StructureMap.Configuration.DSL.Expressions;

namespace MusicInstructor.Tests.LoggingIn
{
    public class When_I_log_in_successfully : AbstractSpecsFor<LoginController>
    {
        
        protected override void ConfigureContainer(IContainer container)
        {
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

        protected override void Given()
        {
            Session.Store(new Login { Id = new Guid("{0840F41C-C6F5-41BC-825B-57FB086BBCF5}"), UserName = "test@user.com", Password = "password" });
            Session.SaveChanges();
        }

        protected override void When()
        {
            Result = SUT.LogIn(new LoginModel {UserName = "test@user.com", Password = "password"});
        }

        [Test]
        public void Should_redirect_to_dashboard()
        {
            Result.ShouldBeType<RedirectToRouteResult<DashboardController>>();

        }

        [Test]
        public void Should_start_a_login_session()
        {
            AuthenticationSession.Id.ShouldEqual(Guid.Parse("0840F41C-C6F5-41BC-825B-57FB086BBCF5"));
        }
    }
}
