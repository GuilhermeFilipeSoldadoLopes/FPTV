using EllipticCurve.Utils;
using FPTV.Data;
using FPTV.Models.MatchModels;
using FPTV.Models.StatisticsModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
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
            getAPIMatches();

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

        private void getAPIMatches()
        {
            var client = new RestClient("https://api.pandascore.co/csgo/matches?sort=&page=1&per_page=50&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            RestResponse response = client.Execute(request);

            JArray matchesArray = JArray.Parse(response.Content);

            foreach (var item in matchesArray.Children<JObject>())
            {
                List<int> matchesList = new List<int>();
                List<int> teamsIdList = new List<int>();

                DateTime beginAt = (DateTime)item["begin_at"];

                bool haveStats = (bool)item["detailed_stats"];

                DateTime endAt = (DateTime)item["end_at"];

                JArray matchArray = (JArray)item["games"];
                foreach (var match in matchArray.Children<JObject>())
                {
                    matchesList.Add((int)match["id"]);
                }

                int matchesCSId = (int)item["id"];

                JObject league = (JObject)item["league"];
                string leagueName = (string)league["name"];
                int leagueId = (int)league["id"];
                string? leagueLink = (string)league["url"];

                JObject live = (JObject)item["live"];
                bool liveSupported = (bool)live["supported"];

                int numberOfGames = (int)item["number_of_games"];

                JArray opponentArray = (JArray)item["opponents"];
                foreach (var opponentObject in opponentArray.Children<JObject>())
                {
                    JObject opponent = (JObject)opponentObject["opponent"];
                    teamsIdList.Add((int)opponent["id"]);
                }

                var type = (string)item["status"];
                //TimeType timeType

                JArray streamArray = (JArray)item["streams_list"];
                //ICollection<Stream>? streamList

                JObject tournament = (JObject)item["tournament"];
                int eventId = (int)tournament["id"];
                string eventName = (string)tournament["name"];
                char tier = (char)tournament["tier"];

                int? winnerTeamId = (int)item["winner_id"];
                string? winnerTeamName = (string)item["winner"];

                bool isFinished = false;

                MatchesCS matches = new MatchesCS();
                _context.Add(matches);
            }
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
