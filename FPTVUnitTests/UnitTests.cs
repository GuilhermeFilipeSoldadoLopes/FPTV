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
        public void Database_ModuleMatchesTest()
        {
            var controller = new MatchesController(_context);
            var result = controller.CSGOMatches();

            var viewResult = Assert.IsType<ViewResult>(result);

            /*
            var model1 = Assert.IsAssignableFrom<IEnumerable<MatchesCS>>(
                viewResult.ViewData.Model);
            Assert.Equal(24, model1.Count());
            
            var model2 = Assert.IsAssignableFrom<MatchesCS>(viewResult.ViewData.Model);
            Assert.Equal(contextFixture.GetMatchesCSId().ToString(), model2.MatchesCSId.ToString());
             */

            //Assert.Null(contextFixture.DbContext.EventCS.Find(contextFixture.GetMatchesCSId()));
            // os dados retornados da BD sao null

            Assert.Equal(contextFixture.GetMatchesCSId().ToString(), contextFixture.DbContext.EventCS.Find(contextFixture.GetMatchesCSId()).ToString()); 
            Assert.Equal(1.ToString(), controller.ViewBag.MatchesCSAPIID.ToString());
            Assert.Equal(contextFixture.GetEventsCSId().ToString(), controller.ViewBag.EventId.ToString());
            Assert.Equal(10065.ToString(), controller.ViewBag.EventAPIID.ToString());
            Assert.Equal("Test".ToString(), controller.ViewBag.EventName.ToString());
            Assert.IsType<DateTime>(controller.ViewBag.BeginAt);
            Assert.IsType<DateTime>(controller.ViewBag.EndAt);
            Assert.False(controller.ViewBag.IsFinished.ToString());
            Assert.IsType<TimeType>(controller.ViewBag.TimeType);
            Assert.False(controller.ViewBag.HaveStats);
            Assert.Null(controller.ViewBag.MatchesList);
            Assert.Equal(1.ToString(), controller.ViewBag.NumberOfGames.ToString());
            Assert.Equal(contextFixture.GetScore().ToString(), controller.ViewBag.Score.ToString());
            Assert.Null(controller.ViewBag.TeamsIDList);
            Assert.Null(controller.ViewBag.TeamsAPIIDList);
            Assert.Equal(contextFixture.GetWinnerTeamId().ToString(), controller.ViewBag.WinnerTeamId.ToString());
            Assert.Equal(1.ToString(), controller.ViewBag.WinnerTeamAPIId.ToString());
            Assert.Equal("SAW".ToString(), controller.ViewBag.WinnerTeamName.ToString());
            Assert.Equal('C'.ToString(), controller.ViewBag.Tier.ToString());
            Assert.False(controller.ViewBag.LiveSupported);
            Assert.Null(controller.ViewBag.StreamList);
            Assert.Equal("Test".ToString(), controller.ViewBag.LeagueName.ToString());
            Assert.Equal(1.ToString(), controller.ViewBag.LeagueId.ToString());
            Assert.Null(controller.ViewBag.LeagueLink);
        }

        /*[Fact]
        public void Database_ModuleEventTest()
        {
            var controller = new EventsController();
            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<EventCS>>(
                viewResult.ViewData.Model);
            Assert.Equal(17, model.Count());
        }*/
    }
}