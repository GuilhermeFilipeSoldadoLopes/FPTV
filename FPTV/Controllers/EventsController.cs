using AngleSharp.Text;
using FPTV.Data;
using FPTV.Models.BLL.Events;
using FPTV.Models.EventsModels;
using FPTV.Models.MatchesModels;
using FPTV.Models.StatisticsModels;
using FPTV.Models.UserModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;

namespace FPTV.Controllers
{
    public class EventsController : Controller
	{
        private readonly FPTVContext _context;

        public EventsController(FPTVContext context)
        {
            _context = context;
        }

        // GET: EventsController
        public ActionResult Index(string sort = "&sort=-begin_at", string filter = "running", string search = "", string page = "&page=1", string game = "csgo")
		{
			ViewData["game"] = game;
			ViewBag.page = "Events";
			search ??= "";
            //Request processing with RestSharp
            var jsonFilter = filter + "?";
            var jsonSort = sort == "&sort=name" ? "&sort=-begin_at" : sort;
            var jsonPage = page;
            var jsonPerPage = "&per_page=10";
            var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";
            var requestLink = "https://api.pandascore.co/" + game + "/tournaments/";
            Console.WriteLine(search);

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
            IList events = game == "csgo" 
                ? GetEventsCS(jarray, filter, sort, search) 
                : GetEventsVal(jarray, filter, sort, search);

			
			return View(events);
        }

        private List<EventCS> GetEventsCS(JArray jarray, string filter, string sort, string search)
        { 
            List<EventCS> events = new();
 
            foreach (JObject e in jarray.Cast<JObject>())
            {
                //Set up values from api
                List<Team> teamList = new();
                
                EventCS ev = new();
                var eventAPIID = e.GetValue("id");
                var nameStage = e.GetValue("name");
                var beginAt = e.GetValue("begin_at");
                var league = e.GetValue("league");
                var teams = e.GetValue("teams");
                var prizePool = e.GetValue("prizepool");
                var winnerTeamId = e.GetValue("winner_id");
                var matches = e.GetValue("matches");
                var tier = e.GetValue("tier");

                //Handling for null values
                ev.EventAPIID = eventAPIID.ToString() == null ? -1 : eventAPIID.Value<int>();
                ev.BeginAt = beginAt.ToString() == "" ? null : beginAt.Value<DateTime>();
                ev.EventName = league.ToString() == "" ? null : league.Value<string>("name");
                ev.LeagueName = nameStage.ToString() == "" ? null : nameStage.Value<string>();
                ev.PrizePool = prizePool.ToString() == "" ? "0" : new string(prizePool.Value<string>().Where(char.IsDigit).ToArray());
                ev.WinnerTeamAPIID = winnerTeamId.ToString() == "" ? -1 : winnerTeamId.Value<int>();
                ev.EventImage = league.Value<string>("image_url") ?? "/images/missing.png";
                ev.MatchesCSAPIID = matches.ToString() == "" ? -1 : matches[0].Value<int>("id");
				ev.EventLink = league.ToString() == "" ? null : league.Value<string>("url");
				ev.Tier = tier.ToString() == "" ? '-' : tier.Value<char>();

                switch (filter)
                {
                    case "running":
                        ev.TimeType = TimeType.Running;
                        ev.Finished = false;
                        break;
                    case "past":
                        ev.TimeType = TimeType.Past;
                        ev.Finished = true;
                        break;
                    case "upcoming":
                        ev.TimeType = TimeType.Upcoming;
                        ev.Finished = false;
                        break;
                }

                if (teams != null)
                {
                    foreach (JObject o in teams)
                    {
                        var teamNameValue = o.GetValue("name");
                        var teamIdValue = o.GetValue("id");
                        var teamId = teamIdValue.ToString() == "" ? -1 : teamIdValue.Value<int>();
                        var teamName = teamNameValue.ToString() == "" ? null : teamNameValue.Value<string>();
                        var teamImage = o.GetValue("image_url").Value<string>();
                        var team = new Team();
                        team.Name = teamName;
                        team.TeamAPIID = teamId;
                        team.Image = teamImage ?? "/images/missing.png";
                        team.Game = GameType.CSGO;
                        teamList.Add(team);
                    }
                }

                //Filling remaining fields
                ev.TeamsList = teamList;
                var winnerTeam = teamList.FirstOrDefault(t => t.TeamAPIID == ev.WinnerTeamAPIID);
                ev.WinnerTeamName = winnerTeam == default ? "-" : winnerTeam.Name;
                ev.WinnerTeamAPIID = winnerTeam == default ? -1 : winnerTeam.TeamAPIID;
                events.Add(ev);
            }

            if (filter == "past")
            {
                var dbEvents = _context.EventCS.ToList();
                
                foreach (var e in events)
                {

                    var dbev = dbEvents.FirstOrDefault(ev => ev.EventAPIID == e.EventAPIID);
                    if (dbev == null)
                    {
                        _context.EventCS.Add(e);
                    }
                    

                }
                _context.SaveChanges();

                events = _context.EventCS.ToList();
                if (sort == "&sort=begin_at")
                {
                    events.Sort((x, y) => Nullable.Compare(x.BeginAt, y.BeginAt));
                }
                if (sort == "&sort=-begin_at")
                {
                    events.Sort((x, y) => Nullable.Compare(y.BeginAt, x.BeginAt));
                }
            }

            switch (sort)
            {
                case "&sort=prizepool":
                    events.Sort((x, y) =>
                    {
                        if (int.TryParse(y.PrizePool, out int yp) && int.TryParse(x.PrizePool, out int xp))
                        {
                            return yp.CompareTo(xp);
                        }
                        return 1;
                    });
                    break;
                case "&sort=name":
                    events.Sort((x, y) => x.EventName.CompareTo(y.EventName));
                    break;
                case "&sort=-tier":
                    events.Sort((x, y) => x.Tier.ToString().CompareTo(y.Tier.ToString()));
                    break;
            }

            if (search != "")
            {
                events = events.Where(e => e.EventName.ToLower().Contains(search.ToLower())).ToList();
            }


            ViewBag.filter = filter;
            ViewBag.sort = sort;
            return events;
        }

