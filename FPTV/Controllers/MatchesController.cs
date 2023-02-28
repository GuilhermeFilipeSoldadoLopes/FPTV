using EllipticCurve.Utils;
using FPTV.Data;
using FPTV.Models.MatchModels;
using FPTV.Models.StatisticsModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SendGrid.Helpers.Mail;
using System.Text.RegularExpressions;

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
            var client = new RestClient("https://api.pandascore.co/csgo/matches?sort=&page=1&per_page=50&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            RestResponse response = client.Execute(request);

            JObject jObject = JObject.Parse(response.Content);

            DateTime beginAt = (DateTime)jObject["begin_at"];
            DateTime endAt = (DateTime)jObject["end_at"];
            int matchesCSId = (int)jObject["id"];
            int eventId = (int)jObject[""];
            string eventName = (string)jObject[""];
            bool isFinished = (bool)jObject[""];
            TimeType timeType = (TimeType)jObject[""];
            bool haveStats = (bool)jObject[""];
            ICollection<MatchCS> matchesList = ()jObject[""];
            int numberOfGames = (int)jObject["number_of_games"];
            List<int> teamsIdList = ()jObject[""];
            int? winnerTeamId = (int)jObject[""];
            string? winnerTeamName = (string)jObject[""];
            char tier = (char)jObject["tournament.tier"];
            bool liveSupported = (bool)jObject[""];
            ICollection<Stream>? streamList = ()jObject[""];
            string leagueName = (string)jObject["league.name"];
            int leagueId = (int)jObject["league_id"];
            string? leagueLink = (string)jObject["league.url"];



            ...





            var matchesCS = await _context.MatchesCS.ToListAsync();
            var matches = new List<MatchCS>();

            foreach (var CSMatches in matchesCS)
            {
                foreach (var CSMatch in CSMatches.MatchesList)
                {
                    matches.Add(CSMatch);
                }
            }

            return View(matches);
        }

        // GET: Val Matches
        public async Task<IActionResult> ValMatches()
        {
            var matchesVal = await _context.MatchesVal.ToListAsync();
            var matches = new List<MatchVal>();

            foreach (var ValMatches in matchesVal)
            {
                foreach (var ValMatch in ValMatches.MatchesList)
                {
                    matches.Add(ValMatch);
                }
            }

            return View(matches);
        }

        //De CS e de Valorant
        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int id)
        {
            return View();
        }

        //De CS e de Valorant
        // GET: Matches/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        //De CS e de Valorant
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

        //De CS e de Valorant
        // GET: Matches/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }

        //De CS e de Valorant
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

        //De CS e de Valorant
        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            return View();
        }

        //De CS e de Valorant
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
