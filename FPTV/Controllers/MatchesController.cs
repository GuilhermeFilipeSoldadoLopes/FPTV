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
using NuGet.Common;
using RestSharp;
using SendGrid.Helpers.Mail;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Numerics;
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
        public ActionResult Index(string sort = "", string filter = "", string page = "&page=1", string game = "csgo")
		{
			ViewData["game"] = game;
			ViewBag.page = "Matches";
            //Request processing with RestSharp
            var jsonFilter = (filter == "" || filter == "livestream") ? "" : "filter[" + filter + "]=true&";
            var jsonSort = (sort == "" || sort == "tournament") ? "" : sort;
            var jsonPage = page;
            var jsonPerPage = "&per_page=10";
            var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
            var requestLink = "https://api.pandascore.co/" + game + "/matches/";

            var fullApiPath = requestLink + "past?" + jsonFilter + jsonSort + jsonPage + jsonPerPage + token;
            var pastMatches = getAPIMatches(fullApiPath, game);
            fullApiPath = requestLink + "running?" + jsonFilter + jsonSort + jsonPage + jsonPerPage + token;
            var runningMatches = getAPIMatches(fullApiPath, game);
            fullApiPath = requestLink + "upcoming?" + jsonFilter + jsonSort + jsonPage + jsonPerPage + token;
            var upcomingMatches = getAPIMatches(fullApiPath, game);

            if (pastMatches == null || runningMatches == null || upcomingMatches == null)
            {
                return View("~/Views/Home/Error404.cshtml");
            }

            List<int> dbMatchesIds = game == "csgo" ? _context.MatchesCS.Select(m => m.MatchesAPIID).ToList() : _context.MatchesVal.Select(m => m.MatchesAPIID).ToList();

            foreach (var match in pastMatches)
            {
                if (game == "csgo")
                {
                    var m = (MatchesCS)match;
                    if (!dbMatchesIds.Contains(m.MatchesAPIID))
                    {
                        _context.MatchesCS.Add(m);
                    }
                }
                else
                {
                    var m = (MatchesVal)match;
                    if (!dbMatchesIds.Contains(m.MatchesAPIID))
                    {
                        _context.MatchesVal.Add(m);
                    }
                }
            }

            _context.SaveChanges();

            pastMatches = game == "csgo" ? _context.MatchesCS.Include(m => m.TeamsList).Include(m => m.Scores).ToList() : _context.MatchesVal.Include(m => m.TeamsList).Include(m => m.Scores).ToList();

            if (sort == "tournament")
            {
                if (game == "csgo")
                {
                    pastMatches = ((List<MatchesCS>)pastMatches).OrderBy(m => m.EventName).ToList();
                    runningMatches = ((List<MatchesCS>)runningMatches).OrderBy(m => m.EventName).ToList();
                    upcomingMatches = ((List<MatchesCS>)upcomingMatches).OrderBy(m => m.EventName).ToList();
                }
                else
                {
                    pastMatches = ((List<MatchesVal>)pastMatches).OrderBy(m => m.EventName).ToList();
                    runningMatches = ((List<MatchesVal>)runningMatches).OrderBy(m => m.EventName).ToList();
                    upcomingMatches = ((List<MatchesVal>)upcomingMatches).OrderBy(m => m.EventName).ToList();
                }
            }

            if (filter == "livestream")
            {
                if (game == "csgo")
                {
                    pastMatches = ((List<MatchesCS>)pastMatches).Where(m => m.LiveSupported == true).ToList();
                    runningMatches = ((List<MatchesCS>)runningMatches).Where(m => m.LiveSupported == true).ToList();
                    upcomingMatches = ((List<MatchesCS>)upcomingMatches).Where(m => m.LiveSupported == true).ToList();
                }
                else
                {
                    pastMatches = ((List<MatchesVal>)pastMatches).Where(m => m.LiveSupported == true).ToList();
                    runningMatches = ((List<MatchesVal>)runningMatches).Where(m => m.LiveSupported == true).ToList();
                    upcomingMatches = ((List<MatchesVal>)upcomingMatches).Where(m => m.LiveSupported == true).ToList();
                }
            }

            ViewBag.pastMatches = pastMatches;

            ViewBag.runningMatches = runningMatches;
            ViewBag.upcomingMatches = upcomingMatches;

            ViewBag.filter = filter;
            ViewBag.sort = sort;

            return View();
        }

        private IList getAPIMatches(string fullApiPath, string game)
        {
            //Request processing with RestSharp
            var client = new RestClient(fullApiPath);
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            var response = client.Execute(request);
            var json = response.Content;

            if(response.StatusCode != System.Net.HttpStatusCode.OK || json == null)
            {
                registerErrorLog(response.StatusCode);
                return null;
            }

            IList matchesList = game == "csgo" ? new List<MatchesCS>() : new List<MatchesVal>();

            var matchesArray = JArray.Parse(json);

            foreach (var item in matchesArray.Cast<JObject>())
            {
                var status = item.GetValue("status");

                if (!status.ToString().Equals("canceled"))
                {
                    dynamic matches = game == "csgo" ? new MatchesCS() : new MatchesVal();
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

                    var matchesAPIId = item.GetValue("id");
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
                    matches.MatchesAPIID = matchesAPIId.ToString() == null ? -1 : matchesAPIId.Value<int>();
                    matches.EventAPIID = eventAPIID.ToString() == null ? -1 : eventAPIID.Value<int>();
                    matches.EventName = eventName.ToString() == null ? "" : matches.LeagueName + " " + eventName.Value<string>();
                    matches.BeginAt = beginAt.ToString() == "" ? new DateTime() : beginAt.Value<DateTime>();
                    matches.EndAt = endAt.ToString() == "" ? new DateTime() : endAt.Value<DateTime>();
                    matches.IsFinished = status.ToString() == "finished" ? true : false;
                    matches.HaveStats = haveStats.ToString() == "true" ? true : false;
                    matches.NumberOfGames = numberOfGames.ToString() == null ? 1 : numberOfGames.Value<int>();
                    matches.WinnerTeamAPIId = winnerTeamAPIId == null ? -1 : winnerTeamAPIId.Value<int>();
                    matches.WinnerTeamName = winnerTeamName == null ? "" : winnerTeamName.Value<string>();
                    matches.Tier = tier.ToString() == "unranked" ? ' ' : tier.Value<char>();
                    matches.LeagueId = LeagueId.ToString() == null ? -1 : LeagueId.Value<int>();
                    matches.LeagueLink = leagueLink.ToString() == null ? "" : leagueLink.Value<string>();

                    /*dynamic matchEvent = game == "csgo" ? new EventCS() : new EventVal();
                    matchEvent.EventAPIID = matches.EventAPIID;
                    matchEvent.BeginAt = new DateTime();
                    matchEvent.EndAt = new DateTime();
                    matchEvent.EventName = matches.EventName;
                    matchEvent.TimeType = TimeType.Running;
                    matchEvent.Finished = false;
                    matchEvent.EventImage = "";
                    matchEvent.EventLink = "";
                    matchEvent.LeagueName = "";
                    matchEvent.PrizePool = "";
                    matchEvent.Tier = ' ';
                    matchEvent.WinnerTeamAPIID = 1;
                    matchEvent.WinnerTeamName = "";
                    matches.Event = matchEvent;*/

                    if ((string)status == "finished")
                    {
                        matches.IsFinished = true;
                        matches.TimeType = TimeType.Past;
                    }
                    if ((string)status == "not_started")
                        matches.TimeType = TimeType.Upcoming;
                    if ((string)status == "running")
                        matches.TimeType = TimeType.Running;

                    foreach (var streamObject in streamArray.Cast<JObject>())
                    {
                        Stream stream = new Stream();

                        var streamLink = streamObject.GetValue("raw_url");
                        var streamLanguage = streamObject.GetValue("language");

                        stream.StreamLink = streamLink.ToString() == "" ? "" : streamLink.Value<string>();
                        stream.StreamLanguage = streamLanguage.ToString() == "" ? "" : streamLanguage.Value<string>();

                        matches.StreamList.Add(stream);
                    }

                    matches.LiveSupported = streamArray.Count() > 0 ? true : false;

                    foreach (var opponentObject in opponentArray.Cast<JObject>())
                    {
                        var team = new Team();
                        var opponent = (JObject)opponentObject.GetValue("opponent");

                        var teamId = opponent.GetValue("id");
                        var teamImage = opponent.GetValue("image_url");
                        var teamName = opponent.GetValue("name");

                        team.TeamAPIID = teamId.ToString() == "" ? -1 : teamId.Value<int>();
                        team.Name = teamName.ToString() == "" ? "undefined" : teamName.Value<string>();
                        team.Image = teamImage.ToString() == "" ? "/images/missing.png" : teamImage.Value<string>();
                        team.CoachName = "";
                        team.Losses = 0;
                        team.Winnings = 0;
                        team.WorldRank = 0;

                        if (game == "csgo")
                            team.Game = GameType.CSGO;
                        else
                            team.Game = GameType.Valorant;

                        matches.TeamsList.Add(team);
                    }

                    if (opponentArray.Count() == 2)
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

                        matchesList.Add(matches);
                    }
                }
            }

            return matchesList;
        }

        public ActionResult Results(int days = 0, string game = "csgo")
		{
			ViewData["game"] = game;
			ViewBag.page = "Results";
            ViewBag.days = days;

			var day = "";
            DateTime date;

            if (days < 0)
            {
                ViewBag.Message = "It is only possible to consult results of matches that have already taken place.";
				day = DateTime.Now.AddDays(0).ToString("yyyy-MM-dd");
                date = DateTime.Now.AddDays(0);
			} else if (days >= 365)
            {
                ViewBag.Message = "You can only consult the results up to 365 days ago.";
				day = DateTime.Now.AddDays(-365).ToString("yyyy-MM-dd");
				date = DateTime.Now.AddDays(-365);
			} else
            {
				day = DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd");
				date = DateTime.Now.AddDays(-days);
			}

            ViewBag.Date = date.ToString("dd-MM-yyyy");

            var jsonFilter = "filter[begin_at]=" + day;
            var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
            var requestLink = "https://api.pandascore.co/" + game + "/matches/";

            var fullApiPath = requestLink + "past?" + jsonFilter + "&sort=&page=1&per_page=10" + token;

            var client = new RestClient(fullApiPath);
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            var response = client.Execute(request);
            var json = response.Content;

            if (response.StatusCode != System.Net.HttpStatusCode.OK || json == null)
			{
				registerErrorLog(response.StatusCode);
                return View("~/Views/Home/Error404.cshtml");
            }

            IList matchesList = game == "csgo" ? new List<MatchesCS>() : new List<MatchesVal>();

            var matchesArray = JArray.Parse(json);

            foreach (var item in matchesArray.Cast<JObject>())
            {
                var status = item.GetValue("status");

                if (!status.ToString().Equals("canceled"))
                {
                    dynamic matches = game == "csgo" ? new MatchesCS() : new MatchesVal();
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

                    var matchesAPIId = item.GetValue("id");
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
                    var leagueImage = league.GetValue("image_url");

                    //Handling for null values
                    matches.LeagueName = leagueName.ToString() == null ? "" : leagueName.Value<string>();
                    matches.MatchesAPIID = matchesAPIId.ToString() == null ? -1 : matchesAPIId.Value<int>();
                    matches.EventAPIID = eventAPIID.ToString() == null ? -1 : eventAPIID.Value<int>();
                    matches.EventName = eventName.ToString() == null ? "" : matches.LeagueName + " " + eventName.Value<string>();
                    matches.BeginAt = beginAt.ToString() == "" ? new DateTime() : beginAt.Value<DateTime>();
                    matches.EndAt = endAt.ToString() == "" ? new DateTime() : endAt.Value<DateTime>();
                    matches.IsFinished = status.ToString() == "finished" ? true : false;
                    matches.HaveStats = haveStats.ToString() == "true" ? true : false;
                    matches.NumberOfGames = numberOfGames.ToString() == null ? 1 : numberOfGames.Value<int>();
                    matches.WinnerTeamAPIId = winnerTeamAPIId == null ? -1 : winnerTeamAPIId.Value<int>();
                    matches.WinnerTeamName = winnerTeamName == null ? "" : winnerTeamName.Value<string>();
                    matches.Tier = tier.ToString() == "unranked" ? ' ' : tier.Value<char>();
                    matches.LeagueId = LeagueId.ToString() == null ? -1 : LeagueId.Value<int>();
                    matches.LeagueLink = leagueLink.ToString() == null ? "" : leagueLink.Value<string>();

                    dynamic matchEvent = game == "csgo" ? new EventCS() : new EventVal();
                    matchEvent.EventAPIID = matches.EventAPIID;
                    matchEvent.BeginAt = new DateTime();
                    matchEvent.EndAt = new DateTime();
                    matchEvent.EventName = matches.EventName;
                    matchEvent.TimeType = TimeType.Running;
                    matchEvent.Finished = false;
                    matchEvent.EventImage = leagueImage.ToString() == "" ? "/images/missing.png" : leagueImage.Value<string>();
                    matchEvent.EventLink = "";
                    matchEvent.LeagueName = "";
                    matchEvent.PrizePool = "";
                    matchEvent.Tier = ' ';
                    matchEvent.WinnerTeamAPIID = 1;
                    matchEvent.WinnerTeamName = "";
                    matches.Event = matchEvent;

                    if ((string)status == "finished")
                    {
                        matches.IsFinished = true;
                        matches.TimeType = TimeType.Past;
                    }
                    if ((string)status == "not_started")
                        matches.TimeType = TimeType.Upcoming;
                    if ((string)status == "running")
                        matches.TimeType = TimeType.Running;

                    foreach (var streamObject in streamArray.Cast<JObject>())
                    {
                        Stream stream = new Stream();

                        var streamLink = streamObject.GetValue("raw_url");
                        var streamLanguage = streamObject.GetValue("language");

                        stream.StreamLink = streamLink.ToString() == "" ? "" : streamLink.Value<string>();
                        stream.StreamLanguage = streamLanguage.ToString() == "" ? "" : streamLanguage.Value<string>();

                        matches.StreamList.Add(stream);
                    }

                    matches.LiveSupported = streamArray.Count() > 0 ? true : false;

                    foreach (var opponentObject in opponentArray.Cast<JObject>())
                    {
                        var team = new Team();
                        var opponent = (JObject)opponentObject.GetValue("opponent");

                        var teamId = opponent.GetValue("id");
                        var teamImage = opponent.GetValue("image_url");
                        var teamName = opponent.GetValue("name");

                        team.TeamAPIID = teamId.ToString() == "" ? -1 : teamId.Value<int>();
                        team.Name = teamName.ToString() == "" ? "undefined" : teamName.Value<string>();
                        team.Image = teamImage.ToString() == "" ? "/images/missing.png" : teamImage.Value<string>();
                        team.CoachName = "";
                        team.Losses = 0;
                        team.Winnings = 0;
                        team.WorldRank = 0;

                        if (game == "csgo")
                            team.Game = GameType.CSGO;
                        else
                            team.Game = GameType.Valorant;

                        matches.TeamsList.Add(team);
                    }

                    if (opponentArray.Count() == 2)
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

                        matchesList.Add(matches);
                    }

                }
            }

            ViewBag.matches = matchesList;

            return View();
        }

        public ActionResult MatchDetails(int id = 0, string type = "past", string game = "csgo")
		{
			ViewData["game"] = game;
			ViewBag.page = "Matches";
            Random rnd = new Random();

            var jsonFilter = "filter[id]=" + id;
            var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
            var requestLink = "https://api.pandascore.co/" + game + "/matches/";

            var fullApiPath = requestLink + type + "?" + jsonFilter + "&sort=" + token;

            var client = new RestClient(fullApiPath);
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            var response = client.Execute(request);
            var json = response.Content;

            dynamic matches = game == "csgo" ? new MatchesCS() : new MatchesVal();
            dynamic matchesPlayer = game == "csgo" ? new List<MatchPlayerStatsCS>() : new List<MatchPlayerStatsVal>();

            var matchesArray = JArray.Parse(json);

            if (response.StatusCode != System.Net.HttpStatusCode.OK || json == null || matchesArray.Count() == 0)
            {
                ViewBag.dropDownGame = game;
                registerErrorLog(response.StatusCode);
                return View("~/Views/Home/Error404.cshtml");
            }

            var matchesObject = (JObject)matchesArray[0];

            var status = matchesObject.GetValue("status");

            matches.TeamsAPIIDList = new List<int>();

            matches.Scores = new List<Score>();
            matches.TeamsList = new List<Team>();
            matches.StreamList = new List<Stream>();

            //Set up values from api
            var league = (JObject)matchesObject.GetValue("league");
            var live = (JObject)matchesObject.GetValue("live");
            var tournament = (JObject)matchesObject.GetValue("tournament");
            var winner = matchesObject.GetValue("winner");
            var results = (JArray)matchesObject.GetValue("results");
            var opponentArray = (JArray)matchesObject.GetValue("opponents");
            var streamArray = (JArray)matchesObject.GetValue("streams_list");

            var matchesAPIId = matchesObject.GetValue("id");
            var eventAPIID = tournament.GetValue("id");
            var eventName = tournament.GetValue("name");
            var beginAt = matchesObject.GetValue("begin_at");
            var endAt = matchesObject.GetValue("end_at");
            var haveStats = matchesObject.GetValue("detailed_stats");
            var numberOfGames = matchesObject.GetValue("number_of_games");
            var winnerTeamAPIId = winner.ToString() == "" ? null : winner.ToObject<JObject>().GetValue("id");
            var winnerTeamName = winner.ToString() == "" ? null : winner.ToObject<JObject>().GetValue("name");
            var tier = tournament.GetValue("tier");
            var leagueName = league.GetValue("name");
            var LeagueId = league.GetValue("id");
            var leagueLink = league.GetValue("url");

            //Handling for null values
            matches.LeagueName = leagueName.ToString() == null ? "" : leagueName.Value<string>();
            matches.MatchesAPIID = matchesAPIId.ToString() == null ? -1 : matchesAPIId.Value<int>();
            matches.EventAPIID = eventAPIID.ToString() == null ? -1 : eventAPIID.Value<int>();
            matches.EventName = eventName.ToString() == null ? "" : matches.LeagueName + " " + eventName.Value<string>();
            matches.BeginAt = beginAt.ToString() == "" ? new DateTime() : beginAt.Value<DateTime>();
            matches.EndAt = endAt.ToString() == "" ? new DateTime() : endAt.Value<DateTime>();
            matches.IsFinished = status.ToString() == "finished" ? true : false;
            matches.HaveStats = haveStats.ToString() == "true" ? true : false;
            matches.NumberOfGames = numberOfGames.ToString() == null ? 1 : numberOfGames.Value<int>();
            matches.WinnerTeamAPIId = winnerTeamAPIId == null ? -1 : winnerTeamAPIId.Value<int>();
            matches.WinnerTeamName = winnerTeamName == null ? "" : winnerTeamName.Value<string>();
            matches.Tier = tier.ToString() == "unranked" ? ' ' : tier.Value<char>();
            matches.LeagueId = LeagueId.ToString() == null ? -1 : LeagueId.Value<int>();
            matches.LeagueLink = leagueLink.ToString() == null ? "" : leagueLink.Value<string>();

            /*dynamic matchEvent = game == "csgo" ? new EventCS() : new EventVal();
            matchEvent.EventAPIID = matches.EventAPIID;
            matchEvent.BeginAt = new DateTime();
            matchEvent.EndAt = new DateTime();
            matchEvent.EventName = matches.EventName;
            matchEvent.TimeType = TimeType.Running;
            matchEvent.Finished = false;
            matchEvent.EventImage = "";
            matchEvent.EventLink = "";
            matchEvent.LeagueName = "";
            matchEvent.PrizePool = "";
            matchEvent.Tier = ' ';
            matchEvent.WinnerTeamAPIID = 1;
            matchEvent.WinnerTeamName = "";
            matches.Event = matchEvent;*/

            if ((string)status == "finished")
            {
                matches.IsFinished = true;
                matches.TimeType = TimeType.Past;
            }
            if ((string)status == "not_started")
                matches.TimeType = TimeType.Upcoming;
            if ((string)status == "running")
                matches.TimeType = TimeType.Running;

            foreach (var streamObject in streamArray.Cast<JObject>())
            {
                Stream stream = new Stream();

                var streamLink = streamObject.GetValue("raw_url");
                var streamLanguage = streamObject.GetValue("language");

                stream.StreamLink = streamLink.ToString() == "" ? "" : streamLink.Value<string>();
                stream.StreamLanguage = streamLanguage.ToString() == "" ? "/images/Flags/4x3/pt.svg" : "/images/Flags/4x3/" + streamLanguage.Value<string>() + ".svg";

                if (streamLanguage.ToString() == "en" || streamLanguage.ToString() == "uk")
                    stream.StreamLanguage = "/images/Flags/4x3/gb.svg";

                matches.StreamList.Add(stream);
            }

            matches.LiveSupported = streamArray.Count() > 0 ? true : false;

            foreach (var opponentObject in opponentArray.Cast<JObject>())
            {
                var team = new Team();
                var opponent = (JObject)opponentObject.GetValue("opponent");

                var teamId = opponent.GetValue("id");
                var teamImage = opponent.GetValue("image_url");
                var teamName = opponent.GetValue("name");

                team.TeamAPIID = teamId.ToString() == "" ? -1 : teamId.Value<int>();
                team.Name = teamName.ToString() == "" ? "undefined" : teamName.Value<string>();
                team.Image = teamImage.ToString() == "" ? "/images/missing.png" : teamImage.Value<string>();
                team.CoachName = "";
                team.Losses = 0;
                team.Winnings = 0;
                team.WorldRank = 0;
                team.Players = new List<Player>();

                if (game == "csgo")
                    team.Game = GameType.CSGO;
                else
                    team.Game = GameType.Valorant;

                //Playersvar
                jsonFilter = "filter[id]=" + teamId;
                token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
                requestLink = "https://api.pandascore.co/" + game + "/teams";

                fullApiPath = requestLink + "?" + jsonFilter + "&sort=" + token;

                client = new RestClient(fullApiPath);
                request = new RestRequest("", Method.Get);
                request.AddHeader("accept", "application/json");
                response = client.Execute(request);
                var teamsJson = response.Content;

                if (response.StatusCode != System.Net.HttpStatusCode.OK || teamsJson == null)
				{
					registerErrorLog(response.StatusCode);
                    return View("~/Views/Home/Error404.cshtml");
                }

                var teams = JArray.Parse(teamsJson);
                var teamObject = (JObject)teams[0];
                var players = (JArray)teamObject.GetValue("players");


                foreach (var playerObject in players.Cast<JObject>())
                {
                    var matchPlayer = new Player();
                    dynamic matchPlayerStats = game == "csgo" ? new MatchPlayerStatsCS() : new MatchPlayerStatsVal();
                    var player = new Player();
                    var playerId = playerObject.GetValue("id");
                    var playerName = playerObject.GetValue("name");
                    var playerImage = playerObject.GetValue("image_url");

                    player.PlayerAPIId = playerId.ToString() == "" ? 1 : playerId.Value<int>();
                    player.Name = playerName.ToString() == "" ? "undefined" : playerName.Value<string>();
                    player.Image = playerImage.ToString() == "" ? "/images/default-profile-icon-24.jpg" : playerImage.Value<string>();
                    matchPlayer.PlayerAPIId = playerId.ToString() == "" ? 1 : playerId.Value<int>();
                    matchPlayer.Name = playerName.ToString() == "" ? "undefined" : playerName.Value<string>();
                    matchPlayer.Image = playerImage.ToString() == "" ? "/images/default-profile-icon-24.jpg" : playerImage.Value<string>();

                    matchPlayerStats.Player = matchPlayer;
                    matchPlayerStats.MatchAPIID = 0;
                    matchPlayerStats.PlayerAPIId = playerId.ToString() == "" ? 1 : playerId.Value<int>();
                    matchPlayerStats.PlayerName = playerName.ToString() == "" ? "undefined" : playerName.Value<string>();;
                    matchPlayerStats.Kills = rnd.Next(1, 31);
                    matchPlayerStats.Deaths = rnd.Next(1, 21);
                    matchPlayerStats.Assists = rnd.Next(1, 11);
                    if(game == "csgo")matchPlayerStats.FlashAssist = rnd.Next(1, 6);
                    matchPlayerStats.ADR = rnd.Next(30, 155);
                    matchPlayerStats.HeadShots = Math.Round((rnd.NextDouble() * 100), 1);
                    double kd_diff = (double)matchPlayerStats.Kills / (double)matchPlayerStats.Deaths;
                    matchPlayerStats.KD_Diff = Math.Round(kd_diff, 2);

                    if (team.Players.Count() < 5)
                    {
                        team.Players.Add(player);

                        matchesPlayer.Add(matchPlayerStats);
                    }
                }

                matches.TeamsList.Add(team);
            }

            if (opponentArray.Count() == 2)
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
            }

            List<string> mapsNames = new List<string>();
            List<string> mapsImages = new List<string>();

            token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
            requestLink = "https://api.pandascore.co/" + game + "/maps";

            fullApiPath = requestLink + "?" + token;

            client = new RestClient(fullApiPath);
            request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            response = client.Execute(request);
            var mapsJson = response.Content;

            if (response.StatusCode != System.Net.HttpStatusCode.OK || mapsJson == null)
			{
				registerErrorLog(response.StatusCode);
                return View("~/Views/Home/Error404.cshtml");
            }

            var maps = JArray.Parse(mapsJson);

            foreach (var mapObject in maps.Cast<JObject>())
            {
                var name = mapObject.GetValue("name");
                var image = mapObject.GetValue("image_url");

                mapsNames.Add(name.ToString() == "" ? "undefined" : name.Value<string>());
                mapsImages.Add(image.ToString() == "" ? "undefined" : image.Value<string>());
            }

            List<string> removedMaps = new List<string>();
            List<string> pickedMaps = new List<string>();

            if (matches.NumberOfGames != 1)
            {
                for (int i = 0; i < 7; i++)
                {
                    var n = rnd.Next(mapsNames.Count());

                    if (!pickedMaps.Contains(mapsNames.GetItemByIndex(n)) && !removedMaps.Contains(mapsNames.GetItemByIndex(n)))
                    {
                        if (pickedMaps.Count() < matches.NumberOfGames)
                            pickedMaps.Add(mapsNames.GetItemByIndex(n));
                        else
                            removedMaps.Add(mapsNames.GetItemByIndex(n));
                    }
                    else
                        i--;
                }
            }
            else
                pickedMaps.Add(mapsNames.GetItemByIndex(rnd.Next(mapsNames.Count())));

            string MVP_PlayerName = "";
            double bestADR = 0.0;
            foreach (var item in matchesPlayer) {
                if (bestADR < ((double)item.Kills / 16) * 100) { 
                    MVP_PlayerName = item.PlayerName;
                    bestADR = ((double)item.Kills / 16) * 100;
                }
            }
            ViewBag.MVP_Player = MVP_PlayerName;

            ViewBag.matches = matches;
            ViewBag.matchesPlayer = matchesPlayer;
            ViewBag.removedMaps = removedMaps;
            ViewBag.pickedMaps = pickedMaps;
            ViewBag.mapsImages = mapsImages;
            ViewBag.mapsNames = mapsNames;

            return View();
        }

        public ActionResult PlayerAndStats()
        {
            return View();
        }

        public ActionResult TeamStats()
        {
            return View();
        }

        private void registerErrorLog(HttpStatusCode statusCode)
        {
            /*
            public string? Error { get; set; }
            public DateTime Date { get; set; }
            public Guid UserId { get; set; }
            public virtual Profile? Profile { get; set; }*/

            ErrorLog error = new ErrorLog();

            error.Error = "MatchesController.cs -> " + statusCode.ToString();
            error.Date = DateTime.Now;

            _context.ErrorLog.Add(error);
            _context.SaveChanges();
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
