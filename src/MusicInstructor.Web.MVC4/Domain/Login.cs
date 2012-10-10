using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicInstructor.Web.Domain
{
    public class Login : Entity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}