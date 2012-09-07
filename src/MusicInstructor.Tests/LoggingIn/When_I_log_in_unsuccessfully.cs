using System;
using System.Web.Mvc;
using MusicInstructor.Web.Controllers;
using MusicInstructor.Web.Domain;
using MusicInstructor.Web.Models;
using NUnit.Framework;
using Raven.Client;
using Should;
using SpecsFor;

namespace MusicInstructor.Tests.LoggingIn
{
    public class When_I_log_in_unsuccessfully : SpecsFor<LoginController>
    {
        private IDocumentSession _session;
        private ActionResult _result;

        protected override void ConfigureContainer(StructureMap.IContainer container)
        {
            container.Configure(x => 
            { x.AddRegistry<UnitTestEnvironment>(); 
            });
            _session = container.GetInstance<IDocumentSession>();
            base.ConfigureContainer(container);
        }

        protected override void Given()
        {
            _session.Store(new Login { Id = new Guid("{0840F41C-C6F5-41BC-825B-57FB086BBCF5}"), UserName = "test@user.com", Password = "password" });
            _session.SaveChanges();
        }

        protected override void When()
        {
            _result = SUT.LogIn(new LoginModel { UserName = "test@user.com", Password = "wrongpassword" });
        }

        [Test]
        public void Should_not_redirect_to_dashboard()
        {
            _result.ShouldBeType<ViewResult>();

        }
    }
}