using AngleSharp.Io;
using FPTV.Data;
using FPTV.Models.EventsModels;
using FPTV.Models.MatchesModels;
using FPTV.Models.StatisticsModels;
using FPTV.Models.UserModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;

namespace FPTV.Controllers
{
    public class StatsController : Controller
    {
        private readonly FPTVContext _context;
        Random _random = new Random();
        MatchesController _matchesController;

        public StatsController(FPTVContext context)
        {
            _context = context;
            _matchesController = new MatchesController(_context);
        }

        //De CSGO e de Valorant
        // GET: CSMatches
        /*
        public async Task<IActionResult> CSGOMatches()
        {
            foreach (var matches in pastMatches)
            {
                _context.MatchesCS.Add(matches);
            }

            return View();
        }*/

        public ActionResult getPastCSGOMatches(string sort = "sort=-begin_at", string page = "&page=1", string filter = "running", string game = "csgo")
        {
            string url = "https://api.pandascore.co/csgo/matches/past?sort=draw&sort=&page=1&per_page=50&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";


            var jsonFilter = filter + "?";
            var jsonSort = sort;
            var jsonPage = page;
            var jsonPerPage = "&per_page = 10";
            var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
            var requestLink = "https://api.pandascore.co/" + game + "/matches/";

            var fullApiPath = requestLink + jsonFilter + jsonSort + jsonPage + jsonPerPage + token;
            Console.WriteLine(fullApiPath);

            var client = new RestClient(fullApiPath);
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            var json = client.Execute(request).Content;

            if (json == null)
            {
                return View(); //We need an error handler for this!
            }

            var jarray = JArray.Parse(json);
            List<MatchCS> pastMatches = new();

            var StaticResults = new[] { "16-12", "14-16", "16-9", "8-16", "10-16", "16-11", "3-16", "16-7" };
            var maps = new[] { "Inferno", "Mirage", "Nuke", "Overpass", "Vertigo", "Ancient", "Anubis" };
            var teamNames = new[] { "G2", "NAVI", "Liquid", "Furia", "BIG", "FTW", "FAZE" };


            foreach (JObject m in jarray.Cast<JObject>())
            {
                //Set up values from api
                Dictionary<int, string?> teamList = new();
                var ma = new MatchCS();
                JArray games = (JArray)m["games"]; 
                foreach (var gamesObject in games.Children<JObject>())
                {
                    //var MatchCSId;
                    var MatchCSAPIID = gamesObject.GetValue("id");
                    var WinnerTeamAPIId = gamesObject.GetValue("winner").Value<int>("id");

                    //var MatchesCSId;
                }
                var MatchesCSAPIId = m.GetValue("id");
                //var PlayerStatsList;

                var eventAPIID = m.GetValue("id");
                var nameStage = e.GetValue("name");
                var beginAt = e.GetValue("begin_at");
                var timeType = TimeType.Running;
                var league = e.GetValue("league");
                var teams = e.GetValue("teams");
                var prizePool = e.GetValue("prizepool");
                var winnerTeamId = e.GetValue("winner_id");
                var winnerTeamAPIId = e.GetValue("winner_id");

                //Handling for null values
                ma.Map = maps[_random.Next(maps.Length)];
                ma.RoundsScore = StaticResults[_random.Next(maps.Length)];
                ma.WinnerTeamName = teamNames[_random.Next(maps.Length)];


                ev.EventAPIID = eventAPIID.ToString() == null ? -1 : eventAPIID.Value<int>();
                ev.BeginAt = beginAt.ToString() == "" ? null : beginAt.Value<DateTime>();
                ev.TimeType = timeType;
                ev.EventName = league.ToString() == "" ? null : league.Value<string>("name");
                ev.LeagueName = nameStage.ToString() == "" ? null : nameStage.Value<string>();
                ev.PrizePool = prizePool.ToString() == "" ? "-" : new string(prizePool.Value<string>().Where(char.IsDigit).ToArray());
                ev.WinnerTeamAPIID = winnerTeamId.ToString() == "" ? -1 : winnerTeamId.Value<int>();

                if (teams != null)
                {
                    foreach (JObject o in teams)
                    {
                        var teamNameValue = o.GetValue("name");
                        var teamIdValue = o.GetValue("id");
                        var teamId = teamIdValue.ToString() == "" ? -1 : teamIdValue.Value<int>();
                        var teamName = teamNameValue.ToString() == "" ? null : teamNameValue.Value<string>();
                        teamList.Add(teamId, teamName);
                    }
                }

                //Filling remaining fields
                ev.TeamsList = teamList.Values.ToList();
                ev.WinnerTeamName = teamList.GetValueOrDefault((int)ev.WinnerTeamAPIID) ?? "-";
                events.Add(ev);

            }

            return null;

        }

