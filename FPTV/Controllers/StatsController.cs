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
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Security.Policy;
using System.Xml.Linq;

namespace FPTV.Controllers
{
    public class StatsController : Controller
    {
        private readonly FPTVContext _context;
        Random _random = new Random();
        MatchesController _matchesController;
        List<MatchesCS> matchesCS = new List<MatchesCS>();

        public StatsController(FPTVContext context)
        {
            _context = context;
            _matchesController = new MatchesController(_context);
        }

        //De CSGO e de Valorant
        // GET: CSMatches

        public async Task<IActionResult> CSGOStats()
        {
            getCSGOMatches();
            return View("PlayerAndStats");
        }
        public async Task<IActionResult> PlayerandStatsCs(int id)
        {
            getCSGOMatches();
            return View("PlayerAndStats");
        }


        public ActionResult getPlayer(int id, string filter = "", string game = "csgo")
        {

            var ranking = new[] { 0.68F, 0.94F, 1.42F, 1.08F, 1.09F, 1.23F, 0.78F, 0.89F, 0.97F, 0.72F,
                0.82F, 0.62F, 1.45F, 1.11F, 1.37F, 1.27F, 1.05F, 1.07F, 1.16F, 1.29F, 1.15F, 0.97F, 0.83F,
                1.36F, 1.10F, 1.07F, 1.19F, 0.77F, 0.90F, 1.14F, 1.52F, 1.54F, 0.58F }; //de 0.58 a 1.54


            //Base url for requests
            var _requestLink = "https://api.pandascore.co/";

            //Filter to select from which pool to fetch the data (upcoming, running or finished/ended)
            var _jsonFilter = filter + "?";
            var _filterID = "filter[id]=132995"; //+ id.ToString();

            //THIS SHOULD BE A CLIENT SECRET
            var _token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";

            //Request processing with RestSharp
            var _fullRequest = _requestLink + game + "/teams?" + _filterID + _token;
        https://api.pandascore.co/csgo/teams?sort=&page=1&per_page=50&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA
            var _client = new RestClient(_fullRequest);
            var _request = new RestRequest("", Method.Get);
            _request.AddHeader("accept", "application/json");
            var _json = _client.Execute(_request).Content;
            var _jarray = JArray.Parse(_json);






            //Base url for requests
            var requestLink = "https://api.pandascore.co/";

            //Filter to select from which pool to fetch the data (upcoming, running or finished/ended)
            var jsonFilter = filter + "?";
            var filterID = "filter[id]=48495"; //+ id.ToString();

            //THIS SHOULD BE A CLIENT SECRET
            var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";

            //Request processing with RestSharp
            var fullRequest = requestLink + game + "/players?" + filterID + token;
            var client = new RestClient(fullRequest);
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            var json = client.Execute(request).Content;
            var jarray = JArray.Parse(json);



            var player = new MatchPlayerStatsCS();
            var _player = new Player();
            _player.Teams = new List<Team>();
            var teamm = new Team();
            _player.Teams.Add(teamm);
            var KdRatio = player.Kills / player.Deaths;

            _player.Rating = ranking[_random.Next(ranking.Length)];

            foreach (var item in jarray.Cast<JObject>())
            {
                player.PlayerCSAPIId = (int)item.GetValue("id");
                player.Kills = _random.Next(30, 301);
                player.Deaths = _random.Next(30, 300);
                player.Assists = _random.Next(1, 11); ;
                player.FlashAssist = _random.Next(1, 6); ;
                player.ADR = _random.NextDouble();
                player.HeadShots = Math.Round(_random.NextDouble() * 100, 2);
                player.KD_Diff = _random.NextDouble();
                player.PlayerName = (string)item.GetValue("name");

                //_player.Age = (int?)item.GetValue("age");
                _player.Age = item.GetValue("age") == null ? 20 : item.GetValue("age").Value<int>();
                _player.Nacionality = (string)item.GetValue("nationality");
                //_player.Image = (string)item.GetValue("image_url");
                _player.Image = item.GetValue("image_url").ToString() == "" ? "/images/default-profile-icon-24.jpg" : item.GetValue("image_url").Value<string>();

                _context.MatchPlayerStatsCS.Add(player);
            }

            foreach (var _item in _jarray.Cast<JObject>())
            {
                var _id = (int)_item.GetValue("id");
                if (_id == player.PlayerCSAPIId)
                {
                    _player.PlayerAPIId = player.PlayerCSAPIId;
                    _player.Name = player.PlayerName;
                }
                teamm.Name = (string ?)_item.GetValue("name");
                teamm.Image = (string?)_item.GetValue("image_url");
            }

            ViewBag.player = player;
            ViewBag._player = _player;
            ViewBag.KdRatio = KdRatio;

            return View("PlayerAndStats");
        }


