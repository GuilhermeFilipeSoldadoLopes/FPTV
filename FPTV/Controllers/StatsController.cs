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
using NuGet.ProjectModel;
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
        /*
        public async Task<IActionResult> MatchDetails()
        {
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
                    List<MatchCS> matchesCs = new List<MatchCS>();
                    matchesCs.Add(ma);
                    matches.MatchesList = matchesCs;
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
                                player.PlayerCSAPIId = (int)p.GetValue("id");
                                player.Kills = _random.Next(1, 31);
                                player.Deaths = _random.Next(1, 21);
                                player.Assists = _random.Next(1, 11); ;
                                player.FlashAssist = _random.Next(1, 6); ;
                                player.ADR = _random.NextDouble();
                                player.HeadShots = _random.NextDouble() * 100;
                                player.KD_Diff = _random.NextDouble();
                                player.PlayerName = (string)p.GetValue("name"); ;
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
                }
                }
                _context.SaveChanges();
            }
            return View("MatchDeatils");
        }*/


        public async Task<IActionResult> PlayerandStatsCs(int id)
        {
            getCSGOMatchesAsync();
            return View("PlayerAndStats");
        }

        public ActionResult getTeam(int id = 132991, string filter = "past", string game = "csgo", string page = "&page=1")
        {
            if (game == "valorant")
            {

                var ranking = new[] { 0.68F, 0.94F, 1.42F, 1.08F, 1.09F, 1.23F, 0.78F, 0.89F, 0.97F, 0.72F,
                    0.82F, 0.62F, 1.45F, 1.11F, 1.37F, 1.27F, 1.05F, 1.07F, 1.16F, 1.29F, 1.15F, 0.97F, 0.83F,
                    1.36F, 1.10F, 1.07F, 1.19F, 0.77F, 0.90F, 1.14F, 1.52F, 1.54F, 0.58F }; //de 0.58 a 1.54

                var teamsList = new[] { "G2", "Heroic", "Natus Vincere", "Liquid", "Vitality", "Outsiders", "Faze",
                    "Complexity", "fnatic", "Cloud9", "Spirit", "Astralis", "MOUZ", "FURIA ", "BIG", "Ninjas in Pyjamas",
                    "IHC", "Eternal Fire", "ENCE", "FORZE", "Bad News Eagles", "MIBR", "Movistar Riders", "9INE", "paiN",
                    "GamerLegion", "Aurora", "Rare Atom", "Grayhound", "NRG", "SAW", "Avangar", "Spirit", "Nexus", "Grayhound",
                    "TYLOO", "Renegates", "SINNERS", "HellRaisers", "Club Brugge", "North", "Dignitas", "Luminosity", "TeamOne",
                    "Sprout", "Cheifs", "SK", "Endpoint", "GODSENT", "Envy", "HAVU", "Envy", "Gambit" };


                //Base url for requests
                var _requestLink = "https://api.pandascore.co/";

                //Filter to select from which pool to fetch the data (upcoming, running or finished/ended)
                var _jsonFilter = filter + "?";
                var _filterID = "filter[id]=132995"; //+ id.ToString();

                //THIS SHOULD BE A CLIENT SECRET
                var _token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";

                //Request processing with RestSharp
                var _fullRequest = _requestLink + game + "/teams?" + _filterID + _token;
            https://api.pandascore.co/valorant/teams?sort=&page=1&per_page=50&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA
                var _client = new RestClient(_fullRequest);
                var _request = new RestRequest("", Method.Get);
                _request.AddHeader("accept", "application/json");
                var _json = _client.Execute(_request).Content;
                var _jarray = JArray.Parse(_json);






                //Base url for requests
                var requestLink = "https://api.pandascore.co/";

                //Filter to select from which pool to fetch the data (upcoming, running or finished/ended)
                var jsonFilter = filter + "?";
                var filterID = "filter[id]=" + id.ToString();

                //THIS SHOULD BE A CLIENT SECRET
                var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";

                //Request processing with RestSharp
                var fullRequest = requestLink + game + "/players?" + filterID + token;
                var client = new RestClient(fullRequest);
                var request = new RestRequest("", Method.Get);
                request.AddHeader("accept", "application/json");
                var json = client.Execute(request).Content;
                var jarray = JArray.Parse(json);



                var player = new MatchPlayerStatsVal();
                var _player = new Player();
                _player.Teams = new List<Team>();
                var teamm = new Team();
                _player.Teams.Add(teamm);


                _player.Rating = ranking[_random.Next(ranking.Length)];

                foreach (var item in jarray.Cast<JObject>())
                {
                    player.MatchValAPIID = (int)item.GetValue("id");
                    player.Kills = _random.Next(30, 301);
                    player.Deaths = _random.Next(30, 300);
                    player.Assists = _random.Next(1, 11); ;
                    player.ADR = _random.NextDouble();
                    player.HeadShots = Math.Round(_random.NextDouble() * 100, 2);
                    player.KD_Diff = _random.NextDouble();
                    player.PlayerName = (string)item.GetValue("name");

                    //_player.Age = (int?)item.GetValue("age");
                    _player.Age = item.GetValue("age") == null ? 20 : item.GetValue("age").Value<int>();
                    _player.Nationality = (string)item.GetValue("nationality");
                    //_player.Image = (string)item.GetValue("image_url");
                    _player.Image = item.GetValue("image_url").ToString() == "" ? "/images/default-profile-icon-24.jpg" : item.GetValue("image_url").Value<string>();

                    _context.MatchPlayerStatsVal.Add(player);
                }

                foreach (var _item in _jarray.Cast<JObject>())
                {
                    var _id = (int)_item.GetValue("id");
                    if (_id == player.PlayerValAPIId)
                    {
                        _player.PlayerAPIId = player.PlayerValAPIId;
                        _player.Name = player.PlayerName;
                    }
                    teamm.Name = (string?)_item.GetValue("name");
                    teamm.Image = (string?)_item.GetValue("image_url");
                }
                double KdRatio = (double)player.Kills / (double)player.Deaths;
                int maps = _random.Next(1, 8);
                var pastTeam1 = teamsList[_random.Next(teamsList.Length)];
                var pastTeam2 = teamsList[_random.Next(teamsList.Length)];
                var pastTeam3 = teamsList[_random.Next(teamsList.Length)];
                ViewBag.KdRatio = Math.Round(KdRatio, 2);
                ViewBag.maps = maps;
                ViewBag.player = player;
                ViewBag._player = _player;
                ViewBag.pastTeam1 = pastTeam1;
                ViewBag.pastTeam2 = pastTeam2;
                ViewBag.pastTeam3 = pastTeam3;

                return View("TeamStats");
            }
            else
            {
                var ranking = new[] { 0.68F, 0.94F, 1.42F, 1.08F, 1.09F, 1.23F, 0.78F, 0.89F, 0.97F, 0.72F,
                    0.82F, 0.62F, 1.45F, 1.11F, 1.37F, 1.27F, 1.05F, 1.07F, 1.16F, 1.29F, 1.15F, 0.97F, 0.83F,
                    1.36F, 1.10F, 1.07F, 1.19F, 0.77F, 0.90F, 1.14F, 1.52F, 1.54F, 0.58F }; //de 0.58 a 1.54

                //var teamsList = new[] { "G2", "Heroic", "Natus Vincere", "Liquid", "Vitality", "Outsiders", "Faze",
                //    "Complexity", "fnatic", "Cloud9", "Spirit", "Astralis", "MOUZ", "FURIA ", "BIG", "Ninjas in Pyjamas",
                //    "IHC", "Eternal Fire", "ENCE", "FORZE", "Bad News Eagles", "MIBR", "Movistar Riders", "9INE", "paiN",
                //    "GamerLegion", "Aurora", "Rare Atom", "Grayhound", "NRG", "SAW", "Avangar", "Spirit", "Nexus", "Grayhound",
                //    "TYLOO", "Renegates", "SINNERS", "HellRaisers", "Club Brugge", "North", "Dignitas", "Luminosity", "TeamOne",
                //    "Sprout", "Cheifs", "SK", "Endpoint", "GODSENT", "Envy", "HAVU", "Envy", "Gambit" };


                //Base url for requests
                var _requestLink = "https://api.pandascore.co/";

                //Filter to select from which pool to fetch the data (upcoming, running or finished/ended)
                var _jsonFilter = filter + "?";
                var _filterID = "filter[id]=" + id.ToString();

                //THIS SHOULD BE A CLIENT SECRET
                var _token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
                var jsonSort = "&sort=";
                var jsonPage = page;
                var jsonPerPage = "&per_page=50";

                //Request processing with RestSharp
                var _fullRequest = _requestLink + game + "/teams?" + _filterID + jsonSort + jsonPage + jsonPerPage + _token;
                var _client = new RestClient(_fullRequest);
                var _request = new RestRequest("", Method.Get);
                _request.AddHeader("accept", "application/json");
                var _json = _client.Execute(_request).Content;
                var _jarray = JArray.Parse(_json);

                var teamsList = new List<Team>();
                var coachNames = new[] { "Rui", "Nuno", "Miguel", "André", "João", "Guilherme" };


                //foreach (var _team in _jarray.Cast<JObject>())
                //{

                //    var team = new Team();
                //    team.TeamAPIID = (int)_team.GetValue("id");
                //    //if(teamm.TeamAPIID == _player.)
                //    team.Name = _team.GetValue("name").ToString() == "" ? "undefined" : _team.GetValue("name").Value<string>();
                //    team.Image = _team.GetValue("image_url").ToString() == "" ? "/images/missing.png" : _team.GetValue("image_url").Value<string>();
                //    team.CoachName = coachNames[_random.Next(coachNames.Length)];
                //    team.WorldRank = 1;
                //    team.Winnings = 1;
                //    team.Losses = 1;
                //    team.Game = GameType.CSGO;
                //    teamsList.Add(team);

                //}


                //Base url for requests
                var requestLink = "https://api.pandascore.co/";

                //Filter to select from which pool to fetch the data (upcoming, running or finished/ended)
                var jsonFilter = filter + "?";
                var filterID = "filter[id]="; //+ id.ToString();
                //THIS SHOULD BE A CLIENT SECRET
                var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";

                //Request processing with RestSharp
                var fullRequest = requestLink + game + "/players?" + filterID + token;
                var client = new RestClient(fullRequest);
                var request = new RestRequest("", Method.Get);
                request.AddHeader("accept", "application/json");
                var json = client.Execute(request).Content;
                var jarray = JArray.Parse(json);



                var team = new Team();
                foreach (var _team in _jarray.Cast<JObject>())
                {
                    team.TeamAPIID = (int)_team.GetValue("id");
                    //if(teamm.TeamAPIID == _player.)
                    team.Name = _team.GetValue("name").ToString() == "" ? "undefined" : _team.GetValue("name").Value<string>();
                    team.Image = _team.GetValue("image_url").ToString() == "" ? "/images/missing.png" : _team.GetValue("image_url").Value<string>();
                    team.CoachName = coachNames[_random.Next(coachNames.Length)];
                    team.WorldRank = 1;
                    team.Winnings = 1;
                    team.Losses = 1;
                    team.Game = GameType.CSGO;
                    var playersOfTeam = (JArray)_team.GetValue("players");
                    foreach (var playerObject in playersOfTeam.Cast<JObject>())
                    {
                        var player = new Player();
                        player.PlayerAPIId = (int)playerObject.GetValue("id");
                        player.Age = playerObject.GetValue("age") == null ? 20 : playerObject.GetValue("age").Value<int>();
                        player.Nationality = (string)playerObject.GetValue("nationality");
                        ViewBag.NacionalityImg = "/images/Flags/4x3/" + player.Nationality + ".svg";
                        //_player.Image = (string)item.GetValue("image_url");
                        player.Image = playerObject.GetValue("image_url").ToString() == "" ? "/images/default-profile-icon-24.jpg" : playerObject.GetValue("image_url").Value<string>();
                        player.Rating = ranking[_random.Next(ranking.Length)];
                        player.Name = (string)playerObject.GetValue("name");
                        team.Players.Add(player);
                    }

                }

                ViewBag.team = team;




                //foreach (var __player in jarray.Cast<JObject>())
                //{

                //    var player = new Player();
                //    player.PlayerAPIId = (int)_team.GetValue("id");
                //    //if(teamm.TeamAPIID == _player.)
                //    player.Name = _team.GetValue("name").ToString() == "" ? "undefined" : _team.GetValue("name").Value<string>();
                //    player.Image = _team.GetValue("image_url").ToString() == "" ? "/images/missing.png" : _team.GetValue("image_url").Value<string>();
                //    player.CoachName = coachNames[_random.Next(coachNames.Length)];
                //    player.WorldRank = 1;
                //    player.Winnings = 1;
                //    player.Losses = 1;
                //    player.Game = GameType.CSGO;

                //    _player.Teams = new List<Team>();
                //    playersList.Add(player);
                //}

                //foreach (var item in _jarray.Cast<JObject>())
                //{

                //}


                //    _player.Rating = ranking[_random.Next(ranking.Length)];

                //foreach (var item in jarray.Cast<JObject>())
                //{
                //    player.PlayerValAPIId = (int)item.GetValue("id");
                //    player.Kills = _random.Next(30, 301);
                //    Console.WriteLine("Kills ------------->" + player.Kills);
                //    player.Deaths = _random.Next(30, 300);
                //    Console.WriteLine("Deaths ------------->" + player.Deaths);
                //    player.Assists = _random.Next(1, 11); ;
                //    player.ADR = _random.NextDouble();
                //    player.HeadShots = Math.Round(_random.NextDouble() * 100, 2);
                //    player.KD_Diff = _random.NextDouble();
                //    player.PlayerName = (string)item.GetValue("name");

                //    //_player.Age = (int?)item.GetValue("age");
                //    _player.Age = item.GetValue("age") == null ? 20 : item.GetValue("age").Value<int>();
                //    _player.Nationality = (string)item.GetValue("nationality");
                //    ViewBag.NacionalityImg = "/images/Flags/4x3/" + _player.Nationality + ".svg";
                //    //_player.Image = (string)item.GetValue("image_url");
                //    _player.Image = item.GetValue("image_url").ToString() == "" ? "/images/default-profile-icon-24.jpg" : item.GetValue("image_url").Value<string>();
                //    _player.Rating = ranking[_random.Next(ranking.Length)];
                //    _player.PlayerAPIId = player.PlayerValAPIId;
                //    _player.Name = player.PlayerName;

                //    var current_team = (JObject)item.GetValue("current_team");

                //    var current_team_id = (int)current_team.GetValue("id");
                //    teamm.TeamAPIID = (int)current_team.GetValue("id");
                //    //if(teamm.TeamAPIID == _player.)
                //    teamm.Name = current_team.GetValue("name").ToString() == "" ? "undefined" : current_team.GetValue("name").Value<string>();
                //    teamm.Image = current_team.GetValue("image_url").ToString() == "" ? "/images/logo1.jpg" : current_team.GetValue("image_url").Value<string>();
                //    teamm.CoachName = coachNames[_random.Next(coachNames.Length)];
                //    teamm.WorldRank = 1;
                //    teamm.Winnings = 1;
                //    teamm.Losses = 1;
                //    teamm.Game = GameType.CSGO;

                //    var pastTeam1 = teamm;
                //    var pastTeam2 = teamsList[_random.Next(teamsList.Count())];
                //    var pastTeam3 = teamsList[_random.Next(teamsList.Count())];
                //    ViewBag.pastTeam1 = pastTeam1;
                //    ViewBag.pastTeam2 = pastTeam2;
                //    ViewBag.pastTeam3 = pastTeam3;
                //    _player.Teams.Add(teamm);
                //    _player.Teams.Add(pastTeam2);
                //    _player.Teams.Add(pastTeam3);

                //    _context.MatchPlayerStatsVal.Add(player);
                //}


                //double KdRatio = (double)player.Kills / (double)player.Deaths;
                //double roundKdRatio = Math.Round(KdRatio, 2);
                //int maps = _random.Next(1, 8);
                //ViewBag.roundKdRatio = roundKdRatio;
                //ViewBag.maps = maps;
                //ViewBag.player = player;
                //ViewBag._player = _player;
                //ViewBag.teamsList = teamsList;

                id = id;
                ViewBag.id = id;
                return View("TeamStats");
            }


            return null;
        }

        public ActionResult getPlayer(int id= 132995, string filter = "past", string game = "csgo", string page = "&page=1")
        {
            if (game == "valorant")
            {

                var ranking = new[] { 0.68F, 0.94F, 1.42F, 1.08F, 1.09F, 1.23F, 0.78F, 0.89F, 0.97F, 0.72F,
                    0.82F, 0.62F, 1.45F, 1.11F, 1.37F, 1.27F, 1.05F, 1.07F, 1.16F, 1.29F, 1.15F, 0.97F, 0.83F,
                    1.36F, 1.10F, 1.07F, 1.19F, 0.77F, 0.90F, 1.14F, 1.52F, 1.54F, 0.58F }; //de 0.58 a 1.54

                //var teamsList = new[] { "G2", "Heroic", "Natus Vincere", "Liquid", "Vitality", "Outsiders", "Faze",
                //    "Complexity", "fnatic", "Cloud9", "Spirit", "Astralis", "MOUZ", "FURIA ", "BIG", "Ninjas in Pyjamas",
                //    "IHC", "Eternal Fire", "ENCE", "FORZE", "Bad News Eagles", "MIBR", "Movistar Riders", "9INE", "paiN",
                //    "GamerLegion", "Aurora", "Rare Atom", "Grayhound", "NRG", "SAW", "Avangar", "Spirit", "Nexus", "Grayhound",
                //    "TYLOO", "Renegates", "SINNERS", "HellRaisers", "Club Brugge", "North", "Dignitas", "Luminosity", "TeamOne",
                //    "Sprout", "Cheifs", "SK", "Endpoint", "GODSENT", "Envy", "HAVU", "Envy", "Gambit" };


                //Base url for requests
                var _requestLink = "https://api.pandascore.co/";

                //Filter to select from which pool to fetch the data (upcoming, running or finished/ended)
                var _jsonFilter = filter + "?";
                var _filterID = "filter[id]="; //+ id.ToString();

                //THIS SHOULD BE A CLIENT SECRET
                var _token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
                var jsonSort = "&sort=";
                var jsonPage = page;
                var jsonPerPage = "&per_page=50";

                //Request processing with RestSharp
                var _fullRequest = _requestLink + game + "/teams?" + _filterID + jsonSort + jsonPage + jsonPerPage + _token;
                var _client = new RestClient(_fullRequest);
                var _request = new RestRequest("", Method.Get);
                _request.AddHeader("accept", "application/json");
                var _json = _client.Execute(_request).Content;
                var _jarray = JArray.Parse(_json);

                var teamsList = new List<Team>();
                var coachNames = new[] { "Rui", "Nuno", "Miguel", "André", "João", "Guilherme" };


                foreach (var _team in _jarray.Cast<JObject>())
                {

                    var team = new Team();
                    team.TeamAPIID = (int)_team.GetValue("id");
                    //if(teamm.TeamAPIID == _player.)
                    team.Name = _team.GetValue("name").ToString() == "" ? "undefined" : _team.GetValue("name").Value<string>();
                    team.Image = _team.GetValue("image_url").ToString() == "" ? "/images/missing.png" : _team.GetValue("image_url").Value<string>();
                    team.CoachName = coachNames[_random.Next(coachNames.Length)];
                    team.WorldRank = 1;
                    team.Winnings = 1;
                    team.Losses = 1;
                    team.Game = GameType.CSGO;
                    teamsList.Add(team);

                }


                //Base url for requests
                var requestLink = "https://api.pandascore.co/";

                //Filter to select from which pool to fetch the data (upcoming, running or finished/ended)
                var jsonFilter = filter + "?";
                var filterID = "filter[id]=" + id.ToString();

                //THIS SHOULD BE A CLIENT SECRET
                var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";

                //Request processing with RestSharp
                var fullRequest = requestLink + game + "/players?" + filterID + token;
                var client = new RestClient(fullRequest);
                var request = new RestRequest("", Method.Get);
                request.AddHeader("accept", "application/json");
                var json = client.Execute(request).Content;
                var jarray = JArray.Parse(json);



                var player = new MatchPlayerStatsVal();
                var _player = new Player();
                _player.Teams = new List<Team>();
                var teamm = new Team();




                _player.Rating = ranking[_random.Next(ranking.Length)];

                foreach (var item in jarray.Cast<JObject>())
                {
                    player.PlayerValAPIId = (int)item.GetValue("id");
                    player.Kills = _random.Next(30, 301);
                    Console.WriteLine("Kills ------------->" + player.Kills);
                    player.Deaths = _random.Next(30, 300);
                    Console.WriteLine("Deaths ------------->" + player.Deaths);
                    player.Assists = _random.Next(1, 11); ;
                    player.ADR = _random.NextDouble();
                    player.HeadShots = Math.Round(_random.NextDouble() * 100, 2);
                    player.KD_Diff = _random.NextDouble();
                    player.PlayerName = (string)item.GetValue("name");

                    //_player.Age = (int?)item.GetValue("age");
                    _player.Age = item.GetValue("age") == null ? 20 : item.GetValue("age").Value<int>();
                    _player.Nationality = (string)item.GetValue("nationality");
                    ViewBag.NacionalityImg = "/images/Flags/4x3/" + _player.Nationality + ".svg";
                    //_player.Image = (string)item.GetValue("image_url");
                    _player.Image = item.GetValue("image_url").ToString() == "" ? "/images/default-profile-icon-24.jpg" : item.GetValue("image_url").Value<string>();
                    _player.Rating = ranking[_random.Next(ranking.Length)];
                    _player.PlayerAPIId = player.PlayerValAPIId;
                    _player.Name = player.PlayerName;

                    var current_team = (JObject)item.GetValue("current_team");

                    var current_team_id = (int)current_team.GetValue("id");
                    teamm.TeamAPIID = (int)current_team.GetValue("id");
                    //if(teamm.TeamAPIID == _player.)
                    teamm.Name = current_team.GetValue("name").ToString() == "" ? "undefined" : current_team.GetValue("name").Value<string>();
                    teamm.Image = current_team.GetValue("image_url").ToString() == "" ? "/images/logo1.jpg" : current_team.GetValue("image_url").Value<string>();
                    teamm.CoachName = coachNames[_random.Next(coachNames.Length)];
                    teamm.WorldRank = 1;
                    teamm.Winnings = 1;
                    teamm.Losses = 1;
                    teamm.Game = GameType.CSGO;

                    var pastTeam1 = teamm;
                    var pastTeam2 = teamsList[_random.Next(teamsList.Count())];
                    var pastTeam3 = teamsList[_random.Next(teamsList.Count())];
                    ViewBag.pastTeam1 = pastTeam1;
                    ViewBag.pastTeam2 = pastTeam2;
                    ViewBag.pastTeam3 = pastTeam3;
                    _player.Teams.Add(teamm);
                    _player.Teams.Add(pastTeam2);
                    _player.Teams.Add(pastTeam3);

                    _context.MatchPlayerStatsVal.Add(player);
                }


                double KdRatio = (double)player.Kills / (double)player.Deaths;
                double roundKdRatio = Math.Round(KdRatio, 2);
                int maps = _random.Next(1, 8);
                ViewBag.roundKdRatio = roundKdRatio;
                ViewBag.maps = maps;
                ViewBag.player = player;
                ViewBag._player = _player;

                id = id;
                ViewBag.id = id;
                return View("PlayerAndStats");
            }
            else
            {
                var ranking = new[] { 0.68F, 0.94F, 1.42F, 1.08F, 1.09F, 1.23F, 0.78F, 0.89F, 0.97F, 0.72F,
                    0.82F, 0.62F, 1.45F, 1.11F, 1.37F, 1.27F, 1.05F, 1.07F, 1.16F, 1.29F, 1.15F, 0.97F, 0.83F,
                    1.36F, 1.10F, 1.07F, 1.19F, 0.77F, 0.90F, 1.14F, 1.52F, 1.54F, 0.58F }; //de 0.58 a 1.54

                //var teamsList = new[] { "G2", "Heroic", "Natus Vincere", "Liquid", "Vitality", "Outsiders", "Faze",
                //    "Complexity", "fnatic", "Cloud9", "Spirit", "Astralis", "MOUZ", "FURIA ", "BIG", "Ninjas in Pyjamas",
                //    "IHC", "Eternal Fire", "ENCE", "FORZE", "Bad News Eagles", "MIBR", "Movistar Riders", "9INE", "paiN",
                //    "GamerLegion", "Aurora", "Rare Atom", "Grayhound", "NRG", "SAW", "Avangar", "Spirit", "Nexus", "Grayhound",
                //    "TYLOO", "Renegates", "SINNERS", "HellRaisers", "Club Brugge", "North", "Dignitas", "Luminosity", "TeamOne",
                //    "Sprout", "Cheifs", "SK", "Endpoint", "GODSENT", "Envy", "HAVU", "Envy", "Gambit" };


                //Base url for requests
                var _requestLink = "https://api.pandascore.co/";

                //Filter to select from which pool to fetch the data (upcoming, running or finished/ended)
                var _jsonFilter = filter + "?";
                var _filterID = "filter[id]="; //+ id.ToString();

                //THIS SHOULD BE A CLIENT SECRET
                var _token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
                var jsonSort = "&sort=";
                var jsonPage = page;
                var jsonPerPage = "&per_page=50";

                //Request processing with RestSharp
                var _fullRequest = _requestLink + game + "/teams?" + _filterID + jsonSort + jsonPage + jsonPerPage + _token;
                var _client = new RestClient(_fullRequest);
                var _request = new RestRequest("", Method.Get);
                _request.AddHeader("accept", "application/json");
                var _json = _client.Execute(_request).Content;
                var _jarray = JArray.Parse(_json);

                var teamsList = new List<Team>();
                var coachNames = new[] { "Rui", "Nuno", "Miguel", "André", "João", "Guilherme" };


                foreach (var _team in _jarray.Cast<JObject>())
                {

                    var team = new Team();
                    team.TeamAPIID = (int)_team.GetValue("id");
                    //if(teamm.TeamAPIID == _player.)
                    team.Name = _team.GetValue("name").ToString() == "" ? "undefined" : _team.GetValue("name").Value<string>();
                    team.Image = _team.GetValue("image_url").ToString() == "" ? "/images/missing.png" : _team.GetValue("image_url").Value<string>();
                    team.CoachName = coachNames[_random.Next(coachNames.Length)];
                    team.WorldRank = 1;
                    team.Winnings = 1;
                    team.Losses = 1;
                    team.Game = GameType.CSGO;
                    teamsList.Add(team);

                }


                //Base url for requests
                var requestLink = "https://api.pandascore.co/";

                //Filter to select from which pool to fetch the data (upcoming, running or finished/ended)
                var jsonFilter = filter + "?";
                var filterID = "filter[id]=" + id.ToString();

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




                _player.Rating = ranking[_random.Next(ranking.Length)];

                foreach (var item in jarray.Cast<JObject>())
                {
                    player.PlayerCSAPIId = (int)item.GetValue("id");
                    player.Kills = _random.Next(30, 301);
                    Console.WriteLine("Kills ------------->" + player.Kills);
                    player.Deaths = _random.Next(30, 300);
                    Console.WriteLine("Deaths ------------->" + player.Deaths);
                    player.Assists = _random.Next(1, 11); ;
                    player.FlashAssist = _random.Next(1, 6); ;
                    player.ADR = _random.NextDouble();
                    player.HeadShots = Math.Round(_random.NextDouble() * 100, 2);
                    player.KD_Diff = _random.NextDouble();
                    player.PlayerName = (string)item.GetValue("name");

                    //_player.Age = (int?)item.GetValue("age");
                    _player.Age = item.GetValue("age") == null ? 20 : item.GetValue("age").Value<int>();
                    _player.Nationality = (string)item.GetValue("nationality");
                    ViewBag.NacionalityImg = "/images/Flags/4x3/" + _player.Nationality + ".svg";
                    //_player.Image = (string)item.GetValue("image_url");
                    _player.Image = item.GetValue("image_url").ToString() == "" ? "/images/default-profile-icon-24.jpg" : item.GetValue("image_url").Value<string>();
                    _player.Rating= ranking[_random.Next(ranking.Length)];
                    _player.PlayerAPIId = player.PlayerCSAPIId;
                    _player.Name = player.PlayerName;

                    var current_team = (JObject)item.GetValue("current_team");
                    
                    var current_team_id = (int)current_team.GetValue("id");
                    teamm.TeamAPIID = (int)current_team.GetValue("id");
                    //if(teamm.TeamAPIID == _player.)
                    teamm.Name = current_team.GetValue("name").ToString() == "" ? "undefined" : current_team.GetValue("name").Value<string>();
                    teamm.Image = current_team.GetValue("image_url").ToString() == "" ? "/images/logo1.jpg" : current_team.GetValue("image_url").Value<string>();
                    teamm.CoachName = coachNames[_random.Next(coachNames.Length)];
                    teamm.WorldRank = 1;
                    teamm.Winnings = 1;
                    teamm.Losses = 1;
                    teamm.Game = GameType.CSGO;

                    var pastTeam1 = teamm;
                    var pastTeam2 = teamsList[_random.Next(teamsList.Count())];
                    var pastTeam3 = teamsList[_random.Next(teamsList.Count())];
                    ViewBag.pastTeam1 = pastTeam1;
                    ViewBag.pastTeam2 = pastTeam2;
                    ViewBag.pastTeam3 = pastTeam3;
                    _player.Teams.Add(teamm);
                    _player.Teams.Add(pastTeam2);
                    _player.Teams.Add(pastTeam3);

                    _context.MatchPlayerStatsCS.Add(player);
            }
                

                double KdRatio = (double)player.Kills / (double)player.Deaths;
                double roundKdRatio = Math.Round(KdRatio, 2);
                int maps = _random.Next(1, 8);
                ViewBag.roundKdRatio = roundKdRatio;
                ViewBag.maps = maps;
                ViewBag.player = player;
                ViewBag._player = _player;
                
                id = id;
                ViewBag.id = id;
                return View("PlayerAndStats");
            }
            return null;
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
                var jsonPerPage = "&per_page=50";
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

        public async Task<ActionResult> getCSGOMatchesAsync(string time = "past")
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
            List<Player>? playerList = new();



            foreach (var item in jarrayMatches.Cast<JObject>())
            {
                var status = item.GetValue("status");

                if (!status.ToString().Equals("canceled"))
                {
                    var opponentArray = (JArray)item.GetValue("opponents");
                    var ma = new MatchCS();
                    ma.PlayerStatsList = new List<MatchPlayerStatsCS>();
                    ma.TeamsList = new List<MatchTeamsCS>();
                    var matches = new MatchesCS();
                    matches.MatchesList = new List<MatchCS>();

                    matches.MatchesAPIID = (int)item.GetValue("id");
                    ma.MatchesCSAPIId = matches.MatchesAPIID;
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
                        team.Name = teamName.ToString() == "" ? "undefined" : teamName.Value<string>();
                        team.Image = teamImage.ToString() == "" ? "/images/logo1.jpg" : teamImage.Value<string>();
                        team.Game = GameType.CSGO;
                        team.CoachName = coachNames[_random.Next(coachNames.Length)]; ;
                        team.WorldRank = 1;
                        team.Winnings = 1;
                        team.Losses = 1;
                        //_context.Team.Add(team);
                        //_context.Team.Add(team);

                        matches.WinnerTeamAPIId = team.TeamAPIID;
                        matches.WinnerTeamName = team.Name;
                        EventCS evt = new EventCS();
                        var events = (JObject)item.GetValue("tournament");

                        matches.EventName = (string)events.GetValue("name");
                        matches.Event = evt;
                        matches.EventAPIID = (int)item.GetValue("tournament_id");
                        evt.BeginAt = new DateTime();
                        evt.EndAt = new DateTime();
                        evt.EventName = matches.EventName;
                        evt.TimeType = TimeType.Running;
                        evt.Finished = false;
                        evt.EventImage = "";
                        evt.EventLink = "";
                        evt.LeagueName = "";
                        evt.PrizePool = "";
                        evt.Tier = ' ';
                        evt.WinnerTeamAPIID = 1;
                        evt.WinnerTeamName = "";

                        var fullApiPath = "https://api.pandascore.co/csgo/teams?filter[id]=" + "133022" + "&sort=&page=1&per_page=50&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
                        var client = new RestClient(fullApiPath);
                        var request = new RestRequest("", Method.Get);
                        request.AddHeader("accept", "application/json");
                        var response = client.Execute(request).Content;
                        var teamsArray = JArray.Parse(response);


                        foreach (JObject t in jarrayTeams.Cast<JObject>())
                        {
                            var count = 0;
                            foreach (JObject p in jarrayPlayers.Cast<JObject>())
                            {
                                var matchPlayer = new MatchPlayerStatsCS();
                                matchPlayer.MatchCS = ma;
                                matchPlayer.MatchCSAPIID = (int)item.GetValue("id");
                                matchPlayer.PlayerCSAPIId = (int)p.GetValue("id");
                                matchPlayer.Kills = _random.Next(1, 31);
                                matchPlayer.Deaths = _random.Next(1, 21);
                                matchPlayer.Assists = _random.Next(1, 11); ;
                                matchPlayer.FlashAssist = _random.Next(1, 6); ;
                                matchPlayer.ADR = _random.NextDouble();
                                matchPlayer.HeadShots = _random.NextDouble() * 100;
                                matchPlayer.KD_Diff = _random.NextDouble();
                                matchPlayer.PlayerName = (string)p.GetValue("name");
                                //if(playerList.Count != 0)
                                //{
                                //    for (int i = 0; i < playerList.Count; i++)
                                //    {
                                //        if (count == i)
                                //        {
                                //            matchPlayer.PlayerCS=playerList[i];
                                //            count++;
                                //        }
                                //    }
                                //}
                                playerStatsList.Add(matchPlayer);
                                ma.PlayerStatsList.Add(matchPlayer);
                                //playerStatsList.Add(player);
                                _context.MatchPlayerStatsCS.Add(matchPlayer);
                                foreach (var _team in teamsArray.Cast<JObject>())
                                {
                                    var players = (JArray)_team.GetValue("players");
                                    foreach (var _player in players.Cast<JObject>())
                                    {
                                        //inicializar cada player
                                        var player = new Player();
                                        player.Teams = new List<Team>();
                                        player.PlayerAPIId = (int)_player.GetValue("id");
                                        player.Name = (string)_player.GetValue("name");
                                        var a = _player.GetValue("age").ToString();
                                        player.Age = _player.GetValue("age") == null ? 20 : _player.GetValue("age").Value<int>();
                                        player.Rating = ranking[_random.Next(ranking.Length)];
                                        team.Name = teamName.ToString() == "" ? "undefined" : teamName.Value<string>();
                                        player.Flag = (string)item.GetValue("nationality") == null ? "undefined" : item.GetValue("nationality").Value<string>();
                                        player.Game = GameType.CSGO;
                                        player.Image = "";
                                        player.Nationality = (string)item.GetValue("nationality") == null ? "undefined" : item.GetValue("nationality").Value<string>();
                                        player.Teams.Add(team);
                                        playerList.Add(player);
                                        if (matchPlayer.PlayerCS == null)
                                        {
                                            matchPlayer.PlayerCS = player;
                                        }
                                        //for (int i = 0; i < _context.Player.Count; i++) {
                                        _context.Player.Add(player);
                                        
                                        //}
                                    }

                                }
                            }
                        }


                        
                        var matchTeam = new MatchTeamsCS();
                        matchTeam.MatchCSAPIID = (int)item.GetValue("id");
                        matchTeam.TeamCSAPIId = (int)team.TeamAPIID;
                        matchTeam.TeamCS = team;
                        matchTeam.Name = (string)opponent.GetValue("name");
                        matchTeam.Location = (string?)opponent.GetValue("location");
                        matchTeam.Image = (string)opponent.GetValue("image_url") == null ? "undefined" : opponent.GetValue("image_url").Value<string>();

                        ma.TeamsList.Add(matchTeam);

                        JArray games = (JArray)item["games"];
                        foreach (var game in games.Cast<JObject>())
                        {
                            var winnerTeam = new Team();
                            var winner = game.GetValue("winner");
                            winnerTeam.TeamAPIID = winner.Value<int>("id");
                            if (winnerTeam.TeamAPIID == team.TeamAPIID)
                            {
                                winnerTeam.Name = team.Name;
                                winnerTeam.Image = team.Image;
                                winnerTeam.CoachName = team.CoachName;
                                winnerTeam.Game = team.Game;
                                winnerTeam.WorldRank = 1;
                                winnerTeam.Winnings = team.Winnings;
                                winnerTeam.Losses = team.Losses;
                                ma.WinnerTeamName = winnerTeam.Name;
                            }
                            //_context.Team.Add(winnerTeam);
                            ma.WinnerTeam = winnerTeam;
                            ma.WinnerTeamAPIId = winnerTeam.TeamAPIID;
                            ma.MatchCSAPIID = (int)game.GetValue("id");
                        }
                        matchCsList.Add(ma);
                        _context.MatchCS.Add(ma);
                        teamsList.Add(matchTeam);
                        _context.MatchTeamsCS.Add(matchTeam);


                        

                    }
                    matches.MatchesList.Add(ma);
                    _context.MatchesCS.Add(matches);
                    await _context.SaveChangesAsync();
                }
            }
            foreach (MatchCS mpCS in matchCsList)
            {
                Console.WriteLine("matchCsList -->" + mpCS);
            }
            foreach (MatchPlayerStatsCS mpCS in playerStatsList)
            {
                Console.WriteLine("playerStatsList -->" + mpCS);
            }
            foreach (MatchTeamsCS mpCS in teamsList)
            {
                Console.WriteLine("teamsList -->" + mpCS);
            }
            ViewBag.playerStatsList = playerStatsList;
            ViewBag.matchPlayerStatsCS = playerStatsList;
            ViewBag.teamsList = teamsList;
            ViewBag.matchCsList = matchCsList;
            ViewBag.player = playerList;
           
            return View("PlayerAndStats");
        }
    }
}