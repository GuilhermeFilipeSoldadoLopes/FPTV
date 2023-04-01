using System.Diagnostics;
using FPTV.Data;
using FPTV.Models;
using FPTV.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PusherServer;

namespace FPTV.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FPTVContext _context;
        private string dropDownGame;
		private string page;

		public HomeController(ILogger<HomeController> logger, FPTVContext context)
        {
            _logger = logger;
            _context = context;
        }

		public IActionResult VALORANT(string page = "Index")
		{
			return RedirectToAction(page, "Home", new { game = "valorant" });
		}

		public IActionResult CSGO(string page = "Index")
		{
			return RedirectToAction(page, "Home", new { game = "csgo" });
		}

		public IActionResult Index(string game = "csgo")
		{
			dropDownGame = game;
			page = "Index";
			var account = _context.Users.ToList().Count();

			var accountTxt = (account == 1) ? " user" : " users";

			ViewBag.dropDownGame = game;
			ViewBag.page = page;
			ViewData["accounts"] = account;
	        ViewData["account_txt"] = accountTxt;

			var visitors = 0;

			if (System.IO.File.Exists("visitors.txt"))
			{
				DateTime lastModificationFileDateTime = System.IO.File.GetLastWriteTime("visitors.txt");
				DateTime lasModificationDate =
					new DateTime(lastModificationFileDateTime.Year, lastModificationFileDateTime.Month, lastModificationFileDateTime.Day);

                Console.WriteLine(lasModificationDate);
                Console.WriteLine(DateTime.Compare(lasModificationDate, DateTime.Now.Date));

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

            //if (System.IO.File.Exists("isDarkmode.txt"))
            //{
            //    System.IO.File.WriteAllText("isDarkmode.txt", "true");
            //}
            //else
            //{
            //    System.IO.File.WriteAllText("isDarkmode.txt", "false");
            //}

            return View();
        }

        public IActionResult Events(string game = "csgo")
		{
			page = "Events";
			ViewBag.page = page;
			return RedirectToAction("Index", page, new { sort = "&sort=-begin_at", filter = "running", game = game });
        }

		public IActionResult ForumIndex()
		{
			return View();
		}
		public IActionResult ForumRules()
		{
			return View();
		}

		public IActionResult Matches(string game = "csgo")
		{
			page = "Matches";
			ViewBag.page = page;
			return RedirectToAction("Index", page, new { sort = "", filter = "", game = game });
		}

        public IActionResult Results(string game = "csgo")
		{
			page = "Matches";
			ViewBag.page = page;
			return RedirectToAction("Results", page, new {game = game });
		}

		public IActionResult CSGOStats()
		{
			page = "Index";
			return RedirectToAction("CSGOStats", "Stats");
		}

		public IActionResult PlayerAndStats()
		{
			page = "Index";
			return RedirectToAction("PlayerAndStats", "Matches");
		}

		public IActionResult TeamStats()
		{
			page = "Index";
			return RedirectToAction("TeamStats", "Matches");
		}

		public IActionResult LoginRegister()
		{
			page = "Index";
			return View();
        }
        
        public IActionResult Privacy()
		{
			page = "Index";
			return View();
        }
        public IActionResult ConfirmDelete()
		{
			page = "Index";
			return View();
        }

		public IActionResult MatchDetails()
		{
			page = "Index";
			return RedirectToAction("Matches", "MatchDetails");
		}

        public IActionResult Test()
		{
			page = "Index";
            return View();
        }
        
		public IActionResult Forum(string game = "csgo")
		{
			page = "Forum";
            ViewBag.page = page;
            ViewBag.dropDownGame = game;
            return View("Index"); //apagar index - quando a pagina tiver feita
		}
        public IActionResult About(string game = "csgo")
		{
			page = "About";
            ViewBag.page = page;
            ViewBag.dropDownGame = game;
			return View(); //return View(); //apagar index - quando a pagina tiver feita
		}
        
        public IActionResult Register()
		{
			page = "Index";
			return View();
        }

        public IActionResult SendEmail()
		{
			page = "Index";
			return View();
        }
        
        public IActionResult StatisticsOfSite()
		{
			page = "Index";
			return View();
        }
        public IActionResult Error404()
        {
            page = "Index";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
		{
			page = "Index";
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}