using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicInstructor.Web.Models
{
    public class CookieAuthenticationSession : IAuthenticationSession
    {
        private const string COOKIE_ID = "authenticatedUserId";
        
        public string Id
        {
            get
            {
                var authenticatedUserIdCookie = AuthenticatedUserIdCookie;
                if (authenticatedUserIdCookie == null)
                    return String.Empty;

                return authenticatedUserIdCookie.Value;
            }
            set { AuthenticatedUserIdCookie.Value = value; }
        }

        private static HttpCookie AuthenticatedUserIdCookie
        {
            get { return HttpContext.Current.Request.Cookies[COOKIE_ID]; }
        }
    }

    public interface IAuthenticationSession
    {
        string Id { get; set; }
    }
}