        private List<EventVal> GetEventsVal(JArray jarray, string filter, string sort, string search)
        {
            List<EventVal> events = new();

            foreach (JObject e in jarray.Cast<JObject>())
            {
                //Set up values from api
                List<Team> teamList = new();

                EventVal ev = new();
                var eventAPIID = e.GetValue("id");
                var nameStage = e.GetValue("name");
                var beginAt = e.GetValue("begin_at");
                var league = e.GetValue("league");
                var teams = e.GetValue("teams");
                var prizePool = e.GetValue("prizepool");
                var winnerTeamId = e.GetValue("winner_id");
                var matches = e.GetValue("matches");
                var tier = e.GetValue("tier");

                //Handling for null values
                ev.EventAPIID = eventAPIID.ToString() == null ? -1 : eventAPIID.Value<int>();
                ev.BeginAt = beginAt.ToString() == "" ? null : beginAt.Value<DateTime>();
                ev.EventName = league.ToString() == "" ? null : league.Value<string>("name");
                ev.LeagueName = nameStage.ToString() == "" ? null : nameStage.Value<string>();
                ev.PrizePool = prizePool.ToString() == "" ? "0" : new string(prizePool.Value<string>().Where(char.IsDigit).ToArray());
                ev.WinnerTeamAPIID = winnerTeamId.ToString() == "" ? -1 : winnerTeamId.Value<int>();
                ev.EventImage = league.Value<string>("image_url") ?? "/images/missing.png";
                ev.MatchesValAPIID = matches.ToString() == "" ? -1 : matches[0].Value<int>("id");
				ev.EventLink = league.ToString() == "" ? null : league.Value<string>("url");
				ev.Tier = tier.ToString() == "" ? '-' : tier.Value<char>();


                switch (filter)
                {
                    case "running":
                        ev.TimeType = TimeType.Running; break;
                    case "past":
                        ev.TimeType = TimeType.Past; break;
                    case "upcoming":
                        ev.TimeType = TimeType.Upcoming; break;

                }


                if (teams != null)
                {
                    foreach (JObject o in teams)
                    {
                        var teamNameValue = o.GetValue("name");
                        var teamIdValue = o.GetValue("id");
                        var teamId = teamIdValue.ToString() == "" ? -1 : teamIdValue.Value<int>();
                        var teamName = teamNameValue.ToString() == "" ? null : teamNameValue.Value<string>();
                        var teamImage = o.GetValue("image_url").Value<string>();
                        var team = new Team();
                        team.Name = teamName;
                        team.TeamAPIID = teamId;
                        team.Game = GameType.Valorant;
                        team.Image = teamImage ?? "/images/missing.png";
                        teamList.Add(team);
                    }
                }

                //Filling remaining fields
                ev.TeamsList = teamList;
                var winnerTeam = teamList.FirstOrDefault(t => t.TeamAPIID == ev.WinnerTeamAPIID);
                ev.WinnerTeamName = winnerTeam == default ? "-" : winnerTeam.Name;
                ev.WinnerTeamAPIID = winnerTeam == default ? -1 : winnerTeam.TeamAPIID;
                events.Add(ev);
            }

            if (filter == "past")
            {
                var dbEvents = _context.EventVal.ToList();
                foreach (var e in events)
				{

					var dbev = dbEvents.FirstOrDefault(ev => ev.EventAPIID == e.EventAPIID);
					if (dbev == null)
					{
                        _context.EventVal.Add(e);
                    }
                    
                    
                }
                _context.SaveChanges();

                events = _context.EventVal.ToList();
                if (sort == "&sort=begin_at")
                {
                    events.Sort((x, y) => Nullable.Compare(y.BeginAt, x.BeginAt));
                }
                if (sort == "&sort=-begin_at")
                {
                    events.Sort((x, y) => Nullable.Compare(y.BeginAt, x.BeginAt));
                }
            }

            switch (sort)
            {
                case "&sort=prizepool":
                    events.Sort((x, y) =>
                    {
                        if (int.TryParse(y.PrizePool, out int yp) && int.TryParse(x.PrizePool, out int xp))
                        {
                            return yp.CompareTo(xp);
                        }
                        return -1;
                    });
                    break;
                case "&sort=name":
                    events.Sort((x, y) => x.EventName.CompareTo(y.EventName));
                    break;
                case "&sort=-tier":
                    events.Sort((x, y) => x.Tier.ToString().CompareTo(y.Tier.ToString()));
                    break;
            }

            if (search != "")
            {
                events = events.Where(e => e.EventName.Contains(search)).ToList();
            }

           
			ViewBag.filter = filter;
            ViewBag.sort = sort;
            return events;
        }

