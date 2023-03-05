using System.Diagnostics;
using FPTV.Data;
using FPTV.Models;
using FPTV.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PusherServer;

namespace FPTV.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FPTVContext _context;

		public HomeController(ILogger<HomeController> logger, FPTVContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
	        var account = _context.Users.ToList().Count();

			var account_txt = (account == 1) ? " user" : " users";

			ViewData["accounts"] = account;
	        ViewData["account_txt"] = account_txt;

			var visitors = 0;

			if (System.IO.File.Exists("visitors.txt"))
			{
				string noOfVisitors = System.IO.File.ReadAllText("visitors.txt");
				visitors = Int32.Parse(noOfVisitors);
			}

			++visitors;
			var visit_text = (visitors == 1) ? " view" : " views";

			System.IO.File.WriteAllText("visitors.txt", visitors.ToString());

			var options = new PusherOptions();
			options.Cluster = "PUSHER_APP_CLUSTER";

			var pusher = new Pusher(
				"PUSHER_APP_ID",
				"PUSHER_APP_KEY",
				"PUSHER_APP_SECRET", options);

			pusher.TriggerAsync(
				"general",
				"newVisit",
				new { visits = visitors.ToString(), message = visit_text });

			ViewData["visitors"] = visitors;
			ViewData["visitors_txt"] = visit_text;

			return View();
        }
        public IActionResult Game()
        {
            return View();
        }

        public IActionResult Matches()
        {
            return View();
        }
        public IActionResult Results()
        {
            return View();
        }
        public IActionResult Events()
        {
            return View();
        }
		public IActionResult EventDetails()
		{
			return View();
		}
		public IActionResult Forum()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult LoginRegister()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult RecoverPW()
        {
            return View();
        }
        public IActionResult SendEmail()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult TeamStats()
        {
            return View();
        }
        public IActionResult PlayerAndStats()
        {
            return View();
        }
        public IActionResult StatisticsOfSite()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}