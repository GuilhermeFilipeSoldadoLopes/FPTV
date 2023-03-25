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
using System.Collections;
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
        public async Task<ActionResult> Index(string sort = "", string filter = "", string page = "&page=1", string game = "valorant")
		{
			ViewBag.dropDownGame = game;
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

            await _context.SaveChangesAsync();
			
			/*pastMatches = game == "csgo" ? _context.MatchesCS.Include(m => m.TeamsList).ToList() : _context.MatchesVal.Include(m => m.TeamsList).ToList();

			foreach (var item in pastMatches)
            {
				dynamic match = game == "csgo" ? (List<MatchesCS>)item : (List<MatchesVal>)item;
                dynamic teams = game == "csgo" ? (List<MatchesCS>)item : (List<MatchesVal>)item;

                //Console.WriteLine(match.MatchesAPIID);
                var b = match.TeamsList;

				foreach (var item2 in b)
				{
					//Console.WriteLine(item2.Name);
					var c = item2.Name;
				}
				Console.WriteLine("");
			}*/

			if (sort == "tournament")
            {
                if (game == "csgo")
                {
                    pastMatches = ((List <MatchesCS>)pastMatches).OrderBy(m => m.EventName).ToList();
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
            var json = client.Execute(request).Content;

            if (json == null)
            {
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

                    dynamic matchEvent = game == "csgo" ? new EventCS() : new EventVal();
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
                    matches.Event = matchEvent;

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
                        team.Image = teamImage.ToString() == "" ? "https://mizuwu.s-ul.eu/9UCb9vsT" : teamImage.Value<string>();
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
                                if(team.TeamAPIID == teamid)
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

		public ActionResult Results(int days = 0, string game = "valorant")
        {
            ViewBag.dropDownGame = game;
            ViewBag.page = "Results";
			ViewBag.days = days;

			var day = DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd");

            var jsonFilter = "filter[begin_at]="+ day;
            var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
            var requestLink = "https://api.pandascore.co/" + game + "/matches/";

            var fullApiPath = requestLink + "past?" + jsonFilter + "&sort=&page=1&per_page=10" + token;

            var client = new RestClient(fullApiPath);
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            var json = client.Execute(request).Content;

            if (json == null)
            {
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
                    matchEvent.EventImage = leagueImage.ToString() == "" ? "https://mizuwu.s-ul.eu/9UCb9vsT" : leagueImage.Value<string>();
					matchEvent.EventLink = "";
                    matchEvent.LeagueName = "";
                    matchEvent.PrizePool = "";
                    matchEvent.Tier = ' ';
                    matchEvent.WinnerTeamAPIID = 1;
                    matchEvent.WinnerTeamName = "";
                    matches.Event = matchEvent;

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
                        team.Image = teamImage.ToString() == "" ? "https://mizuwu.s-ul.eu/9UCb9vsT" : teamImage.Value<string>();
                        team.CouchName = "";
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

		public ActionResult MatchDetails(int id = 0, string type = "past", string game = "valorant")
        {
            ViewBag.dropDownGame = game;
            ViewBag.page = "Matches";

            /*var jsonFilter = "filter[id]=" + id;
            var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
            var requestLink = "https://api.pandascore.co/" + game + "/matches/";

            var fullApiPath = requestLink + type+ "?" + jsonFilter + "&sort=&page=1&per_page=10" + token;

            var client = new RestClient(fullApiPath);
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            var json = client.Execute(request).Content;

            if (json == null)
            {
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

                    dynamic matchEvent = game == "csgo" ? new EventCS() : new EventVal();
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
                    matches.Event = matchEvent;

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
                        team.Image = teamImage.ToString() == "" ? "https://mizuwu.s-ul.eu/9UCb9vsT" : teamImage.Value<string>();
                        team.CouchName = "";
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

            ViewBag.matches = matchesList;*/

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
