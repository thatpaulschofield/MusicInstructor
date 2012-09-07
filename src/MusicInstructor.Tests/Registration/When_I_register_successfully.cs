using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MusicInstructor.Tests.LoggingIn;
using MusicInstructor.Web.Controllers;
using MusicInstructor.Web.Domain;
using MusicInstructor.Web.Models;
using NUnit.Framework;
using SpecsFor;
using Should;

namespace MusicInstructor.Tests.Registration
{
    public class When_I_register_successfully : AbstractSpecsFor<RegistrationController>
    {
        private Login _login;
        private UserProfile _profile;

        protected override void When()
        {
            base.Given();
            Result = SUT.Register(new RegistrationModel
                             {
                                 Id = new Guid("{FD3936F3-71CE-4C5D-A4F4-73794A10DFD5}"),
                                 FirstName = "Bob",
                                 LastName = "Vance",
                                 Password = "password",
                                 UserName = "test@user.com"
                             });
            _login = Session.Load<Login>(new Guid("FD3936F3-71CE-4C5D-A4F4-73794A10DFD5"));
            _profile = Session.Load<UserProfile>(new Guid("FD3936F3-71CE-4C5D-A4F4-73794A10DFD5"));
        }

        [Test]
        public void Should_create_login()
        {
            _login.ShouldNotBeNull("Login was not created");
        }

        [Test]
        public void Should_create_profile()
        {
            _profile.ShouldNotBeNull("Profile was not created");
        }

    }
}
