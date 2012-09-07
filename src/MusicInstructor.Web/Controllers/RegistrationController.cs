using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using MusicInstructor.Web.Domain;
using MusicInstructor.Web.Models;
using Raven.Client;
using MvcContrib;

namespace MusicInstructor.Web.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IDocumentSession _documentSession;

        public RegistrationController(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ActionResult Index()
        {
            return View(new RegistrationModel { Id = Guid.NewGuid()});
        }

        public ActionResult Register(RegistrationModel registration)
        {
            _documentSession.Store(Mapper.Map<RegistrationModel, Login>(registration));
            _documentSession.SaveChanges();

            return this.RedirectToAction<DashboardController>(c => c.Index());
        }
    }
}
