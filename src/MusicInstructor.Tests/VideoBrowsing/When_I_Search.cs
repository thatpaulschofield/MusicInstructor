using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Google.GData.Extensions;
using Google.GData.Extensions.MediaRss;
using Google.YouTube;
using MusicInstructor.Tests.LoggingIn;
using MusicInstructor.Web.Controllers;
using MusicInstructor.Web.Models;
using NUnit.Framework;
using Should;

namespace MusicInstructor.Tests.VideoBrowsing
{
    public class When_I_Search : AbstractSpecsFor<VideoBrowserController>
    {
        private RedirectToRouteResult _result;

        protected override void When()
        {
            var model = new VideoBrowserModel
                            {
                                Instrument = "bass"
                            };
            _result = SUT.Search(model) as RedirectToRouteResult;

        }

        [Test]
        public void Should_display_video_page_for_correct_search()
        {
            _result.RouteValues.ShouldContain(new KeyValuePair<string, object>("instrument", "bass"));
        }
    }

    public class When_I_Browse : AbstractSpecsFor<VideoBrowserController>
    {
        private ViewResult _result;
        private VideoBrowserModel _responseModel;

        protected override void ConfigureContainer(StructureMap.IContainer container)
        {
            var search = new StubKeywordVideoSearch
                             {
                                 ResultsToReturn = new List<Video>
                                                       {
                                                           new Video()
                                                       }
                             };
            container.Configure(c => c.For<IVideoKeywordSearch>().Use<StubKeywordVideoSearch>());
        }

        protected override void When()
        {
            var model = new VideoBrowserModel
                            {
                                Instrument = "bass"
                            };
            _result = SUT.Index(model) as ViewResult;
            _responseModel = (_result.Model as VideoBrowserModel);
        }

        [Test]
        public void Should_perform_correct_search()
        {
            _responseModel.Instrument.ShouldEqual("bass");
        }

        [Test]
        public void Should_include_all_results()
        {
            _responseModel.Videos.Count().ShouldEqual(1);
        }
    }

    public class StubKeywordVideoSearch : IVideoKeywordSearch
    {
        public StubKeywordVideoSearch()
        {
            ResultsToReturn = new List<Video>();
        }
        public IQueryable<Video> Execute()
        {
            return ResultsToReturn.AsQueryable();
        }

        public string Keywords { get; set; }
        public List<Video> ResultsToReturn { get; set; }
    }
}