        private String request(string category, string sort = "sort=-status", string page = "&page=1", string filter = "past", string game = "csgo")
        {
            if (category == "matches")
            {
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
            }
            else if (category == "teams")
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
            else
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

        }

        private void getCSGOMatches(string time = "past")
        {
            //string url =https://api.pandascore.co/csgo/matches/past?sort=draw&sort=&page=1&per_page=50&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA

            var jsonMatches = request("matches");
            var jsonTeams = request("teams");
            var jsonPlayers = request("players");

            var jarrayMatches = JArray.Parse(jsonMatches);
            var jarrayTeams = JArray.Parse(jsonTeams);
            var jarrayPlayers = JArray.Parse(jsonPlayers);



            var StaticResults = new[] { "16-12", "14-16", "16-9", "8-16", "10-16", "16-11", "3-16", "16-7" };
            var maps = new[] { "Inferno", "Mirage", "Nuke", "Overpass", "Vertigo", "Ancient", "Anubis" };
            var teamNames = new[] { "G2", "NAVI", "Liquid", "Furia", "BIG", "FTW", "FAZE" };
            var coachNames = new[] { "Rui", "Nuno", "Miguel", "André", "João", "Guilherme" };
            var ranking = new[] { 0.68F, 0.94F, 1.42F, 1.08F, 1.09F, 1.23F, 0.78F, 0.89F, 0.97F, 0.72F,
                0.82F, 0.62F, 1.45F, 1.11F, 1.37F, 1.27F, 1.05F, 1.07F, 1.16F, 1.29F, 1.15F, 0.97F, 0.83F,
                1.36F, 1.10F, 1.07F, 1.19F, 0.77F, 0.90F, 1.14F, 1.52F, 1.54F, 0.58F }; //de 0.58 a 1.54

            List<MatchCS>? matchCsList = new();
            List<MatchPlayerStatsCS>? playerStatsList = new();
            List<MatchTeamsCS>? teamsList = new();

            foreach (var item in jarrayMatches.Cast<JObject>())
            {
                var status = item.GetValue("status");

                if (!status.ToString().Equals("canceled"))
                {
                    var opponentArray = (JArray)item.GetValue("opponents");
                    var ma = new MatchCS();

                    var matches = new MatchesCS();
                    matches.MatchesList.Add(ma);
                    matches.MatchesCSAPIID = (int)item.GetValue("id");
                    ma.MatchesCSAPIId = matches.MatchesCSAPIID;
                    ma.RoundsScore = StaticResults[_random.Next(maps.Length)];
                    ma.Map = maps[_random.Next(maps.Length)];



                    foreach (var opponentObject in opponentArray.Cast<JObject>())
                    {
                        var opponent = (JObject)opponentObject.GetValue("opponent");

                        var teamIdValue = opponent.GetValue("id");
                        var teamImage = opponent.GetValue("image_url");
                        var teamName = opponent.GetValue("name");
                        var teamId = teamIdValue.ToString() == "" ? -1 : teamIdValue.Value<int>();

                        var team = new Team();
                        team.TeamAPIID = teamId;
                        team.Name = teamName.ToString() == "?" ? "undefined" : teamName.Value<string>();
                        team.Image = teamImage.ToString() == "" ? "/images/logo1.jpg" : teamImage.Value<string>();

                        var fullApiPath = "https://api.pandascore.co/csgo/teams?filter[id]=" + teamId + "&sort=&page=1&per_page=50&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
                        var client = new RestClient(fullApiPath);
                        var request = new RestRequest("", Method.Get);
                        request.AddHeader("accept", "application/json");
                        var response = client.Execute(request).Content;
                        var teamsArray = JArray.Parse(response);




                        foreach (var _team in teamsArray.Cast<JObject>())
                        {
                            team.CouchName = coachNames[_random.Next(coachNames.Length)]; ;
                            team.WorldRank = 1;
                            team.Winnings = 1;
                            team.Losses = 1;
                            var players = (JArray)_team.GetValue("players");
                            foreach (var _player in players.Cast<JObject>())
                            {
                                //inicializar cada player
                                var player = new Player();
                                player.PlayerAPIId = (int)_player.GetValue("id");
                                player.Name = (string)_player.GetValue("name");
                                player.Age = (int)_player.GetValue("age");
                                player.Rating = ranking[_random.Next(ranking.Length)];
                                player.Teams.Add(team);
                            }

                        }
                        var matchTeam = new MatchTeamsCS();
                        matchTeam.MatchCSAPIID = (int)item.GetValue("id");
                        matchTeam.TeamCSAPIId = (int)team.TeamAPIID;
                        matchTeam.TeamCS = team;
                        matchTeam.Name = (string)opponent.GetValue("name");
                        matchTeam.Location = (string?)opponent.GetValue("location");
                        matchTeam.Image = (string)opponent.GetValue("image_url");
                        ma.TeamsList.Add(matchTeam);

                        JArray games = (JArray)item["games"];
                        foreach (var game in games.Cast<JObject>())
                        {
                            var winnerTeam = new Team();
                            var winner = game.GetValue("winner");
                            winnerTeam.TeamAPIID = winner.Value<int>("id");
                            ma.WinnerTeam = winnerTeam;
                            ma.WinnerTeamAPIId = winnerTeam.TeamAPIID;
                            ma.MatchCSAPIID = (int)game.GetValue("id");
                            //var MatchCSAPIID = games.GetValue("id");
                            //var MatchesCSAPIId = (int)m.GetValue("id");

                            //var winnerTeamAPIId = winner.ToString() == "" ? null : winner.ToObject<JObject>().GetValue("id");
                        }
                        matchCsList.Add(ma);
                        _context.MatchCS.Add(ma);
                        teamsList.Add(matchTeam);
                        _context.MatchTeamsCS.Add(matchTeam);


                        foreach (JObject t in jarrayTeams.Cast<JObject>())
                        {
                            foreach (JObject p in jarrayPlayers.Cast<JObject>())
                            {
                                var player = new MatchPlayerStatsCS();
                                player.MatchCS = ma;
                                player.MatchCSAPIID = (int)item.GetValue("id");
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
                                ma.PlayerStatsList.Add(player);
                                _context.MatchPlayerStatsCS.Add(player);
                            }
                        }
                        ViewBag.playerStatsList = playerStatsList;
                        ViewBag.teamsList = teamsList;
                        ViewBag.matchCsList = matchCsList;
                    }
                    /*
                    //JArray games = (JArray)item["games"];
                        foreach (var gamesObject in games.Children<JObject>())
                        {
                            var ma = new MatchCS();
                            var matches = new MatchesCS();
                            matches.MatchesList.Add(ma);
                            matches.MatchesCSAPIID = (int)m.GetValue("id");
                            var MatchCSAPIID = gamesObject.GetValue("id");
                            var MatchesCSAPIId = (int)m.GetValue("id");
                            ma.RoundsScore = StaticResults[_random.Next(maps.Length)];
                            ma.Map = maps[_random.Next(maps.Length)];
                            //var winnerTeamAPIId = winner.ToString() == "" ? null : winner.ToObject<JObject>().GetValue("id");
                            ma.WinnerTeamName = teamNames[_random.Next(maps.Length)];
                            ma.MatchCSAPIID = (int)MatchCSAPIID;
                            ma.WinnerTeamAPIId = winnerTeamAPIId.ToString() == "" ? -1 : winnerTeamAPIId.Value<int>();
                            matchCsList.Add(ma);
                            _context.MatchCS.Add(ma);
                        }
                }*/
                }
                _context.SaveChanges();
            }
        }
    }
}