        // GET: EventsController/Details/5
        public ActionResult Details(int id, string filter = "running", string game = "csgo")
		{
			ViewData["game"] = game;

			switch (game)
			{
				case "csgo":
					SendEventInfoCS(GetEventCS(id, filter));
					break;
				case "valorant":
					SendEventInfoVal(GetEventVal(id, filter));
					break;
			}

            return View();
        }

       

		private EventVal GetEventVal(int id, string filter)
		{
			if (filter == "past")
			{
				var dbev = _context.EventVal.Include(ev => ev.TeamsList).FirstOrDefault(ev => ev.EventAPIID == id);
				if (dbev != default)
				{
					return dbev;
				}
			}

			EventVal ev = new();
			//Base url for requests
			var requestLink = "https://api.pandascore.co/";

			//Filter to select from which pool to fetch the data (upcoming, running or finished/ended)
			var jsonFilter = filter + "?";
			var filterID = "filter[id]=" + id.ToString();

			//THIS SHOULD BE A CLIENT SECRET
			var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";

			//Request processing with RestSharp
			var fullRequest = requestLink + "valorant" + "/tournaments/" + jsonFilter + filterID + token;
			var client = new RestClient(fullRequest);
			var request = new RestRequest("", Method.Get);
			request.AddHeader("accept", "application/json");
			var json = client.Execute(request).Content;

			var jarray = JArray.Parse(json);

			foreach (JObject e in jarray.Cast<JObject>())
			{
				//Set up values from api
				List<Team> teamList = new();

				var eventAPIID = e.GetValue("id");
				var name = e.GetValue("name");
				var nameStage = e.GetValue("name");
				var beginAt = e.GetValue("begin_at");
				var endAt = e.GetValue("end_at");
				var timeType = TimeType.Running;
				var tier = e.GetValue("tier");
				var league = e.GetValue("league");
				var teams = e.GetValue("teams");
				var prizePool = e.GetValue("prizepool");
				var winnerTeamId = e.GetValue("winner_id");
				var matchesJ = e.GetValue("matches");

				//Handling for null values
				ev.EventAPIID = eventAPIID.ToString() == null ? -1 : eventAPIID.Value<int>();
				ev.BeginAt = beginAt.ToString() == "" ? null : beginAt.Value<DateTime>();
				ev.EndAt = endAt.ToString() == "" ? null : endAt.Value<DateTime>();
				ev.TimeType = timeType;
				ev.Tier = tier.ToString() == "" ? null : tier.Value<char>();
				ev.EventLink = league.ToString() == "" ? null : league.Value<string>("url");
				ev.Finished = false;
				ev.EventName = league.ToString() == "" ? null : league.Value<string>("name");
				ev.LeagueName = nameStage.ToString() == "" ? null : nameStage.Value<string>();
				ev.PrizePool = prizePool.ToString() == "" ? "0" : new string(prizePool.Value<string>().Where(char.IsDigit).ToArray());
				ev.WinnerTeamAPIID = winnerTeamId.ToString() == "" ? -1 : winnerTeamId.Value<int>();
				ev.EventImage = league.Value<string>("image_url") ?? "/images/missing.png";


				if (teams != null)
				{
					foreach (JObject o in teams.Cast<JObject>())
					{
						var teamNameValue = o.GetValue("name");
						var teamIdValue = o.GetValue("id");
						var teamId = teamIdValue.ToString() == "" ? -1 : teamIdValue.Value<int>();
						var teamName = teamNameValue.ToString() == "" ? null : teamNameValue.Value<string>();
						var teamImage = o.GetValue("image_url").Value<string>();
						var team = new Team();
						team.Name = teamName;
						team.TeamAPIID = teamId;
						team.Game = GameType.Valorant;
                        team.Image = teamImage ?? "/images/missing.png";
                        teamList.Add(team);

					}
					ev.TeamsList = teamList;
				}
			}
			return ev;
		}

