using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTV.Controllers
{
    public class Matches : Controller
    {
        // GET: Matches
        public ActionResult Index()
        {
            return View();
        }

        // GET: Matches/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Matches/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Matches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Matches/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Matches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Matches/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Matches/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
