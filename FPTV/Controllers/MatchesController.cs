using FPTV.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FPTV.Controllers
{
    public class MatchesController : Controller
    {
        private readonly FPTVContext _context;

        public MatchesController(FPTVContext context)
        {
            _context = context;
        }

        // GET: CS Matches
        public async Task<IActionResult> CSMatches()
        {
            var matchesCS = await _context.MatchesCS.ToListAsync();

            return View(matchesCS);
        }

        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int id)
        {
            return View();
        }

        // GET: Matches/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: Matches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
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
        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }

        // POST: Matches/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection)
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
        public async Task<IActionResult> Delete(int id)
        {
            return View();
        }

        // POST: Matches/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
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
