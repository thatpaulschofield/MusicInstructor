using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicInstructor.Web.Models
{
    public class CookieAuthenticationSession : IAuthenticationSession
    {
        private const string COOKIE_ID = "authenticatedUserId";
        
        public Guid Id
        {
            get
            {
                var authenticatedUserIdCookie = AuthenticatedUserIdCookie;
                if (authenticatedUserIdCookie == null)
                    return new Guid();

                Guid parsedId;
                Guid.TryParse(authenticatedUserIdCookie.Value, out parsedId);
                return parsedId;
            }
            set { ResponseCookie.Value = value.ToString(); }
        }

        private static HttpCookie AuthenticatedUserIdCookie
        {
            get
            {
                var existingCookie = HttpContext.Current.Request.Cookies[COOKIE_ID];
                return existingCookie ?? CreateNewCookie(); 
            }
        }

        private static HttpCookie ResponseCookie
        {
            get
            {
                return HttpContext.Current.Response.Cookies[COOKIE_ID];
            }
        }

        private static HttpCookie CreateNewCookie()
        {
            var cookie = new HttpCookie(COOKIE_ID);
            HttpContext.Current.Response.AppendCookie(cookie);
            HttpContext.Current.Request.Cookies.Add(cookie);
            return cookie;
        }
    }

    public interface IAuthenticationSession
    {
        Guid Id { get; set; }
    }
}