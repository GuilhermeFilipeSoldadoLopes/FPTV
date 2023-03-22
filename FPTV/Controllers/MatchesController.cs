using AngleSharp.Common;
using AngleSharp.Dom;
using EllipticCurve.Utils;
using FPTV.Data;
using FPTV.Models.EventsModels;
using FPTV.Models.MatchesModels;
using FPTV.Models.StatisticsModels;
using FPTV.Models.ToReview;
using FPTV.Models.UserModels;
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
using System.ComponentModel;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using static System.Formats.Asn1.AsnWriter;
using Stream = FPTV.Models.MatchesModels.Stream;

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
        public async Task<ActionResult> CSGOMatches(string sort = "", string filter = "", string page = "&page=1", string game = "csgo")
        {
            //Request processing with RestSharp
            var jsonFilter = (filter == "" || filter == "livestream") ? "" : "filter[" + filter + "]=true&";
            var jsonSort = (sort == "" || sort == "tournament") ? "" : sort;
            var jsonPage = page;
            var jsonPerPage = "&per_page=10";
            var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
            var requestLink = "https://api.pandascore.co/" + game + "/matches/";

            var fullApiPath = requestLink + "past?" + jsonFilter + jsonSort + jsonPage + jsonPerPage + token;
            List<MatchesCS> pastMatches = getAPICSGOMatches(fullApiPath, game);
            fullApiPath = requestLink + "running?" + jsonFilter + jsonSort + jsonPage + jsonPerPage + token;
            List<MatchesCS> runningMatches = getAPICSGOMatches(fullApiPath, game);
            fullApiPath = requestLink + "upcoming?" + jsonFilter + jsonSort + jsonPage + jsonPerPage + token;
            List<MatchesCS> upcomingMatches = getAPICSGOMatches(fullApiPath, game);

            List<int> dbMatchesIds = _context.MatchesCS.Select(m => m.MatchesCSAPIID).ToList();

            foreach (var match in pastMatches)
            {
                Console.WriteLine(match.MatchesCSAPIID);

                foreach (var item in match.TeamsList)
                {
                    Console.WriteLine(item.Name);
                }
                Console.WriteLine("");

                if (!dbMatchesIds.Contains(match.MatchesCSAPIID))
                {
                    _context.MatchesCS.Add(match);
                }
            }

            //para nao dar erro
            await _context.SaveChangesAsync();

            if (sort == "tournament")
            {
                pastMatches = pastMatches.OrderBy(m => m.EventName).ToList();
                runningMatches = runningMatches.OrderBy(m => m.EventName).ToList();
                upcomingMatches = upcomingMatches.OrderBy(m => m.EventName).ToList();
            }

            if (filter == "livestream")
            {
                pastMatches = pastMatches.Where(m => m.LiveSupported == true).ToList();
                runningMatches = runningMatches.Where(m => m.LiveSupported == true).ToList();
                upcomingMatches = upcomingMatches.Where(m => m.LiveSupported == true).ToList();
            }

            var a = await _context.MatchesCS.ToListAsync();

            /*foreach (var item in a)
            {
                var b = item.TeamsList;

                foreach (var item2 in b)
                {
                    var c = item2.Name;
                }
            }*/

            ViewBag.pastMatches = pastMatches;

            ViewBag.runningMatches = runningMatches;
            ViewBag.upcomingMatches = upcomingMatches;

            ViewBag.filter = filter;
            ViewBag.sort = sort;

            return View();
        }

        private async void getPastMatches(string fullApiPath)
        {
            List<MatchesCS> pastMatches = getAPICSGOMatches(fullApiPath, "");
            List<int> dbMatchesIds = _context.MatchesCS.Select(m => m.MatchesCSAPIID).ToList();

            foreach (var matches in pastMatches)
            {
                var id = matches.MatchesCSAPIID;

                if (!dbMatchesIds.Contains(id))
                {
                    _context.MatchesCS.Add(matches);

                    dbMatchesIds.Add(id);
                }
            }

            await _context.SaveChangesAsync();
        }

        private List<MatchesCS> getAPICSGOMatches(string fullApiPath, string game)
        {
            //Request processing with RestSharp
            var client = new RestClient(fullApiPath);
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            var json = client.Execute(request).Content;

            if (json == null)
            {
                return null;
            }

            List<MatchesCS> matchesCS = new List<MatchesCS>();

            var matchesArray = JArray.Parse(json);

            foreach (var item in matchesArray.Cast<JObject>())
            {
                var status = item.GetValue("status");

                if (!status.ToString().Equals("canceled"))
                {
                    MatchesCS matches = new MatchesCS();
                    matches.TeamsAPIIDList = new List<int>();

                    matches.Scores = new List<Score>();
                    matches.TeamsList = new List<Team>();
                    matches.StreamList = new List<Stream>();

                    //Set up values from api
                    var league = (JObject)item.GetValue("league");
                    var live = (JObject)item.GetValue("live");
                    var tournament = (JObject)item.GetValue("tournament");
                    var winner = item.GetValue("winner");
                    var results = (JArray)item.GetValue("results");
                    var opponentArray = (JArray)item.GetValue("opponents");
                    var streamArray = (JArray)item.GetValue("streams_list");

                    var matchesCSId = item.GetValue("id");
                    var eventAPIID = tournament.GetValue("id");
                    var eventName = tournament.GetValue("name");
                    var beginAt = item.GetValue("begin_at");
                    var endAt = item.GetValue("end_at");
                    var haveStats = item.GetValue("detailed_stats");
                    var numberOfGames = item.GetValue("number_of_games");
                    var winnerTeamAPIId = winner.ToString() == "" ? null : winner.ToObject<JObject>().GetValue("id");
                    var winnerTeamName = winner.ToString() == "" ? null : winner.ToObject<JObject>().GetValue("name");
                    var tier = tournament.GetValue("tier");
                    var leagueName = league.GetValue("name");
                    var LeagueId = league.GetValue("id");
                    var leagueLink = league.GetValue("url");

                    //Handling for null values
                    matches.LeagueName = leagueName.ToString() == null ? "" : leagueName.Value<string>();
                    matches.MatchesCSAPIID = matchesCSId.ToString() == null ? -1 : matchesCSId.Value<int>();
                    matches.EventAPIID = eventAPIID.ToString() == null ? -1 : eventAPIID.Value<int>();
                    matches.EventName = eventName.ToString() == null ? "" : matches.LeagueName + " " + eventName.Value<string>();
                    matches.BeginAt = beginAt.ToString() == "" ? new DateTime() : beginAt.Value<DateTime>();
                    matches.EndAt = endAt.ToString() == "" ? new DateTime() : endAt.Value<DateTime>();
                    matches.IsFinished = status.ToString() == "finished" ? true : false;
                    matches.HaveStats = haveStats.ToString() == "true" ? true : false;
                    matches.NumberOfGames = numberOfGames.ToString() == null ? 1 : numberOfGames.Value<int>();
                    var aa = winnerTeamAPIId;
                    matches.WinnerTeamAPIId = winnerTeamAPIId == null ? -1 : winnerTeamAPIId.Value<int>();
                    matches.WinnerTeamName = winnerTeamName == null ? "" : winnerTeamName.Value<string>();
                    matches.Tier = tier.ToString() == "unranked" ? ' ' : tier.Value<char>();
                    matches.LeagueId = LeagueId.ToString() == null ? -1 : LeagueId.Value<int>();
                    matches.LeagueLink = leagueLink.ToString() == null ? "" : leagueLink.Value<string>();

                    var eventCS = new EventCS();
                    eventCS.EventAPIID = matches.EventAPIID;
                    eventCS.BeginAt = new DateTime();
                    eventCS.EndAt = new DateTime();
                    eventCS.EventName = matches.EventName;
                    eventCS.TimeType = TimeType.Running;
                    eventCS.Finished = false;
                    eventCS.MatchesCSAPIID = matches.MatchesCSAPIID;
                    eventCS.EventImage = "";
                    eventCS.EventLink = "";
                    eventCS.LeagueName = "";
                    eventCS.PrizePool = "";
                    eventCS.Tier = ' ';
                    eventCS.WinnerTeamAPIID = 1;
                    eventCS.WinnerTeamName = "";
                    matches.EventCS = eventCS;

                    matches.TimeType = TimeType.Running;
                    if (status.Equals("finished"))
                    {
                        matches.IsFinished = true;
                        matches.TimeType = TimeType.Past;
                    }
                    if (status.Equals("not_started"))
                        matches.TimeType = TimeType.Upcoming;

                    foreach (var streamObject in streamArray.Cast<JObject>())
                    {
                        Stream stream = new Stream();

                        var streamLink = streamObject.GetValue("raw_url");
                        var streamLanguage = streamObject.GetValue("language");

                        stream.StreamLink = streamLink.ToString() == "" ? "" : streamLink.Value<string>();
                        stream.StreamLanguage = streamLanguage.ToString() == "" ? "" : streamLanguage.Value<string>();

                        matches.StreamList.Add(stream);
                    }

                    matches.LiveSupported = matches.StreamList.Count() > 0 ? true : false;

                    foreach (var opponentObject in opponentArray.Cast<JObject>())
                    {
                        var team = new Team();
                        var opponent = (JObject)opponentObject.GetValue("opponent");

                        var teamId = opponent.GetValue("id");
                        var teamImage = opponent.GetValue("image_url");
                        var teamName = opponent.GetValue("name");

                        team.TeamAPIID = teamId.ToString() == "" ? -1 : teamId.Value<int>();
                        team.Name = teamName.ToString() == "" ? "undefined" : teamName.Value<string>();
                        team.Image = teamImage.ToString() == "" ? "/images/logo1.jpg" : teamImage.Value<string>();
                        team.CouchName = "";
                        team.Losses = 0;
                        team.Winnings = 0;
                        team.WorldRank = 0;
                        if(game == "csgo")
                            team.Game = GameType.CSGO;
                        else
                            team.Game = GameType.Valorant;

                        matches.TeamsList.Add(team);
                    }

                    if (matches.TeamsList.Count() == 2)
                    {
                        foreach (var teamResult in results.Cast<JObject>())
                        {
                            var score = teamResult.GetValue("score");
                            var team_id = teamResult.GetValue("team_id");

                            var teamid = team_id.ToString() == null ? 1 : team_id.Value<int>();
                            var points = score.ToString() == "" ? 0 : score.Value<int>();

                            foreach (var team in matches.TeamsList)
                            {
                                if (team.TeamAPIID == teamid)
                                {
                                    var result = new Score();
                                    result.TeamScore = points;
                                    result.Team = team;
                                    result.TeamName = team.Name;

                                    matches.Scores.Add(result);
                                }
                            }
                        }

                        matchesCS.Add(matches);
                    }
                }
            }

            return matchesCS;
        }

        /*// De CSGO e de Valorant
        // GET: Matches/CSMatcheDetails/5
        public ActionResult CSMatcheDetails(int id)
        {
            return View();
        }

        // GET: Matches/ValMatcheDetails/5
        public ActionResult ValMatcheDetails(Guid id)
        {
            return View();
        }

        //De CSGO e de Valorant
        // GET: Matches/CSMatcheCreate
        public ActionResult CSMatcheCreate()
        {
            return View();
        }

        //De CSGO e de Valorant
        // POST: Matches/CSMatcheCreate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CSMatcheCreate(IFormCollection collection)
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
        public ActionResult CSMatcheEdit(int id)
        {
            return View();
        }

        //De CSGO e de Valorant
        // POST: Matches/CSMatcheEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CSMatcheEdit(int id, IFormCollection collection)
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
        public ActionResult CSMatcheDelete(int id)
        {
            return View();
        }

        //De CSGO e de Valorant
        // POST: Matches/CSMatcheDelete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CSMatcheDelete(int id, IFormCollection collection)
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
