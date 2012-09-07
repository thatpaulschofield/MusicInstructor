using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MusicInstructor.Web;
using MusicInstructor.Web.Controllers;
using MusicInstructor.Web.Domain;
using MusicInstructor.Web.Models;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Linq;
using SpecsFor;
using Should;
using MvcContrib.ActionResults;
using StructureMap;
using StructureMap.Configuration.DSL.Expressions;

namespace MusicInstructor.Tests.LoggingIn
{
    public class When_I_log_in_successfully : SpecsFor<LoginController>
    {
        private IDocumentSession _session;
        private ActionResult _result;
        private IAuthenticationSession _authenticationSession;

        protected override void ConfigureContainer(IContainer container)
        {
            container.Configure(x =>
                                    {
                                        x.AddRegistry<DefaultRegistry>();
                                        x.AddRegistry<UnitTestEnvironment>();
                                    });
            base.ConfigureContainer(container);
            container.SetDefaultsToProfile("UnitTests");
            _session = container.GetInstance<IDocumentSession>();
            _authenticationSession = container.GetInstance<IAuthenticationSession>();
        }

        protected override void Given()
        {
            _session.Store(new Login { Id = new Guid("{0840F41C-C6F5-41BC-825B-57FB086BBCF5}"), UserName = "test@user.com", Password = "password" });
            _session.SaveChanges();
        }

        protected override void When()
        {
            _result = SUT.LogIn(new LoginModel {UserName = "test@user.com", Password = "password"});
        }

        [Test]
        public void Should_redirect_to_dashboard()
        {
            _result.ShouldBeType<RedirectToRouteResult<DashboardController>>();

        }

        [Test]
        public void Should_start_a_login_session()
        {
            _authenticationSession.Id.ShouldEqual("0840F41C-C6F5-41BC-825B-57FB086BBCF5".ToLower());
        }
    }
}
