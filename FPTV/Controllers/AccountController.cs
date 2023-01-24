using Microsoft.AspNetCore.Mvc;

namespace FPTV.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
