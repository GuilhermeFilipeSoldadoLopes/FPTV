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

        public IActionResult Index()
        {
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}