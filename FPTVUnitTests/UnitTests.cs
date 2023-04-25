using System.Drawing;
using FPTV.Controllers;
using FPTV.Data;
using FPTV.Models.EventsModels;
using FPTV.Models.Forum;
using FPTV.Models.MatchesModels;
using FPTV.Models.StatisticsModels;
using FPTV.Models.UserModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using static System.Formats.Asn1.AsnWriter;

namespace FPTVUnitTests
{
    //Classe de testes dos controladores (Home, Events, Matches, Stats)
    public class UnitTests : IClassFixture<ApplicationDbContextFixture>
    {
        private ApplicationDbContextFixture contextFixture;
        private FPTVContext _context;

        public UnitTests(ApplicationDbContextFixture _contextFixture)
        {
            contextFixture = _contextFixture;
            _context = _contextFixture.DbContext;
        }

        //TU1
        //HomeController
        [Fact]
        public void Index_ReturnsViewResult()
        {
            var controller = new HomeController(null, _context);
            var result = controller.Index("");
            Assert.IsType<ViewResult>(result);
        }

        //TU2
        //HomeController
        [Fact]
        public void Privacy_ReturnsViewResult()
        {
            var controller = new HomeController(null, _context);
            var result = controller.Privacy();
            Assert.IsType<ViewResult>(result);
        }

        //TU3
        //HomeController
        [Fact]
        public void ConfirmDelete_ReturnsViewResult()
        {
            var controller = new HomeController(null, _context);
            var result = controller.ConfirmDelete();
            Assert.IsType<ViewResult>(result);
        }


        //TU4
        //EventsController
        [Fact]
        public void Events_ReturnsViewResult()
        {
            var controller = new EventsController(_context);
            var result = controller.Index();
            Assert.IsType<ViewResult>(result);
        }

        //TU5
        //EventsController
        [Fact]
        public void EventDetails_ReturnsViewResult()
        {
            var controller = new EventsController(_context);
            var result = controller.Details(10065, "past", "csgo");
            Assert.IsType<ViewResult>(result);
        }

        //TU6
        //MatchesController
        [Fact]
        public void Matches_ReturnsViewResult()
        {
            var controller = new MatchesController(_context);
            var result = controller.Index();
            Assert.IsType<ViewResult>(result);
        }

        //TU7
        //MatchesController
        [Fact]
        public void MatchesDetails_ReturnsViewResult()
        {
            var controller = new MatchesController(_context);
            var result = controller.MatchDetails(751212, "past", "csgo");
            Assert.IsType<ViewResult>(result);
        }

        //TU8
        //MatchesController
        [Fact]
        public void Results_ReturnsViewResult()
        {
            var controller = new MatchesController(_context);
            var result = controller.Results();
            Assert.IsType<ViewResult>(result);
        }

        //TU9
        //MatchesController
        [Fact]
        public void TeamStats_ReturnsViewResult()
        {
            var controller = new MatchesController(_context);
            var result = controller.TeamStats();
            Assert.IsType<ViewResult>(result);
        }

        //TU10
        //MatchesController
        [Fact]
        public void PlayerAndStats_ReturnsViewResult()
        {
            var controller = new MatchesController(_context);
            var result = controller.PlayerAndStats();
            Assert.IsType<ViewResult>(result);
        }

