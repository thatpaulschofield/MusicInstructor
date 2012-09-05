using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicInstructor.Web.Domain;
using MusicInstructor.Web.Models;

namespace MusicInstructor.Web.Controllers
{
    public class AuthenticatedUserController : Controller
    {
        private readonly ProfileViewModel _profile;
        //
        // GET: /Authenticated/
        public AuthenticatedUserController(ProfileViewModel profile)
        {
            _profile = profile;
        }

        public ActionResult Index()
        {
            if (_profile.IsNull)
                return new PartialViewResult { ViewName = "NotAuthenticated" };
            return new PartialViewResult { ViewName = "Authenticated", ViewData = new ViewDataDictionary(_profile)};
        }

    }
}
