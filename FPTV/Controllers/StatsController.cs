using AngleSharp.Io;
using FPTV.Data;
using FPTV.Models.MatchModels;
using FPTV.Models.StatisticsModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Linq.Expressions;

namespace FPTV.Controllers
{
    public class StatsController : Controller
    {
        private readonly FPTVContext _context;

        public StatsController(FPTVContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> PlayerAndStats(string gameType)
        {
            switch (gameType)
            {
                case "csgo":
                    var playerStatsCs = await _context.MatchPlayerStatsCS.ToListAsync();
                    var statsCs = new List<MatchPlayerStatsCS>();

                    foreach (var staCs in statsCs)
                    {
                        statsCs.Add(staCs);
                    }

                    return View(statsCs);

                case "valorant":
                    var playerStatsVal = await _context.MatchPlayerStatsVal.ToListAsync();
                    var statsVal = new List<MatchPlayerStatsVal>();

                    foreach (var staVal in statsVal)
                    {
                        statsVal.Add(staVal);
                    }

                    return View(statsVal);

                default:
                    return null; 
            }
        }

        public async Task<ActionResult> TeamAndStats(string gameType)
        {
            switch (gameType)
            {
                case "csgo":
                    var teamStatsCs = await _context.MatchTeamsCS.ToListAsync();
                    var statsTeamCs = new List<MatchTeamsCS>();

                    foreach (var staTeamCs in statsTeamCs)
                    {
                        statsTeamCs.Add(staTeamCs);
                    }

                    return View(statsTeamCs);

                case "valorant":
                    var teamStatsVal = await _context.MatchTeamsVal.ToListAsync();
                    var statsTeamVal = new List<MatchTeamsVal>();

                    foreach (var staTeamVal in statsTeamVal)
                    {
                        statsTeamVal.Add(staTeamVal);
                    }

                    return View(statsTeamVal);

                default:
                    return null;
            }
        }

        public Task<ActionResult> MatchPlayerStatsCS()
        {
            var client = new RestClient("");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            RestResponse response = client.Execute(request);

            JObject jObject = JObject.Parse(response.Content);
            JArray gamesArray = (JArray)jObject["games"];

            MatchPlayerStatsCS auxMatchPlayerStatsCS = new MatchPlayerStatsCS()

            foreach (JObject gameObject in gamesArray)
            {
                int MatchCSId = (int)jObject["match_id"];
                int PlayerCSId = (int)jObject["player_id"];
                int Kills = (DateTime)jObject["kills"];
                int Deaths = (DateTime)jObject["deaths"];
                int Assists = (DateTime)jObject["assists"];
                int FlashAssist = (DateTime)jObject["flash_assists"];
                int ADR = (DateTime)jObject["adr"];
                int HeadShots = (DateTime)jObject["headshots"];
                int KD_Diff = (DateTime)jObject["k_d_diff"];
                int PlayerName = (DateTime)jObject["player_id"];
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: StatsController/Create
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

        // GET: StatsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StatsController/Edit/5
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

        // GET: StatsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StatsController/Delete/5
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