		private EventCS GetEventCS(int id, string filter)
		{
			if (filter == "past")
			{
				var dbev = _context.EventCS.Include(ev => ev.TeamsList).FirstOrDefault(ev => ev.EventAPIID == id);
				if (dbev != default)
				{
					return dbev;
				}
			}

			EventCS ev = new();
			//Base url for requests
			var requestLink = "https://api.pandascore.co/";

			//Filter to select from which pool to fetch the data (upcoming, running or finished/ended)
			var jsonFilter = filter + "?";
			var filterID = "filter[id]=" + id.ToString();

			//THIS SHOULD BE A CLIENT SECRET
			var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";

			//Request processing with RestSharp
			var fullRequest = requestLink + "csgo" + "/tournaments/" + jsonFilter + filterID + token;
			var client = new RestClient(fullRequest);
			var request = new RestRequest("", Method.Get);
			request.AddHeader("accept", "application/json");
			var json = client.Execute(request).Content;

			var jarray = JArray.Parse(json);

			foreach (JObject e in jarray.Cast<JObject>())
			{
				//Set up values from api
				List<Team> teamList = new();

				var eventAPIID = e.GetValue("id");
				var name = e.GetValue("name");
				var nameStage = e.GetValue("name");
				var beginAt = e.GetValue("begin_at");
				var endAt = e.GetValue("end_at");
				var timeType = TimeType.Running;
				var tier = e.GetValue("tier");
				var league = e.GetValue("league");
				var teams = e.GetValue("teams");
				var prizePool = e.GetValue("prizepool");
				var winnerTeamId = e.GetValue("winner_id");
				var matchesJ = e.GetValue("matches");

				//Handling for null values
				ev.EventAPIID = eventAPIID.ToString() == null ? -1 : eventAPIID.Value<int>();
				ev.BeginAt = beginAt.ToString() == "" ? null : beginAt.Value<DateTime>();
				ev.EndAt = endAt.ToString() == "" ? null : endAt.Value<DateTime>();
				ev.TimeType = timeType;
				ev.Tier = tier.ToString() == "" ? null : tier.Value<char>();
				ev.EventLink = league.ToString() == "" ? null : league.Value<string>("url");
				ev.Finished = false;
				ev.EventName = league.ToString() == "" ? null : league.Value<string>("name");
				ev.LeagueName = nameStage.ToString() == "" ? null : nameStage.Value<string>();
				ev.PrizePool = prizePool.ToString() == "" ? "0" : new string(prizePool.Value<string>().Where(char.IsDigit).ToArray());
				ev.WinnerTeamAPIID = winnerTeamId.ToString() == "" ? -1 : winnerTeamId.Value<int>();
				ev.EventImage = league.Value<string>("image_url") ?? "/images/missing.png";


				if (teams != null)
				{
					foreach (JObject o in teams.Cast<JObject>())
					{
						var teamNameValue = o.GetValue("name");
						var teamIdValue = o.GetValue("id");
						var teamId = teamIdValue.ToString() == "" ? -1 : teamIdValue.Value<int>();
						var teamName = teamNameValue.ToString() == "" ? null : teamNameValue.Value<string>();
						var teamImage = o.GetValue("image_url").Value<string>();
						var team = new Team();
						team.Name = teamName;
						team.TeamAPIID = teamId;
						team.Game = GameType.Valorant;
						
                        team.Image = teamImage ?? "/images/missing.png";
                        
                        teamList.Add(team);

					}
					ev.TeamsList = teamList;
				}
			}
			return ev;
		}

