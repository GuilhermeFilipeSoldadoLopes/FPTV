using AngleSharp.Io;
using FPTV.Data;
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

namespace FPTV.Controllers
{
	/*public class StatsController : Controller
    {
        private readonly FPTVContext _context;
        Random _random = new Random();

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
        private void GetPastMatchCS()
        {
            var client = new RestClient("https://api.pandascore.co/csgo/matches/past?sort=&page=1&per_page=50");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            RestResponse response = client.Execute(request);

            JArray statsArray = JArray.Parse(response.Content);
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
    }*/
}