        //TU11
        //MatchesController
        [Fact]
        public void Database_ModuleMatchesCSTest()
        {
            var controller = new MatchesController(_context);
            var result = controller.Results();
            var viewResult = Assert.IsType<ViewResult>(result);

            MatchesCS matchesCS = contextFixture.DbContext.MatchesCS.FirstOrDefault(m => m.MatchesCSId == contextFixture.GetMatchesCSId());
            var matchesResult = Assert.IsType<MatchesCS>(matchesCS);

            Assert.Equal(contextFixture.GetMatchesCSId(), matchesCS.MatchesCSId);
            Assert.Equal(736079, matchesCS.MatchesAPIID);
            Assert.Equal(contextFixture.DbContext.EventCS.FirstOrDefault(e => e.EventCSID == contextFixture.GetEventsCSId()), matchesCS.Event);
            Assert.Equal(10065, matchesCS.EventAPIID);
            Assert.Equal("Test", matchesCS.EventName);
            Assert.IsType<DateTime>(matchesCS.BeginAt);
            Assert.IsType<DateTime>(matchesCS.EndAt);
            Assert.False(matchesCS.IsFinished);
            Assert.IsType<TimeType>(matchesCS.TimeType);
            Assert.False(matchesCS.HaveStats);
            Assert.Null(matchesCS.MatchesList);
            Assert.Equal(1, matchesCS.NumberOfGames);
            Assert.Equal(contextFixture.GetScore(), matchesCS.Scores);
            Assert.Null(matchesCS.TeamsAPIIDList);
            Assert.Equal(1, matchesCS.WinnerTeamAPIId);
            Assert.Equal("Test1", matchesCS.WinnerTeamName);
            Assert.Equal('C', matchesCS.Tier);
            Assert.False(matchesCS.LiveSupported);
            Assert.Null(matchesCS.StreamList);
            Assert.Equal("Test", matchesCS.LeagueName);
            Assert.Equal(1, matchesCS.LeagueId);
            Assert.Null(matchesCS.LeagueLink);
        }

        //TU12
        //EventsController
        [Fact]
        public void Database_ModuleEventsCSTest()
        {
            var controller = new EventsController(_context);
            var result = controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);

            EventCS eventCS = contextFixture.DbContext.EventCS.FirstOrDefault(e => e.EventCSID == contextFixture.GetEventsCSId());
            var eventResult = Assert.IsType<EventCS>(eventCS);

            Assert.Equal(contextFixture.GetEventsCSId(), eventCS.EventCSID);
            Assert.Equal(10065, eventCS.EventAPIID);
            Assert.Equal("Test", eventCS.EventName);
            Assert.Equal("Test", eventCS.LeagueName);
			Assert.Equal("/images/iconMenu.jpg", eventCS.EventImage);
			Assert.Equal("Test", eventCS.EventLink);
            Assert.IsType<TimeType>(eventCS.TimeType);
            Assert.False(eventCS.Finished);
            Assert.IsType<DateTime>(eventCS.BeginAt);
            Assert.IsType<DateTime>(eventCS.EndAt);
            Assert.Equal(736079, eventCS.MatchesCSAPIID);
            Assert.Equal("1000000$", eventCS.PrizePool);
            Assert.Equal(736079, eventCS.MatchesCSAPIID);
            Assert.Equal("Test1", eventCS.WinnerTeamName);
            Assert.Equal('C', eventCS.Tier);
        }

		//TU13
		//ForumController
		[Fact]
		public void Forum_ReturnsViewResult()
		{
            var controller = new ForumController(_context, null);
            var result = controller.IndexAsyncTest();
            Assert.IsType<ViewResult>(result);
        }

        //TU14
        //ForumController
        [Fact]
		public void Profile_ReturnsViewResult()
		{
            var controller = new ForumController(_context, null);
			Profile profile = contextFixture.DbContext.Profiles.FirstOrDefault(p => p.Id == contextFixture.GetAdminId());
			var result = controller.Profile(profile.Id);
            Assert.IsType<ViewResult>(result);
        }

        //TU15
        //ForumController
        [Fact]
        public void CreateTopic_ReturnsViewResult()
        {
            var controller = new ForumController(_context, null);

			var result = controller.NewTopic();
            Assert.IsType<ViewResult>(result);

            contextFixture.CreateNewTopic();

            Topic topic = contextFixture.DbContext.Topics.FirstOrDefault(t => t.TopicId == contextFixture.GetTopicId());
            var topicResult = Assert.IsType<Topic>(topic);

            Assert.Equal(contextFixture.GetTopicId(), topic.TopicId);
            Assert.Equal("Test", topic.Title);
            Assert.Equal("Test123", topic.Content);
            Assert.IsType<DateTime>(topic.Date);
            Assert.Equal(contextFixture.GetAdminId(), topic.ProfileId);
            Assert.Equal(contextFixture.DbContext.Profiles.FirstOrDefault(p => p.Id == contextFixture.GetAdminId()), topic.Profile);
            Assert.IsType<List<Comment>>(topic.Comments);

            var finalResult = controller.TopicAsync(topic.TopicId);
            Assert.IsType<ViewResult>(finalResult);
        }
	}
}