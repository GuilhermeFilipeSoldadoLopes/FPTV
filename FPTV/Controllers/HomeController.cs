using System.Collections;
using System.Diagnostics;
using System.Net;
using FPTV.Data;
using FPTV.Models;
using FPTV.Models.EventsModels;
using FPTV.Models.MatchesModels;
using FPTV.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using PusherServer;
using RestSharp;

namespace FPTV.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FPTVContext _context;
        private string dropDownGame;
		private string page;

		public HomeController(ILogger<HomeController> logger, FPTVContext context)
        {
            _logger = logger;
            _context = context;
        }

		public IActionResult VALORANT(string page = "Index")
		{
			return RedirectToAction(page, "Home", new { game = "valorant" });
		}

		public IActionResult CSGO(string page = "Index")
		{
			return RedirectToAction(page, "Home", new { game = "csgo" });
		}

		public IActionResult Index(string game)
		{
            if (game == "")
            {
                game = null;
            }

            ViewData["game"] = game;
			page = "Index";
			var account = _context.Users.Where(u => u.EmailConfirmed == true).ToList().Count();

			var accountTxt = (account == 1) ? " user" : " users";
			
			ViewBag.page = page;
			ViewData["accounts"] = account;
	        ViewData["account_txt"] = accountTxt;

			var visitors = 0;
			var visitText = "views";


            if (System.IO.File.Exists("visitors.txt"))
			{
				DateTime lastModificationFileDateTime = System.IO.File.GetLastWriteTime("visitors.txt");
				DateTime lasModificationDate =
					new DateTime(lastModificationFileDateTime.Year, lastModificationFileDateTime.Month, lastModificationFileDateTime.Day);

                Console.WriteLine(lasModificationDate);
                Console.WriteLine(DateTime.Compare(lasModificationDate, DateTime.Now.Date));

                if (DateTime.Compare(lasModificationDate, DateTime.Now.Date) < 0)
				{
					System.IO.File.WriteAllText("visitors.txt", 0.ToString());
				}

				string noOfVisitors = System.IO.File.ReadAllText("visitors.txt");
				visitors = Int32.Parse(noOfVisitors);

                ++visitors;
                visitText = (visitors == 1) ? " view" : " views";

                System.IO.File.WriteAllText("visitors.txt", visitors.ToString());
            } else {
                System.IO.File.WriteAllText("visitors.txt", 1.ToString());
            }
            
            var options = new PusherOptions();
			options.Cluster = "PUSHER_APP_CLUSTER";

			var pusher = new Pusher(
				"PUSHER_APP_ID",
				"PUSHER_APP_KEY",
				"PUSHER_APP_SECRET", options);

			pusher.TriggerAsync(
				"general",
				"newVisit",
				new { visits = visitors.ToString(), message = visitText });

			ViewData["visitors"] = visitors;
			ViewData["visitors_txt"] = visitText;

            //csgo/matches/past?
            var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
            var requestLink = "https://api.pandascore.co/";
            var jsonPage = "&page=1";
			var jsonPerPage = "&per_page=5";

			var fullApiPath = requestLink + "valorant/matches/running?" + jsonPage + jsonPerPage + token;
            var liveValorantMatches = getAPIMatches(fullApiPath, "valorant");
            fullApiPath = requestLink + "csgo/matches/running?" + jsonPage + jsonPerPage + token;
            var liveCSMatches = getAPIMatches(fullApiPath, "csgo");
            fullApiPath = requestLink + "valorant/matches/past?" + jsonPage + jsonPerPage + token;
            var ValorantResults = getAPIMatches(fullApiPath, "valorant");
            fullApiPath = requestLink + "csgo/matches/past?" + jsonPage + jsonPerPage + token;
            var CSResults = getAPIMatches(fullApiPath, "csgo");

			if (liveValorantMatches == null || liveCSMatches == null || ValorantResults == null || CSResults == null)
			{
				return View("~/Views/Home/Error404.cshtml");
			}

			ViewBag.liveValorantMatches = liveValorantMatches;
            ViewBag.liveCSMatches = liveCSMatches;
            ViewBag.ValorantResults = ValorantResults;
            ViewBag.CSResults = CSResults;

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

            if (response.StatusCode != System.Net.HttpStatusCode.OK || json == null)
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
                    matches.StreamList = new List<Models.MatchesModels.Stream>();

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
						Models.MatchesModels.Stream stream = new Models.MatchesModels.Stream();

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

        private void registerErrorLog(HttpStatusCode statusCode)
        {
            ErrorLog error = new ErrorLog();

            error.Error = "MatchesController.cs -> " + statusCode.ToString();
            error.Date = DateTime.Now;

            _context.ErrorLog.Add(error);
            _context.SaveChanges();
        }

        [Authorize]
        public IActionResult Events(string game = "csgo")
		{
			page = "Events";
			ViewBag.page = page;
			return RedirectToAction("Index", page, new { sort = "&sort=-begin_at", filter = "running", game = game });
        }

        [Authorize]
		public IActionResult Forum(string game)
		{
			page = "Forum";
            ViewBag.page = page;
            return RedirectToAction("Forum", "Index", new { game = game});
		}

        [Authorize]
        public IActionResult ForumRules()
		{
			page = "Forum";
			//return RedirectToAction("Forum", "ForumRules");
			return View();
		}

        [Authorize]
        public IActionResult NewTopic()
		{
			return View();
		}

        [Authorize]
        public IActionResult Topic()
		{
			return View();
		}

		public IActionResult Matches(string game = "csgo")
		{
			page = "Matches";
			ViewBag.page = page;
			return RedirectToAction("Index", page, new { sort = "", filter = "", game = game });
		}

        public IActionResult Results(string game = "csgo")
		{
			page = "Matches";
			ViewBag.page = page;
			return RedirectToAction("Results", page, new {game = game });
		}

        [Authorize]
        public IActionResult CSGOStats()
		{
			page = "Index";
			return RedirectToAction("CSGOStats", "Stats");
		}

        [Authorize]
        public IActionResult PlayerAndStats()
		{
			page = "Index";
			return RedirectToAction("PlayerAndStats", "Matches");
		}

        [Authorize]
        public IActionResult TeamStats()
		{
			page = "Index";
			return RedirectToAction("TeamStats", "Matches");
		}

		public IActionResult LoginRegister()
		{
			page = "Index";
			return View();
        }
        
        public IActionResult Privacy()
		{
			page = "Index";
			return View();
        }
        public IActionResult ConfirmDelete()
		{
			page = "Index";
			return View();
        }

		public IActionResult MatchDetails()
		{
			page = "Index";
			return RedirectToAction("Matches", "MatchDetails");
		}

        public IActionResult Test()
		{
			page = "Index";
            return View();
        }
        
        public IActionResult About()
		{
			page = "About";
            ViewBag.page = page;
			return View(); //return View(); //apagar index - quando a pagina tiver feita
		}
        
        public IActionResult Register()
		{
			page = "Index";
			return View();
        }

        public IActionResult SendEmail()
		{
			page = "Index";
			return View();
        }
        
        public IActionResult StatisticsOfSite()
		{
			page = "Index";
			return View();
        }
        public IActionResult Error404()
        {
            page = "Index";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            page = "Index";
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}