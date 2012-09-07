using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicInstructor.Web.Models
{
    public class RegistrationModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}