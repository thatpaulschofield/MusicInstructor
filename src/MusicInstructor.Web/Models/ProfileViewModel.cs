using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicInstructor.Web.Models
{
    public class ProfileViewModel
    {
        static ProfileViewModel _nullProfile = new ProfileViewModel { IsNull = true};

        public string UserName { get; set; }
        public bool IsNull { get; private set; }

        public static ProfileViewModel NullProfile  
        {
            get { return _nullProfile; }
        }
    }
}