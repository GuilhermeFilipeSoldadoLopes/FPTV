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
using System.Numerics;
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
        
        public async Task<IActionResult> CSGOStats()
        {
            getPastCSGOMatches();
            return View();
        }



        private String request(string category, string sort = "sort=-begin_at", string page = "&page=1", string filter = "past", string game = "csgo")
        {
            if (category == "matches") { 
                var jsonFilter = filter + "?";
                var jsonSort = sort;
                var jsonPage = page;
                var jsonPerPage = "&per_page=10";
                var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
                var requestLink = "https://api.pandascore.co/" + game + "/" + category + "/";
                var fullApiPath = requestLink + jsonFilter + jsonSort + jsonPage + jsonPerPage + token;

                var client = new RestClient(fullApiPath);
                var request = new RestRequest("", Method.Get);
                request.AddHeader("accept", "application/json");

                return client.Execute(request).Content;
            } else if(category == "teams")
            {
                var jsonFilter = filter + "?";
                var jsonSort = sort;
                var jsonPage = page;
                var jsonPerPage = "&per_page=10";
                var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
                var requestLink = "https://api.pandascore.co/" + game + "/" + category + "/";
                var fullApiPath = requestLink + "?" + jsonPage + jsonPerPage + token;

                var client = new RestClient(fullApiPath);
                var request = new RestRequest("", Method.Get);
                request.AddHeader("accept", "application/json");

                return client.Execute(request).Content;
            }
            else{
                var jsonFilter = filter + "?";
                var jsonSort = sort;
                var jsonPage = page;
                var jsonPerPage = "&per_page=10";
                var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
                var requestLink = "https://api.pandascore.co/" + game + "/" + category + "/";
                var fullApiPath = requestLink + "?" + jsonPage + jsonPerPage + token;

                var client = new RestClient(fullApiPath);
                var request = new RestRequest("", Method.Get);
                request.AddHeader("accept", "application/json");

                return client.Execute(request).Content;
            }

        }

        private void getPastCSGOMatches()
        {
            string url = "https://api.pandascore.co/csgo/matches/past?sort=draw&sort=&page=1&per_page=50&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";

            var jsonMatches = request("matches");
            var jsonTeams = request("teams");
            var jsonPlayers = request("players");

            var jarrayMatches = JArray.Parse(jsonMatches);
            var jarrayTeams = JArray.Parse(jsonTeams);
            var jarrayPlayers = JArray.Parse(jsonPlayers);
            List<MatchCS> pastMatches = new();

            var StaticResults = new[] { "16-12", "14-16", "16-9", "8-16", "10-16", "16-11", "3-16", "16-7" };
            var maps = new[] { "Inferno", "Mirage", "Nuke", "Overpass", "Vertigo", "Ancient", "Anubis" };
            var teamNames = new[] { "G2", "NAVI", "Liquid", "Furia", "BIG", "FTW", "FAZE" };



            foreach (JObject m in jarrayMatches.Cast<JObject>())
            {
                List<MatchCS>? matchCsList = new();
                List<MatchPlayerStatsCS>? playerStatsList = new();
                List<MatchTeamsCS>? teamsList = new();

                JArray games = (JArray)m["games"];
                foreach (var gamesObject in games.Children<JObject>())
                {
                    var ma = new MatchCS();
                    


                    var MatchCSAPIID = gamesObject.GetValue("id");
                    var MatchesCSAPIId = (int)m.GetValue("id");
                    var winner = gamesObject.GetValue("winner");
                    var winnerTeamAPIId = winner.ToString() == "" ? null : winner.ToObject<JObject>().GetValue("id");
                    ma.Map = maps[_random.Next(maps.Length)];
                    ma.RoundsScore = StaticResults[_random.Next(maps.Length)];
                    ma.WinnerTeamName = teamNames[_random.Next(maps.Length)];
                    ma.MatchCSAPIID = (int)MatchCSAPIID;
                    ma.WinnerTeamAPIId = winnerTeamAPIId.ToString() == "" ? -1 : winnerTeamAPIId.Value<int>();
                    matchCsList.Add(ma);
                    _context.MatchCS.Add(ma);
                    foreach (JObject t in jarrayTeams.Cast<JObject>())
                    {
                        var team = new MatchTeamsCS();
                        team.MatchCSAPIID = ma.MatchCSAPIID;
                        team.TeamCSAPIId = (int)t.GetValue("id");
                        team.Name = (string)t.GetValue("name");
                        team.Location = (string)t.GetValue("location");
                        team.Image = (string)t.GetValue("image_url");
                        teamsList.Add(team);

                        _context.MatchTeamsCS.Add(team);
                        foreach (JObject p in jarrayPlayers.Cast<JObject>())
                        {
                            var player = new MatchPlayerStatsCS();
                            player.MatchCSAPIID = ma.MatchCSAPIID;
                            player.PlayerCSAPIId = (int)t.GetValue("id");
                            player.Kills = _random.Next(1, 31);
                            player.Deaths = _random.Next(1, 21);
                            player.Assists = _random.Next(1, 11); ;
                            player.FlashAssist = _random.Next(1, 6); ;
                            player.ADR = _random.NextDouble();
                            player.HeadShots = _random.NextDouble() * 100;
                            player.KD_Diff = _random.NextDouble();
                            player.PlayerName = (string)t.GetValue("name"); ;
                            playerStatsList.Add(player);
                            _context.MatchPlayerStatsCS.Add(player);
                        }
                        _context.MatchTeamsCS.Add(team);
                    }
                    ViewBag.playerStatsList = playerStatsList;
                    ViewBag.teamsList = teamsList;
                    ViewBag.matchCsList = matchCsList;
                }
                _context.SaveChanges();
            }
        }
    }
}