		private void SendEventInfoVal(EventVal ev)
		{
			List<MatchesVal> pMatches = new();
			List<MatchesVal> uMatches = new();
			List<MatchesVal> rMatches = new();

			//Base url for requests
			var requestLink = "https://api.pandascore.co/";
			var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";

			var eventID = "filter[tournament_id]=" + ev.EventAPIID.ToString();
			var pageSort = "&sort=&page=1&per_page=100";
			var fullPRequest = requestLink + "valorant" + "/matches/" + "past?" + eventID + pageSort + token;
			var fullRRequest = requestLink + "valorant" + "/matches/" + "running?" + eventID + pageSort + token;
			var fullURequest = requestLink + "valorant" + "/matches/" + "upcoming?" + eventID + pageSort + token;

			Console.WriteLine(fullPRequest);

			var pClient = new RestClient(fullPRequest);
			var rClient = new RestClient(fullRRequest);
			var uClient = new RestClient(fullURequest);

			var pReq = new RestRequest("", Method.Get);
			var rReq = new RestRequest("", Method.Get);
			var uReq = new RestRequest("", Method.Get);

			pReq.AddHeader("accept", "application/json");
			rReq.AddHeader("accept", "application/json");
			uReq.AddHeader("accept", "application/json");

			var pastMatches = pClient.Execute(pReq).Content;
			var runningMatches = rClient.Execute(rReq).Content;
			var upcomingMatches = uClient.Execute(uReq).Content;

			if (pastMatches != null)
			{

				foreach (JObject o in JArray.Parse(pastMatches).Cast<JObject>())
				{
					List<Score> scores = new();
					var m = new MatchesVal();

					m.MatchesAPIID = o.GetValue("id").Value<int>();
					m.BeginAt = o.GetValue("begin_at").ToString() == "" ? null : o.GetValue("begin_at").Value<DateTime>();
					m.TimeType = TimeType.Past;
					m.NumberOfGames = o.GetValue("number_of_games").Value<int>();

					var results = o.GetValue("results");

					foreach (JObject r in results.Cast<JObject>())
					{
						var s = new Score();
						s.Team = ev.TeamsList.FirstOrDefault(t => t.TeamAPIID == r.Value<int>("team_id"));

						s.TeamName = s.Team.Name;
						s.TeamScore = r.Value<int>("score");
						scores.Add(s);
					}

					m.Scores = scores;
					pMatches.Add(m);
				}
			}

			if (runningMatches != null)
			{

				foreach (JObject o in JArray.Parse(runningMatches).Cast<JObject>())
				{
					List<Score> scores = new();
					var m = new MatchesVal();

					m.MatchesAPIID = o.GetValue("id").Value<int>();
					m.BeginAt = o.GetValue("begin_at").ToString() == "" ? null : o.GetValue("begin_at").Value<DateTime>();
					m.TimeType = TimeType.Running;
					m.NumberOfGames = o.GetValue("number_of_games").Value<int>();

					var results = o.GetValue("results");

					foreach (JObject r in results.Cast<JObject>())
					{
						var s = new Score();
						s.Team = ev.TeamsList.FirstOrDefault(t => t.TeamAPIID == r.Value<int>("team_id"));
						s.TeamName = s.Team.Name;
						s.TeamScore = r.Value<int>("score");
						scores.Add(s);
					}

					m.Scores = scores;
					rMatches.Add(m);
				}
			}

			if (upcomingMatches != null)
			{

				foreach (JObject o in JArray.Parse(upcomingMatches).Cast<JObject>())
				{
					var m = new MatchesVal();
					List<Score> scores = new();

					m.MatchesAPIID = o.GetValue("id").Value<int>();
					m.BeginAt = o.GetValue("begin_at").ToString() == "" ? null : o.GetValue("begin_at").Value<DateTime>();
					m.TimeType = TimeType.Upcoming;
					m.NumberOfGames = o.GetValue("number_of_games").Value<int>();

					var results = o.GetValue("results");


					foreach (JObject r in results.Cast<JObject>())
					{
						var s = new Score();
						s.Team = ev.TeamsList.FirstOrDefault(t => t.TeamAPIID == r.Value<int>("team_id"));
						if (s.Team != default)
						{

							s.TeamName = s.Team.Name;
							s.TeamScore = r.Value<int>("score");

						}
						scores.Add(s);
					}

					m.Scores = scores;
					uMatches.Add(m);


				}
			}

			ViewBag.Event = ev;
			ViewBag.TeamList = ev.TeamsList.ToList();
			ViewBag.pastMatches = pMatches;
			ViewBag.upcomingMatches = uMatches;
			ViewBag.runningMatches = rMatches;

		}

