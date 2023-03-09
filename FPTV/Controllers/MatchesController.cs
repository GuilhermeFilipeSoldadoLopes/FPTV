using AngleSharp.Common;
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
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using Stream = FPTV.Models.MatchModels.Stream;

namespace FPTV.Controllers
    //O sistema deverá permitir filtrar as partidas por
    //  estado(a decorrer, por decorrer, terminado)
    //  se a partida tem estatísticas
    //  se tem uma livestream da partida
    //  por eventos
    //O sistema deverá permitir ordenar as partidas por
    //  ordem cronológica
    //  por nome do evento
{
    public class MatchesController : Controller
    {
        private readonly FPTVContext _context;

        public MatchesController(FPTVContext context)
        {
            _context = context;
        }

        //De CSGO e de Valorant
        // GET: CSMatches
        public async Task<IActionResult> CSGOMatches()
        {
            List<MatchesCS> pastMatches = getAPICSGOMatches("https://api.pandascore.co/csgo/matches/past?sort=&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA");
            List<MatchesCS> runningMatches = getAPICSGOMatches("https://api.pandascore.co/csgo/matches/running?sort=&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA");
            List<MatchesCS> upcomingMatches = getAPICSGOMatches("https://api.pandascore.co/csgo/matches/upcoming?sort=&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA");

            List<int> dbMatchesIds = _context.MatchesCS.Select(m => m.MatchesCSAPIID).ToList();

            //Apenas os past são guardados, mas podem ser necessarios os running ou os upcomming
            foreach (var matches in pastMatches)
            {
                var id = matches.MatchesCSAPIID;

                if (!dbMatchesIds.Contains(id))
                {
                    _context.MatchesCS.Add(matches);

                    dbMatchesIds.Add(id);
                }
            }

            _context.SaveChanges();

            ViewBag.pastMatches = _context.MatchesCS;

            foreach (var item in _context.MatchesCS)
            {
                var c = item.TeamsAPIIDList;
                var a = item.TeamsAPIIDList.ElementAt(0);
                var b = item.TeamsAPIIDList.ElementAt(1);
            }

            //ViewBag.runningMatches = runningMatches;
            //ViewBag.upcomingMatches = upcomingMatches;

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
                    matches.TeamsAPIIDList = new List<int>();
                    matches.StreamList = new List<Stream>();

                    matches.MatchesCSAPIID = (int)item["id"];

                    matches.BeginAt = (DateTime)item["begin_at"];

                    matches.HaveStats = (bool)item["detailed_stats"];

                    var endAt = item["end_at"];

                    if (!endAt.ToString().Equals(""))
                        matches.EndAt = (DateTime)endAt;

                    JArray matchArray = (JArray)item["games"];
                    foreach (var match in matchArray.Children<JObject>())
                    {
                        MatchCS matchCS = new MatchCS();

                        matchCS.MatchesCSId = matches.MatchesCSId;
                        matchCS.MatchesCSAPIId = matches.MatchesCSAPIID;

                        matchCS.MatchCSAPIID = (int)match["id"];

                        //TODO

                        //matchCS.RoundsScore = (string)match[""];//Não esta na api
                        //matchCS.Map = (string)match[""];//Não esta na api

                        JObject winner = (JObject)match["winner"];
                        var winnerId = winner["id"];//id -> Guid
                        if (!winnerId.ToString().Equals(""))
                        {
                            matchCS.WinnerTeamAPIId = (int)winner["id"];
                            //matchCS.WinnerTeamName = (string)winner[""];//Não esta na api
                        }

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

                        var teamId = (int)opponent["id"];//id -> Guid

                        matches.TeamsAPIIDList.Add((int)opponent["id"]);
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
                    var eventId = (int)tournament["id"];//id -> Guid

                    matches.EventAPIID = (int)tournament["id"];
                    matches.EventName = (string)tournament["name"];
                    matches.Tier = (char)tournament["tier"];

                    var matchesWinner = item["winner"];

                    if(!matchesWinner.ToString().Equals(""))
                    {
                        var teamId = item["id"];//id -> Guid

                        matches.WinnerTeamAPIId = (int)item["id"];
                        matches.WinnerTeamName = (string)item["name"];
                    }

                    matchesCS.Add(matches);
                }
            }

            return matchesCS;
        }

        // GET: ValMatches
        public async Task<IActionResult> ValMatches()
        {
            List<MatchesVal> pastMatches = getAPIValMatches("https://api.pandascore.co/valorant/matches/past?sort=&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA");
            List<MatchesVal> runningMatches = getAPIValMatches("https://api.pandascore.co/valorant/matches/running?sort=&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA");
            List<MatchesVal> upcommingMatches = getAPIValMatches("https://api.pandascore.co/valorant/matches/upcoming?sort=&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA");

            List<int> dbMatchesIds = _context.MatchesVal.Select(m => m.MatchesValAPIID).ToList();

            //Apenas os past são guardados, mas podem ser necessarios os running ou os upcomming
            foreach (var matches in pastMatches)
            {
                var id = matches.MatchesValAPIID;

                if (!dbMatchesIds.Contains(id))
                {
                    //sss
                    _context.MatchesVal.Add(matches);

                    dbMatchesIds.Add(id);
                }
            }

            _context.SaveChanges();

            ViewBag["pastMatches"] = pastMatches;
            ViewBag["runningMatches"] = runningMatches;
            ViewBag["upcommingMatches"] = upcommingMatches;

            return View();
        }

        private List<MatchesVal> getAPIValMatches(string APIUrl)
        {
            List<MatchesVal> matchesVal = new List<MatchesVal>();

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
                    MatchesVal matches = new MatchesVal();

                    matches.MatchesList = new List<MatchVal>();
                    matches.TeamsAPIIDList = new List<int>();
                    matches.StreamList = new List<Stream>();

                    matches.MatchesValAPIID = (int)item["id"];

                    matches.BeginAt = (DateTime)item["begin_at"];

                    matches.HaveStats = (bool)item["detailed_stats"];

                    var endAt = item["end_at"];

                    if (!endAt.ToString().Equals(""))
                        matches.EndAt = (DateTime)endAt;

                    JArray matchArray = (JArray)item["games"];
                    foreach (var match in matchArray.Children<JObject>())
                    {
                        MatchVal matchVal = new MatchVal();

                        matchVal.MatchesValId = matches.MatchesValId;
                        matchVal.MatchesValAPIId = matches.MatchesValAPIID;

                        matchVal.MatchValAPIID = (int)match["id"];

                        //TODO

                        //matchVal.RoundsScore = (string)match[""];//Não esta na api
                        //matchVal.Map = (string)match[""];//Não esta na api

                        JObject winner = (JObject)match["winner"];
                        var winnerId = winner["id"];//id -> Guid
                        if (!winnerId.ToString().Equals(""))
                        {
                            matchVal.WinnerTeamAPIId = (int)winner["id"];
                            //matchVal.WinnerTeamName = (string)winner[""];//Não esta na api
                        }

                        matches.MatchesList.Add(matchVal);
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

                        var teamId = (int)opponent["id"];//id -> Guid

                        matches.TeamsAPIIDList.Add((int)opponent["id"]);
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
                    var eventId = (int)tournament["id"];//id -> Guid

                    matches.EventAPIID = (int)tournament["id"];
                    matches.EventName = (string)tournament["name"];
                    matches.Tier = (char)tournament["tier"];

                    var matchesWinner = item["winner"];

                    if (!matchesWinner.ToString().Equals(""))
                    {
                        var teamId = item["id"];//id -> Guid

                        matches.WinnerTeamAPIId = (int)item["id"];
                        matches.WinnerTeamName = (string)item["name"];
                    }

                    matchesVal.Add(matches);
                }
            }

            return matchesVal;
        }

        //De CSGO e de Valorant
        // GET: Matches/CSMatcheDetails/5
        public async Task<IActionResult> CSMatcheDetails(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.MatchesCS.Include(m => m.MatchesList).Include(m => m.TeamsAPIIDList).Include(m => m.StreamList).FirstOrDefaultAsync(m => m.MatchesCSAPIID == id);

            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // GET: Matches/ValMatcheDetails/5
        public async Task<IActionResult> ValMatcheDetails(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.MatchesVal.Include(m => m.MatchesList).Include(m => m.TeamsIdList).Include(m => m.StreamList).FirstOrDefaultAsync(m => m.MatchesValId == id);

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
