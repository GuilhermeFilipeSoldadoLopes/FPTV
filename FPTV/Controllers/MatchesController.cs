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
        public ActionResult CSGOMatches(string sort = "sort=begin_at", string filter = "detailed_stats", string page = "&page=1", string game = "csgo")
        {
            //Request processing with RestSharp
            var jsonFilter = "filter[" + filter + "]=true&";
            var jsonSort = sort;
            var jsonPage = page;
            var jsonPerPage = "&per_page=10";
            var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
            var requestLink = "https://api.pandascore.co/" + game + "/matches/";

            var fullApiPath = requestLink + "past?" + jsonFilter + jsonSort + jsonPage + jsonPerPage + token;
            List<MatchesCS> pastMatches = getAPICSGOMatches(fullApiPath);
            fullApiPath = requestLink + "running?" + /*jsonFilter +*/ jsonSort + jsonPage + jsonPerPage + token;
            List<MatchesCS> runningMatches = getAPICSGOMatches(fullApiPath);
            fullApiPath = requestLink + "upcoming?" + jsonFilter + jsonSort + jsonPage + jsonPerPage + token;
            List<MatchesCS> upcomingMatches = getAPICSGOMatches(fullApiPath);

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

            /*List<MatchTeamsCS> teams = new List<MatchTeamsCS>();
            List<int> dbTeamsIds = new List<int>();

            foreach (var matches in pastMatches)
            {
                foreach (var item in matches.TeamsAPIIDList)
                {
                    if (!dbTeamsIds.Contains(item))
                    {

                        teams.Add();
                        dbTeamsIds.Add();
                    }
                }
                
            }

            foreach (var matches in runningMatches)
            {
                foreach (var item in matches.TeamsAPIIDList)
                {
                    if (!dbTeamsIds.Contains(item))
                    {

                        teams.Add();
                        dbTeamsIds.Add();
                    }
                }

            }

            foreach (var matches in upcomingMatches)
            {
                foreach (var item in matches.TeamsAPIIDList)
                {
                    if (!dbTeamsIds.Contains(item))
                    {

                        teams.Add();
                        dbTeamsIds.Add();
                    }
                }

            }

            foreach (var item in pastMatches)
            {
                var a = item.TeamsAPIIDList.ToList().ElementAt(0);
                var b = item.TeamsAPIIDList.ToList().ElementAt(1);
                var c = "";

                foreach (var team in teams)
                {
                    if (team.TeamCSAPIId == a)
                        c = team.Name;
                }

                foreach (var team in teams)
                {
                    if (team.TeamCSAPIId == b)
                        c = team.Name;
                }
            }*/

            _context.SaveChanges();

            ViewBag.pastMatches = pastMatches;//Tem de ir buscar os da base de dados

            ViewBag.runningMatches = runningMatches;
			ViewBag.upcommingMatches = upcomingMatches;

            //ViewBag.teams = teams;

            ViewBag.filter = filter;
            ViewBag.sort = sort;

            return View();
		}

		private List<MatchesCS> getAPICSGOMatches(string fullApiPath)
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

                    matches.StreamList = new List<Stream>();
                    matches.Score = new Dictionary<int, int>();

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
                    var liveSupported = live.GetValue("supported");
                    var leagueName = league.GetValue("name");
                    var LeagueId = league.GetValue("id");
                    var leagueLink = league.GetValue("url");

                    /*
                    *Guid EventId
                    *Guid? WinnerTeamId
                    *List<Guid>? TeamsIDList
                    *ICollection<MatchCS>? MatchesList
                    */

                    //Handling for null values
                    matches.MatchesCSAPIID = matchesCSId.ToString() == null ? -1 : matchesCSId.Value<int>();
                    matches.EventAPIID = eventAPIID.ToString() == null ? -1 : eventAPIID.Value<int>();
                    matches.EventName = eventName.ToString() == null ? "" : eventName.Value<string>();
                    matches.BeginAt = beginAt.ToString() == "" ? null : beginAt.Value<DateTime>();
                    matches.EndAt = endAt.ToString() == "" ? null : endAt.Value<DateTime>();
                    matches.IsFinished = status.ToString() == "finished" ? true : false;
                    matches.HaveStats = haveStats.ToString() == "true" ? true : false;
                    matches.NumberOfGames = numberOfGames.ToString() == null ? 1 : numberOfGames.Value<int>();
                    var aa = winnerTeamAPIId;
                    matches.WinnerTeamAPIId = winnerTeamAPIId == null ? -1 : winnerTeamAPIId.Value<int>();
                    matches.WinnerTeamName = winnerTeamName == null ? "" : winnerTeamName.Value<string>();
                    matches.Tier = tier.ToString() == null ? ' ' : tier.Value<char>();
                    matches.LiveSupported = liveSupported.ToString() == "true" ? true : false;
                    matches.LeagueName = leagueName.ToString() == null ? "" : leagueName.Value<string>();
                    matches.LeagueId = LeagueId.ToString() == null ? -1 : LeagueId.Value<int>();
                    matches.LeagueLink = leagueLink.ToString() == null ? "" : leagueLink.Value<string>();

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

                        matches.StreamList.Add(stream);
                    }

                    foreach (var team in results.Cast<JObject>())
                    {
                        var score = team.GetValue("score");
                        var team_id = team.GetValue("team_id");

                        matches.Score.Add((int)team_id, (int)score);
                    }

                    foreach (var opponentObject in opponentArray.Cast<JObject>())
                    {
                        var opponent = (JObject)opponentObject.GetValue("opponent");

                        var teamIdValue = opponent.GetValue("id");
                        var teamImage = opponent.GetValue("image_url");
                        var teamName = opponent.GetValue("name");
                        var teamId = teamIdValue.ToString() == "" ? -1 : teamIdValue.Value<int>();

                        matches.TeamsAPIIDList.Add(teamId);
                    }

                    Console.WriteLine(matchesCSId);
                    foreach (var a in matches.TeamsAPIIDList)
                    {
                        Console.WriteLine(matchesCSId + " -> " + a);
                    }
                    Console.WriteLine("");

                    if(matches.TeamsAPIIDList.Count() == 2)
                        matchesCS.Add(matches);
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