		private void SendEventInfoCS(EventCS ev)
		{
			List<MatchesCS> pMatches = new();
			List<MatchesCS> uMatches = new();
			List<MatchesCS> rMatches = new();

			//Base url for requests
			var requestLink = "https://api.pandascore.co/";
			var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";

			var eventID = "filter[tournament_id]=" + ev.EventAPIID.ToString();
			var pageSort = "&sort=&page=1&per_page=100";
			var fullPRequest = requestLink + "csgo" + "/matches/" + "past?" + eventID + pageSort + token;
			var fullRRequest = requestLink + "csgo" + "/matches/" + "running?" + eventID + pageSort + token;
			var fullURequest = requestLink + "csgo" + "/matches/" + "upcoming?" + eventID + pageSort + token;

			Console.WriteLine(fullPRequest);

			var pClient = new RestClient(fullPRequest);
			var rClient = new RestClient(fullRRequest);
			var uClient = new RestClient(fullURequest);

			var pReq = new RestRequest("", Method.Get);
			var rReq = new RestRequest("", Method.Get);
			var uReq = new RestRequest("", Method.Get);

			pReq.AddHeader("accept", "application/json");
			rReq.AddHeader("accept", "application/json");
			uReq.AddHeader("accept", "application/json");

			var pastMatches = pClient.Execute(pReq).Content;
			var runningMatches = rClient.Execute(rReq).Content;
			var upcomingMatches = uClient.Execute(uReq).Content;

			if (pastMatches != null)
			{

				foreach (JObject o in JArray.Parse(pastMatches).Cast<JObject>())
				{
					List<Score> scores = new();
					var m = new MatchesCS();

					m.MatchesAPIID = o.GetValue("id").Value<int>();
					m.BeginAt = o.GetValue("begin_at").ToString() == "" ? null : o.GetValue("begin_at").Value<DateTime>();
					m.TimeType = TimeType.Past;
					m.NumberOfGames = o.GetValue("number_of_games").Value<int>();

					var results = o.GetValue("results");

					foreach (JObject r in results.Cast<JObject>())
					{
						var s = new Score();
						s.Team = ev.TeamsList.FirstOrDefault(t => t.TeamAPIID == r.Value<int>("team_id"));
						if (s.Team == null)
						{
							continue;
						}
						s.TeamName = s.Team.Name;
						s.TeamScore = r.Value<int>("score");
						scores.Add(s);
					}

					m.Scores = scores;
					pMatches.Add(m);
				}
				pMatches.RemoveAll(m => m.BeginAt== null);
			}

			if (runningMatches != null)
			{

				foreach (JObject o in JArray.Parse(runningMatches).Cast<JObject>())
				{
					List<Score> scores = new();
					var m = new MatchesCS();

					m.MatchesAPIID = o.GetValue("id").Value<int>();
					m.BeginAt = o.GetValue("begin_at").ToString() == "" ? null : o.GetValue("begin_at").Value<DateTime>();
					m.TimeType = TimeType.Running;
					m.NumberOfGames = o.GetValue("number_of_games").Value<int>();

					var results = o.GetValue("results");

					foreach (JObject r in results.Cast<JObject>())
					{
						var s = new Score();
						s.Team = ev.TeamsList.FirstOrDefault(t => t.TeamAPIID == r.Value<int>("team_id"));
						s.TeamName = s.Team.Name;
						s.TeamScore = r.Value<int>("score");
						scores.Add(s);
					}

					m.Scores = scores;
					rMatches.Add(m);
				}
			}

			if (upcomingMatches != null)
			{

				foreach (JObject o in JArray.Parse(upcomingMatches).Cast<JObject>())
				{
					var m = new MatchesCS();
					List<Score> scores = new();

					m.MatchesAPIID = o.GetValue("id").Value<int>();
					m.BeginAt = o.GetValue("begin_at").ToString() == "" ? null : o.GetValue("begin_at").Value<DateTime>();
					m.TimeType = TimeType.Upcoming;
					m.NumberOfGames = o.GetValue("number_of_games").Value<int>();

					var results = o.GetValue("results");


					foreach (JObject r in results.Cast<JObject>())
					{
						var s = new Score();
						s.Team = ev.TeamsList.FirstOrDefault(t => t.TeamAPIID == r.Value<int>("team_id"));
						if (s.Team != default)
						{

							s.TeamName = s.Team.Name;
							s.TeamScore = r.Value<int>("score");

						}
						scores.Add(s);
					}

					m.Scores = scores;
					uMatches.Add(m);


				}
			}

			ViewBag.Event = ev;
			ViewBag.TeamList = ev.TeamsList.ToList();
			ViewBag.pastMatches = pMatches;
			ViewBag.upcomingMatches = uMatches;
			ViewBag.runningMatches = rMatches;
		}
	}
}
