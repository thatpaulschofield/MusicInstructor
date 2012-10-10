using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Google.GData.Client;
using Google.GData.YouTube;
using Google.YouTube;
using MusicInstructor.Web.Models;
using Raven.Client;

namespace MusicInstructor.Web.Controllers
{
    public class VideoBrowserController : Controller
    {
        private readonly IVideoKeywordSearch _search;

        public VideoBrowserController(IVideoKeywordSearch search)
        {
            _search = search;
        }

        public ActionResult Index(VideoBrowserModel model)
        {
            _search.Keywords = model.Instrument;
            var results = _search.Execute();
            model.Videos = results;
            return View("Index", model); 
        }

        public ActionResult Search(VideoBrowserModel model)
        {
            return base.RedirectToAction("Index", "VideoBrowser", BuildDictionary(model));
        }

        private RouteValueDictionary BuildDictionary(VideoBrowserModel model)
        {
            var dictionary = new RouteValueDictionary();
            dictionary.Add("instrument", model.Instrument);
            return dictionary;
        }
    }

    public class VideoSearchQuery : IVideoKeywordSearch
    {
        private readonly YouTubeRequest _request;

        public VideoSearchQuery(YouTubeRequest request)
        {
            _request = request;
        }

        public string Keywords { get; set; }

        public IQueryable<Video> Execute()
        {
            string queryUrl =
                String.Format("http://gdata.youtube.com/feeds/api/videos?q={0}&start-index=21&max-results=10&v=2", HttpContext.Current.Server.UrlEncode(this.Keywords));
            var query = new FeedQuery(queryUrl);
            return _request.Get<Video>(query).Entries.AsQueryable();
        }
    }

    public interface IVideoKeywordSearch : Query<Video>
    {
        string Keywords { set; }
    }

    public interface Query <T>
    {
        IQueryable<T> Execute();
    }
}