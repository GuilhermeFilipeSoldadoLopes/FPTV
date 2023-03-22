using FPTV.Controllers;
using FPTV.Data;
using FPTV.Models.EventsModels;
using FPTV.Models.MatchesModels;
using Microsoft.AspNetCore.Mvc;

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
            var result = controller.Index();
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
            var controller = new EventsController();
            var result = controller.Index();
            Assert.IsType<ViewResult>(result);
        }

        //TU5
        //EventsController
        [Fact]
        public void EventDetails_ReturnsViewResult()
        {
            var controller = new EventsController();
            var result = controller.Details(1);
            Assert.IsType<ViewResult>(result);
        }

        //TU6
        //HomeController -> MatchesController
        [Fact]
        public void Matches_ReturnsViewResult()
        {
            var controller = new MatchesController(_context);
            var result = controller.CSGOMatches();
            Assert.IsType<ViewResult>(result);
        }

        //TU7
        //HomeController -> MatchesController
        [Fact]
        public void MatchesDetails_ReturnsViewResult()
        {
            var controller = new HomeController(null, _context);
            var result = controller.MatchDetails();
            Assert.IsType<ViewResult>(result);
        }

        //TU8
        //HomeController -> StatsController
        [Fact]
        public void Results_ReturnsViewResult()
        {
            var controller = new HomeController(null, _context);
            var result = controller.Results();
            Assert.IsType<ViewResult>(result);
        }

        //TU9
        //HomeController -> StatsController
        [Fact]
        public void TeamStats_ReturnsViewResult()
        {
            var controller = new HomeController(null, _context);
            var result = controller.TeamStats();
            Assert.IsType<ViewResult>(result);
        }

        //TU10
        //HomeController -> StatsController
        [Fact]
        public void PlayerAndStats_ReturnsViewResult()
        {
            var controller = new HomeController(null, _context);
            var result = controller.PlayerAndStats();
            Assert.IsType<ViewResult>(result);
        }

        //TU11
        //MatchesController
        [Fact]
        public void Database_ModuleMatchesCSTest()
        {
            var controller = new MatchesController(_context);
            var result = controller.CSGOMatches();
            var viewResult = Assert.IsType<ViewResult>(result);

            MatchesCS matchesCS = contextFixture.DbContext.MatchesCS.FirstOrDefault(m => m.MatchesCSId == contextFixture.GetMatchesCSId());
            var matchesResult = Assert.IsType<MatchesCS>(matchesCS);

            Assert.Equal(contextFixture.GetMatchesCSId(), matchesCS.MatchesCSId);
            Assert.Equal(1, matchesCS.MatchesCSAPIID);
            Assert.Equal(contextFixture.DbContext.EventCS.FirstOrDefault(e => e.EventCSID == contextFixture.GetEventsCSId()), matchesCS.EventCS);
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
            Assert.Equal(contextFixture.GetTeamsList(), matchesCS.TeamsList);
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
            var controller = new EventsController();
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
            Assert.Equal(contextFixture.GetTeamsList(), eventCS.TeamsList);
            Assert.Equal("1000000$", eventCS.PrizePool);
            Assert.Equal(1, eventCS.MatchesCSAPIID);
            Assert.Equal("Test1", eventCS.WinnerTeamName);
            Assert.Equal('C', eventCS.Tier);
        }
	}
}