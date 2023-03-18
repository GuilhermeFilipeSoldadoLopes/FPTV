using FPTV.Models.BLL.Events;
using FPTV.Models.EventsModels;
using FPTV.Models.MatchesModels;
using FPTV.Models.StatisticsModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;

namespace FPTV.Controllers
{
    public class EventsController : Controller
    {
        // GET: EventsController
        public ActionResult Index(string sort = "&sort=-begin_at", string filter = "running", string search = "", string page = "&page=1", string game = "csgo")
        {
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
            List<EventCS> events = new(); 
            
            foreach(JObject e in jarray.Cast<JObject>()) 
            {
				//Set up values from api
				Dictionary<int, string?> teamList = new();
				var ev = new EventCS();
                var eventAPIID = e.GetValue("id");
                var nameStage = e.GetValue("name");
                var beginAt = e.GetValue("begin_at");
                var league = e.GetValue("league");
                var teams = e.GetValue("teams");
                var prizePool = e.GetValue("prizepool");
                var winnerTeamId = e.GetValue("winner_id");

				//Handling for null values
				ev.EventAPIID = eventAPIID.ToString() == null ? -1 : eventAPIID.Value<int>();
                ev.BeginAt = beginAt.ToString() == "" ? null : beginAt.Value<DateTime>();
                ev.EventName = league.ToString() == "" ? null : league.Value<string>("name");
                ev.LeagueName = nameStage.ToString() == "" ? null : nameStage.Value<string>();
				ev.PrizePool = prizePool.ToString() == "" ? "-" : new string(prizePool.Value<string>().Where(char.IsDigit).ToArray());
				ev.WinnerTeamAPIID = winnerTeamId.ToString() == "" ? -1 : winnerTeamId.Value<int>();

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
                        teamList.Add(teamId, teamName);
                    }
                }

                //Filling remaining fields
                //ev.TeamsList = teamList.Values.ToList();
                ev.WinnerTeamName = teamList.GetValueOrDefault((int) ev.WinnerTeamAPIID) ?? "-";
                events.Add(ev);
			}

			if (sort == "&sort=name")
			{
				events.Sort((x, y) => x.EventName.CompareTo(y.EventName));
			}

			if (search != "")
            {
                events = events.Where(e => e.EventName.Contains(search)).ToList();
            }

           
            ViewBag.filter = filter;
            ViewBag.sort = sort;

            return View(events);
        }

        // GET: EventsController/Details/5
        public ActionResult Details(int id, string filter = "running", string game = "csgo")
        {
            
            //Base url for requests
            var requestLink = "https://api.pandascore.co/";

            //Filter to select from which pool to fetch the data (upcoming, running or finished/ended)
			var jsonFilter = filter + "?";
            var filterID = "filter[id]=" + id.ToString();

            //THIS SHOULD BE A CLIENT SECRET
			var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";

            //Request processing with RestSharp
			var fullRequest = requestLink + game + "/tournaments/" + jsonFilter + filterID + token;
            var client = new RestClient(fullRequest);
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            var json = client.Execute(request).Content;

            if (json == null)
            {
                return View(); //We need an error handler for this!
            }

            var jarray = JArray.Parse(json);
            List<EventCS> events = new();
            List<MatchesCS> pMatches = new();
            List<MatchesCS> uMatches = new();
            List<MatchesCS> rMatches = new();
			var ev = new EventCS();

			foreach (JObject e in jarray.Cast<JObject>())
            {
                //Set up values from api
                Dictionary<int, string?> teamList = new();
                
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
				ev.PrizePool = prizePool.ToString() == "" ? "-" : new string(prizePool.Value<string>().Where(char.IsDigit).ToArray());
                ev.WinnerTeamAPIID = winnerTeamId.ToString() == "" ? -1 : winnerTeamId.Value<int>();

                if (teams != null)
                {
                    foreach (JObject o in teams.Cast<JObject>())
                    {
                        var teamNameValue = o.GetValue("name");
                        var teamIdValue = o.GetValue("id");
                        var teamId = teamIdValue.ToString() == "" ? -1 : teamIdValue.Value<int>();
                        var teamName = teamNameValue.ToString() == "" ? null : teamNameValue.Value<string>();
                        teamList.Add(teamId, teamName);
                    }
                    //ev.TeamsList = teamList.Values.ToList();
                }

                if(matchesJ != null)
                {
                    var eventID = "filter[tournament_id]=" + ev.EventAPIID.ToString();
                    var pageSort = "&sort=&page=1&per_page=100";
					var fullPRequest = requestLink + game + "/matches/" + "past?" + eventID + pageSort + token;
					var fullRRequest = requestLink + game + "/matches/" + "running?" + eventID + pageSort + token;
					var fullURequest = requestLink + game + "/matches/" + "upcoming?" + eventID + pageSort + token;

                    Console.WriteLine(fullURequest);

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
							var m = new MatchesCS();
							var s = new Dictionary<int, int>();
							m.MatchesCSAPIID = o.GetValue("id").Value<int>();
							m.BeginAt = o.GetValue("begin_at").Value<DateTime>();
							m.TimeType = TimeType.Past;
							m.NumberOfGames = o.GetValue("number_of_games").Value<int>();

							var results = o.GetValue("results");

							foreach (JObject r in results.Cast<JObject>())
							{
								s.Add(r.Value<int>("team_id"), r.Value<int>("score"));
							}

							//m.Score = s;
							pMatches.Add(m);
						}
					}
                    
                    if(runningMatches != null)
                    {
						foreach (JObject o in JArray.Parse(runningMatches).Cast<JObject>())
						{
							var m = new MatchesCS();
							var s = new Dictionary<int, int>();
							m.MatchesCSAPIID = o.GetValue("id").Value<int>();
							m.BeginAt = o.GetValue("begin_at").Value<DateTime>();
							m.TimeType = TimeType.Running;
							m.NumberOfGames = o.GetValue("number_of_games").Value<int>();

							var results = o.GetValue("results");

							foreach (JObject r in results.Cast<JObject>())
							{
								s.Add(r.Value<int>("team_id"), r.Value<int>("score"));
                                Console.WriteLine(teamList[s.First().Key]);
							}

                            //m.Score = s;
                            rMatches.Add(m);
						}
					}

					if (upcomingMatches != null)
					{
						foreach (JObject o in JArray.Parse(upcomingMatches).Cast<JObject>())
						{
							var m = new MatchesCS();
							var s = new Dictionary<int, int>();
							m.MatchesCSAPIID = o.GetValue("id").Value<int>();
							m.BeginAt = o.GetValue("begin_at").Value<DateTime>();
							m.TimeType = TimeType.Upcoming;
							m.NumberOfGames = o.GetValue("number_of_games").Value<int>();

							var results = o.GetValue("results");

                            
							foreach (JObject r in results.Cast<JObject>())
							{
                               
								s.Add(r.Value<int>("team_id"), r.Value<int>("score"));

							}

                            //m.Score = s;
							uMatches.Add(m);
                            

                        }
					}

				}
                
                
                ViewBag.Event = ev;
                ViewBag.TeamList = teamList;
                ViewBag.pastMatches = pMatches;
                ViewBag.upcomingMatches = uMatches;
                ViewBag.runningMatches = rMatches;

			}

            return View();
        }

        // GET: EventsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EventsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: EventsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EventsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: EventsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EventsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
