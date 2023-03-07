using System.Diagnostics;
using FPTV.Data;
using FPTV.Models;
using FPTV.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PusherServer;

namespace FPTV.Controllers
{
    /// <summary>
    /// Controller for the principal actions
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FPTVContext _context;

        /// <summary>
        /// Controller constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
		public HomeController(ILogger<HomeController> logger, FPTVContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Returns the view Index
        /// <returns>View</returns>
        public IActionResult Index()
        {
	        var account = _context.Users.ToList().Count();

			var accountTxt = (account == 1) ? " user" : " users";

			ViewData["accounts"] = account;
	        ViewData["account_txt"] = accountTxt;

			var visitors = 0;

			if (System.IO.File.Exists("visitors.txt"))
			{
				DateTime lastModificationFileDateTime = System.IO.File.GetLastWriteTime("visitors.txt");
				DateTime lasModificationDate =
					new DateTime(lastModificationFileDateTime.Year, lastModificationFileDateTime.Month, lastModificationFileDateTime.Day);
				int days = (DateTime.Now - lasModificationDate).Days;

				if (DateTime.Compare(lasModificationDate, DateTime.Now.Date) < 0)
				{
					System.IO.File.WriteAllText("visitors.txt", 0.ToString());
				}

				string noOfVisitors = System.IO.File.ReadAllText("visitors.txt");
				visitors = Int32.Parse(noOfVisitors);
			}

			++visitors;
			var visitText = (visitors == 1) ? " view" : " views";

			if (System.IO.File.Exists("visitors.txt"))
			{
				System.IO.File.WriteAllText("visitors.txt", visitors.ToString());

			}
			else
			{
				System.IO.File.WriteAllText("visitors.txt", 1.ToString());
			}

			var options = new PusherOptions();
			options.Cluster = "PUSHER_APP_CLUSTER";

			var pusher = new Pusher(
				"PUSHER_APP_ID",
				"PUSHER_APP_KEY",
				"PUSHER_APP_SECRET", options);

			pusher.TriggerAsync(
				"general",
				"newVisit",
				new { visits = visitors.ToString(), message = visitText });

			ViewData["visitors"] = visitors;
			ViewData["visitors_txt"] = visitText;

			return View();
        }

        /// <summary>
        /// Returns the view Game
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Game()
        {
            return View();
        }

        /// <summary>
        /// Returns the view Matches
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Matches()
        {
            return View();
        }

        /// <summary>
        /// Returns the view Results
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Results()
        {
            return View();
        }

        /// <summary>
        /// Returns the view Events
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Events()
        {
            return View();
        }

        /// <summary>
        /// Returns the view EventDetails
        /// </summary>
        /// <returns>View</returns>
		public IActionResult EventDetails()
		{
			return View();
        }

        /// <summary>
        /// Returns the view Forum
        /// </summary>
        /// <returns>View</returns>
		public IActionResult Forum()
        {
            return View();
        }

        /// <summary>
        /// Returns the view About
        /// </summary>
        /// <returns>View</returns>
        public IActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Returns the view LoginRegister
        /// </summary>
        /// <returns>View</returns>
        public IActionResult LoginRegister()
        {
            return View();
        }

        /// <summary>
        /// Returns the view Register
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Register()
        {
            return View();
        }

        /*
        /// <summary>
        /// Returns the view ForgotPassword
        /// </summary>
        /// <returns>View</returns>
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Returns the view ForgotPasswordConfirmation
        /// </summary>
        /// <returns>View</returns>
        public IActionResult ForgotPasswordConfirmation()
        {
	        return View();
		}
        */

        /// <summary>
        /// Returns the view SendEmail
        /// </summary>
        /// <returns>View</returns>
        public IActionResult SendEmail()
        {
            return View();
        }

        /// <summary>
        /// Returns the view Privacy
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Returns the view TeamStats
        /// </summary>
        /// <returns>View</returns>
        public IActionResult TeamStats()
        {
            return View();
        }

        /// <summary>
        /// Returns the view PlayerAndStats
        /// </summary>
        /// <returns>View</returns>
        public IActionResult PlayerAndStats()
        {
            return View();
        }

        /// <summary>
        /// Returns the view StatisticsOfSite
        /// </summary>
        /// <returns>View</returns>
        public IActionResult StatisticsOfSite()
        {
            return View();
        }

        /// <summary>
        /// Returns the view Error
        /// </summary>
        /// <returns>View</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}