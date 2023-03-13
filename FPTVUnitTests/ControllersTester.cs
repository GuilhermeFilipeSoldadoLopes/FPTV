using FPTV.Controllers;
using FPTV.Data;
using Microsoft.AspNetCore.Mvc;

namespace FPTVUnitTests
{
    //Classe de testes dos controladores (Home, Events, Matches, Stats)
    public class ControllersTester : IClassFixture<ApplicationDbContextFixture>
    {
        private FPTVContext _context;

        public ControllersTester(ApplicationDbContextFixture context)
        {
            _context = context.DbContext;
        }

        //HomeController
        [Fact]
        public void Index_ReturnsViewResult()
        {
            var controller = new HomeController(null, _context);
            var result = controller.Index();
            Assert.IsType<ViewResult>(result);
        }

        //HomeController
        [Fact]
        public void Privacy_ReturnsViewResult()
        {
            var controller = new HomeController(null, _context);
            var result = controller.Privacy();
            Assert.IsType<ViewResult>(result);
        }

        //HomeController
        [Fact]
        public void ConfirmDelete_ReturnsViewResult()
        {
            var controller = new HomeController(null, _context);
            var result = controller.ConfirmDelete();
            Assert.IsType<ViewResult>(result);
        }

        //EventsController
        [Fact]
        public void Events_ReturnsViewResult()
        {
            var controller = new EventsController();
            var result = controller.Index();
            Assert.IsType<ViewResult>(result);
        }

        //EventsController
        [Fact]
        public void EventDetails_ReturnsViewResult()
        {
            var controller = new EventsController();
            var result = controller.Details(1);
            Assert.IsType<ViewResult>(result);
        }

        //HomeController -> MatchesController
        [Fact]
        public void Matches_ReturnsViewResult()
        {
            var controller = new HomeController(null, _context);
            var result = controller.Matches();
            Assert.IsType<ViewResult>(result);
        }

        //HomeController -> MatchesController
        [Fact]
        public void MatchesDetails_ReturnsViewResult()
        {
            var controller = new HomeController(null, _context);
            var result = controller.MatchDetails();
            Assert.IsType<ViewResult>(result);
        }

        //HomeController -> StatsController
        [Fact]
        public void Results_ReturnsViewResult()
        {
            var controller = new HomeController(null, _context);
            var result = controller.Results();
            Assert.IsType<ViewResult>(result);
        }

        //HomeController -> StatsController
        [Fact]
        public void TeamStats_ReturnsViewResult()
        {
            var controller = new HomeController(null, _context);
            var result = controller.TeamStats();
            Assert.IsType<ViewResult>(result);
        }

        //HomeController -> StatsController
        [Fact]
        public void PlayerAndStats_ReturnsViewResult()
        {
            var controller = new HomeController(null, _context);
            var result = controller.PlayerAndStats();
            Assert.IsType<ViewResult>(result);
        }
    }
}