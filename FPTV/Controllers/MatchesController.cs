using EllipticCurve.Utils;
using FPTV.Data;
using FPTV.Models.MatchModels;
using FPTV.Models.StatisticsModels;
using FPTV.Models.ToReview;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Stream = FPTV.Models.MatchModels.Stream;

namespace FPTV.Controllers
{
    public class MatchesController : Controller
    {
        private readonly FPTVContext _context;

        public MatchesController(FPTVContext context)
        {
            _context = context;
        }

        public ActionResult Matches()
        {
            return View();
        }

        //De CSGO e de Valorant
        // GET: CSMatches
        public async Task<IActionResult> CSGOMatches()
        {
            List<MatchesCS> pastMatches = getAPICSGOMatches("https://api.pandascore.co/csgo/matches/past?sort=&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA");
            List<MatchesCS> runningMatches = getAPICSGOMatches("https://api.pandascore.co/csgo/matches/running?sort=&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA");
            List<MatchesCS> upcommingMatches = getAPICSGOMatches("https://api.pandascore.co/csgo/matches/upcoming?sort=&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA");

            foreach (var matches in pastMatches)
            {
                _context.MatchesCS.Add(matches);
            }

            return View();
        }

        private List<MatchesCS> getAPICSGOMatches(string APIUrl)
        {
            List<MatchesCS> matchesCS = new List<MatchesCS>();

            var client = new RestClient(APIUrl);
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            RestResponse response = client.Execute(request);

            JArray matchesArray = JArray.Parse(response.Content);

            foreach (var item in matchesArray.Children<JObject>())
            {
                var status = (string)item["status"];

                if (!status.Equals("canceled"))
                {
                    MatchesCS matches = new MatchesCS();

                    matches.MatchesList = new List<MatchCS>();
                    matches.TeamsIdList = new List<Guid>();
                    matches.StreamList = new List<Stream>();

                    matches.BeginAt = (DateTime)item["begin_at"];

                    matches.HaveStats = (bool)item["detailed_stats"];

                    var endAt = item["end_at"];

                    if (endAt != null)
                        matches.EndAt = (DateTime)endAt;

                    JArray matchArray = (JArray)item["games"];
                    foreach (var match in matchArray.Children<JObject>())
                    {
                        MatchCS matchCS = new MatchCS();

                        matchCS.MatchesCSId = matches.MatchesCSId;

                        //TODO

                        matches.MatchesList.Add(matchCS);
                    }

                    JObject league = (JObject)item["league"];
                    matches.LeagueName = (string)league["name"];
                    matches.LeagueId = (int)league["id"];
                    matches.LeagueLink = (string)league["url"];

                    JObject live = (JObject)item["live"];
                    matches.LiveSupported = (bool)live["supported"];

                    matches.NumberOfGames = (int)item["number_of_games"];

                    JArray opponentArray = (JArray)item["opponents"];
                    foreach (var opponentObject in opponentArray.Children<JObject>())
                    {
                        JObject opponent = (JObject)opponentObject["opponent"];

                        //matches.TeamsIdList.Add((string)opponent["id"]);//id -> Guid
                    }

                    matches.IsFinished = false;

                    matches.TimeType = TimeType.Running;
                    if (status.Equals("finished"))
                    {
                        matches.IsFinished = true;
                        matches.TimeType = TimeType.Past;
                    }
                    if (status.Equals("not_started"))
                        matches.TimeType = TimeType.Upcoming;

                    JArray streamArray = (JArray)item["streams_list"];
                    foreach (var streamObject in streamArray.Children<JObject>())
                    {
                        Stream stream = new Stream();

                        stream.StreamLink = (string)streamObject["raw_url"];
                        stream.StreamLanguage = (string)streamObject["language"];

                        matches.StreamList.Add(stream);
                    }

                    JObject tournament = (JObject)item["tournament"];
                    matches.EventId = (Guid)tournament["id"];//id -> Guid
                    matches.EventName = (string)tournament["name"];
                    matches.Tier = (char)tournament["tier"];

                    matches.WinnerTeamId = (Guid)item["winner_id"];
                    matches.WinnerTeamName = (string)item["winner"];

                    matchesCS.Add(matches);
                }
            }

            return matchesCS;
        }

        private List<MatchesCS> getAPIRunningMatchesCSGO()
        {
            List<MatchesCS> runningMatches = new List<MatchesCS>();



            return runningMatches;
        }

        private List<MatchesCS> getAPIUpcommingMatchesCSGO()
        {
            List<MatchesCS> upcommingMatches = new List<MatchesCS>();



            return upcommingMatches;
        }

        //De CSGO e de Valorant
        // GET: Matches/CSMatcheDetails/5
        public async Task<IActionResult> CSMatcheDetails(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.MatchesCS.Include(m => m.MatchesList).Include(m => m.TeamsIdList).Include(m => m.StreamList).FirstOrDefaultAsync(m => m.MatchesCSId == id);

            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        /*//De CSGO e de Valorant
        // GET: Matches/CSMatcheCreate
        public async Task<IActionResult> CSMatcheCreate()
        {
            return View();
        }

        //De CSGO e de Valorant
        // POST: Matches/CSMatcheCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CSMatcheCreate(IFormCollection collection)
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

        //De CSGO e de Valorant
        // GET: Matches/CSMatcheEdit/5
        public async Task<IActionResult> CSMatcheEdit(int id)
        {
            return View();
        }

        //De CSGO e de Valorant
        // POST: Matches/CSMatcheEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CSMatcheEdit(int id, IFormCollection collection)
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

        //De CSGO e de Valorant
        // GET: Matches/CSMatcheDelete/5
        public async Task<IActionResult> CSMatcheDelete(int id)
        {
            return View();
        }

        //De CSGO e de Valorant
        // POST: Matches/CSMatcheDelete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CSMatcheDelete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/
    }
}
