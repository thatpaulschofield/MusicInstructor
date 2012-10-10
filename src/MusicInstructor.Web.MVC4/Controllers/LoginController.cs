using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicInstructor.Web.Domain;
using MusicInstructor.Web.Models;
using MvcContrib;
using Raven.Client;

namespace MusicInstructor.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IDocumentSession _documentSession;
        private readonly IAuthenticationSession _authenticationSession;

        public LoginController(IDocumentSession documentSession, IAuthenticationSession authenticationSession)
        {
            _documentSession = documentSession;
            _authenticationSession = authenticationSession;
        }

        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View(new LoginModel());
        }

        public ActionResult LogIn(LoginModel request)
        {
            var login = _documentSession.Query<Login>().FirstOrDefault(l => l.UserName == request.UserName && l.Password == request.Password);
            if (login == null)
                return View("Index", request);

            _authenticationSession.Id = login.Id;
            return this.RedirectToAction<DashboardController>(x => x.Index());
        }
    }
}
