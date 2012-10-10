using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.YouTube;

namespace MusicInstructor.Web.Models
{
    public class VideoBrowserModel
    {
        public string Instrument { get; set; }

        public IEnumerable<Video> Videos { get; set; }
    }
}