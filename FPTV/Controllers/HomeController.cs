using System.Diagnostics;
using FPTV.Models;
using Microsoft.AspNetCore.Mvc;

namespace FPTV.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns the view Index
        /// </summary>
        /// <returns>View</returns>
        public IActionResult Index()
        {
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

        /// <summary>
        /// Returns the view RecoverPW
        /// </summary>
        /// <returns>View</returns>
        public IActionResult RecoverPW()
        {
            return View();
        }

        /// <summary>
        /// Returns the view SendEmail
        /// </summary>
        /// <returns>View</returns>
        public IActionResult SendEmail()
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