        //De CSGO e de Valorant
        // GET: CSMatches
        public async Task<IActionResult> CSGOMatches()
        {
            List<MatchesCS> pastMatches = getAPICSGOMatches("https://api.pandascore.co/csgo/matches/past?sort=&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA");
            List<MatchesCS> runningMatches = getAPICSGOMatches("https://api.pandascore.co/csgo/matches/running?sort=&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA");
            List<MatchesCS> upcommingMatches = getAPICSGOMatches("https://api.pandascore.co/csgo/matches/upcoming?sort=&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA");

            List<int> dbMatchesIds = _context.MatchesCS.Select(m => m.MatchesCSAPIID).ToList();

            //Apenas os past são guardados, mas podem ser necessarios os running ou os upcomming
            foreach (var matches in pastMatches)
            {
                var id = matches.MatchesCSAPIID;

                if (!dbMatchesIds.Contains(id))
                {
                    //sss
                    _context.MatchesCS.Add(matches);

                    dbMatchesIds.Add(id);
                }
            }

            _context.SaveChanges();

            ViewBag["pastMatches"] = pastMatches;
            ViewBag["runningMatches"] = runningMatches;
            ViewBag["upcommingMatches"] = upcommingMatches;

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

                    if (!matchesWinner.ToString().Equals(""))
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
        /*
        private void GetMatchPlayerStatsCS()
        {
            var client = new RestClient("https://api.pandascore.co/csgo/matches/past?sort=&page=1&per_page=50");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            RestResponse response = client.Execute(request);

            JArray matchesArray = JArray.Parse(response.Content);


            MatchPlayerStatsCS pastMatchCs = new MatchPlayerStatsCS();

            foreach (var item in matchesArray.Children<JObject>())
            {
                MatchesCS matches = new MatchesCS();

                ICollection<MatchCS> matchesList = new List<MatchCS>();
                List<Guid> teamsList = new List<Guid>();
                ICollection<Stream> streamsList = new List<Stream>();

                matches.BeginAt = (DateTime)item["begin_at"];

                matches.HaveStats = (bool)item["detailed_stats"];

                matches.EndAt = (DateTime)item["end_at"];

                JArray matchArray = (JArray)item["games"];
                foreach (var match in matchArray.Children<JObject>())
                {
                    MatchCS matchCS = new MatchCS();

                    matchCS.MatchesCSId = matches.MatchesCSId;

                }

                foreach (JObject gameObject in gamesArray)
                {
                    int MatchCSId = (int)jObject["id"];
                    int PlayerCSId = (int)jObject["player_id"];
                    int Kills = (DateTime)jObject["kills"];
                    int Deaths = (DateTime)jObject["deaths"];
                    int Assists = (DateTime)jObject["assists"];
                    int FlashAssist = (DateTime)jObject["flash_assists"];
                    int ADR = (DateTime)jObject["adr"];
                    int HeadShots = (DateTime)jObject["headshots"];
                    int KD_Diff = (DateTime)jObject["k_d_diff"];
                    int PlayerName = (DateTime)jObject["first_name"];
                }
            }
        }
        */

        private void GetPastMatchCS()
        {
            List<MatchesCS> pastMatches = getAPICSGOMatches("https://api.pandascore.co/csgo/matches/past?sort=&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA");
            MatchesCS pastMatchesCs = new MatchesCS();
            MatchTeamsCS pastMatchTeamCs = new MatchTeamsCS();
            MatchCS pastMatchCs = new MatchCS();

            var StaticResults = new[] { "16-12", "14-16", "16-9", "8-16", "10-16", "16-11", "3-16", "16-7" };
            var maps = new[] { "Inferno", "Mirage", "Nuke", "Overpass", "Vertigo", "Ancient", "Anubis"};

            foreach (var item in statsArray.Children<JObject>())
            {
                pastMatchCs.MatchCSId = (int)item["games/match_id"];
                //pastMatchCs.MatchesCSId = pastMatchesCs.MatchesCSId; ir buscar o controller das matches que retorna as matchesid e igualar o
                //meu MatchesCSId com o MatchesCSId das matches
                //pastMatchCs.PlayerStatsList = ;
                pastMatchCs.RoundsScore = StaticResults[_random.Next(StaticResults.Length)];
                pastMatchCs.Map = maps[_random.Next(maps.Length)];
                pastMatchCs.TeamsList = pastMatchTeamCs.
                pastMatchCs.WinnerTeamId = _random.Next(1, 101);
                pastMatchCs.WinnerTeamName = _random.NextDouble();

                _context.Add(matches);
            }
        }

        private void GetRunningMatchCS()
        {
            var client = new RestClient("https://api.pandascore.co/csgo/matches/running?sort=&page=1&per_page=50");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            RestResponse response = client.Execute(request);

            JArray statsArray = JArray.Parse(response.Content);
            MatchCS runningMatchCs = new MatchCS();
        }

        private void GetUpComingMatchCS()
        {
            var client = new RestClient("https://api.pandascore.co/csgo/matches/upcoming?sort=&page=1&per_page=50");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            RestResponse response = client.Execute(request);

            JArray statsArray = JArray.Parse(response.Content);
            MatchCS upcomingMatchCs = new MatchCS();
        }

        private void GetPastMatchPlayerStatsCS()
        {
            //MatchCSId
            var client = new RestClient("https://api.pandascore.co/csgo/matches?&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            RestResponse response = client.Execute(request);

            JArray statsArray = JArray.Parse(response.Content);
            MatchPlayerStatsCS pastMatchCs = new MatchPlayerStatsCS();

            var nomes = new[] { "Aaliyah", "Aaron", "Abagail", "Abbey", "Abbie", "Abbigail", "Abby", "Abdiel" };

            foreach (var item in statsArray.Children<JObject>())
            {
                matches.MatchCSId = (int)item["games/match_id"];
                matches.PlayerCSId = _random.Next();
                matches.Kills = _random.Next(1, 31);
                matches.Deaths = _random.Next(1, 21);
                matches.Assists = _random.Next(1, 11);
                matches.FlashAssist = _random.Next(1, 6);
                matches.ADR = _random.Next(1, 101);
                matches.HeadShots = _random.NextDouble();
                matches.KD_Diff = _random.NextDouble();
                matches.PlayerName = nomes[_random.Next(nomes.Length)];

                _context.Add(matches);
            }
        }

        private void getMatchPlayerStatsVal()
        {
            //MatchCSId
            var client = new RestClient("https://api.pandascore.co/valorant/matches?sort=&page=1&per_page=50");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            RestResponse response = client.Execute(request);

            JArray statsArray = JArray.Parse(response.Content);
            MatchPlayerStatsVal matches = new MatchPlayerStatsVal();

            var nomes = new[] { "Aaliyah", "Aaron", "Abagail", "Abbey", "Abbie", "Abbigail", "Abby", "Abdiel" };

            foreach (var item in statsArray.Children<JObject>())
            {
                matches.MatchValId = (int)item["id"];
                matches.PlayerValId = _random.Next();
                matches.Kills = _random.Next(1, 31);
                matches.Deaths = _random.Next(1, 21);
                matches.Assists = _random.Next(1, 11);
                matches.ADR = _random.Next(1, 101);
                matches.HeadShots = _random.NextDouble();
                matches.KD_Diff = _random.NextDouble();
                matches.PlayerName = nomes[_random.Next(nomes.Length)];

                _context.Add(matches);
            }
        }


        private void getMatchTeamsCS()
        {
            //MatchCSId
            var client = new RestClient("https://api.pandascore.co/csgo/matches?&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            RestResponse response = client.Execute(request);

            JArray statsArray = JArray.Parse(response.Content);
            MatchTeamsCS matches = new MatchTeamsCS();

            var nomes = new[] { "Aaliyah", "Aaron", "Abagail", "Abbey", "Abbie", "Abbigail", "Abby", "Abdiel" };

            foreach (var item in statsArray.Children<JObject>())
            {
                matches.MatchCSId = (int)item["id"];
                matches.Name = (string)item["acronym"];
                matches.Location = (string)item["location"];
                matches.Image = (string)item["image_url"];

                JArray matchArray = (JArray)item["opponent"];
                foreach (var match in matchArray.Children<JObject>())
                {
                    matches.TeamCSId = (int)item["id"];

                }

                _context.Add(matches);
            }
        }

        private void getMatchTeamsVal() // em vez de ter isto a ir buscar ao list das matches
                                        // tenho de fazer um método para cada link da api ou seja um 
                                        // para upcoming, on going e past
        {
            //MatchCSId
            var client = new RestClient("https://api.pandascore.co/valorant/matches?sort=&page=1&per_page=50");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            RestResponse response = client.Execute(request);

            JArray statsArray = JArray.Parse(response.Content);
            MatchTeamsVal matches = new MatchTeamsVal();

            var nomes = new[] { "Aaliyah", "Aaron", "Abagail", "Abbey", "Abbie", "Abbigail", "Abby", "Abdiel" };

            foreach (var item in statsArray.Children<JObject>())
            {
                matches.MatchValId = (int)item["match_id"];
                matches.Name = (string)item["acronym"];
                matches.Location = (string)item["location"];
                matches.Image = (string)item["image_url"];

                JArray matchArray = (JArray)item["opponent"];
                foreach (var match in matchArray.Children<JObject>())
                {
                    matches.TeamValId = (int)item["id"];

                }

                _context.Add(matches);
            }
        }
    }
}
