using MusicInstructor.Web.Controllers;
using MusicInstructor.Web.Models;
using MvcContrib.TestHelper;
using NUnit.Framework;
using SpecsFor;
using SpecsFor.Mvc;

namespace MusicInstructor.Tests.Integration
{
    [Category("Integration")]
    public class LoginSpecs : SpecsFor<MvcWebApp>
    {
        protected override void Given()
        {
            SUT.NavigateTo<LoginController>(x => x.Index());
        }

        protected override void When()
        {
            SUT.FindFormFor<LoginModel>()
                .Field(x => x.UserName).SetValueTo("test@user.com")
                .Field(x => x.Password).SetValueTo("password")
                .Submit();
        }

        [Test]
        public void then()
        {
            SUT.Route.ShouldMapTo<DashboardController>();
            SUT.FindDisplayFor<ProfileViewModel>().DisplayFor(vm => vm.UserName).Text.ShouldBe("test@user.com");
        }
    }

    public class FailedLoginSpecs : SpecsFor<MvcWebApp>
    {
        protected override void Given()
        {
            SUT.NavigateTo<LoginController>(x => x.Index());
        }

        protected override void When()
        {
            SUT.FindFormFor<LoginModel>()
                .Field(x => x.UserName).SetValueTo("test@user.com")
                .Field(x => x.Password).SetValueTo("wrongpassword")
                .Submit();
        }

        [Test]
        public void then()
        {
            SUT.Route.ShouldMapTo<LoginController>();
        }
    }

    //An NUnit SetUpFixture is executed before
    //any tests in the fixture's namespace or 
    //subnamespaces.  Usually you would put
    //this in the root of your test project.
}
