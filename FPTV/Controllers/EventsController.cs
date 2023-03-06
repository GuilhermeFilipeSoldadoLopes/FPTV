using FPTV.Models.BLL.Events;
using FPTV.Models.EventModels;
using FPTV.Models.MatchModels;
using FPTV.Models.StatisticsModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace FPTV.Controllers
{
    public class EventsController : Controller
    {
        // GET: EventsController
        public ActionResult Index()
        {
			//Request processing with RestSharp
			var client = new RestClient("https://api.pandascore.co/csgo/tournaments/running?sort=-begin_at&page=1&per_page=6&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA");
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
                var timeType = Models.MatchModels.TimeType.Running;
                var league = e.GetValue("league");
                var teams = e.GetValue("teams");
                var prizePool = e.GetValue("prizepool");
                var winnerTeamId = e.GetValue("winner_id");

				//Handling for null values
				ev.EventAPIID = eventAPIID.ToString() == null ? -1 : eventAPIID.Value<int>();
                ev.BeginAt = beginAt.ToString() == "" ? null : beginAt.Value<DateTime>();
                ev.TimeType = timeType;
                ev.EventName = league.ToString() == "" ? null : league.Value<string>("name");
                ev.EventStage = nameStage.ToString() == "" ? null : nameStage.Value<string>();
				ev.PrizePool = prizePool.ToString() == "" ? "-" : new string(prizePool.Value<string>().Where(char.IsDigit).ToArray());
				ev.WinnerTeamID = winnerTeamId.ToString() == "" ? -1 : winnerTeamId.Value<int>(); 
                
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
                ev.WinnerTeamName = teamList.GetValueOrDefault(ev.WinnerTeamID) == null ? "-" : teamList.GetValueOrDefault(ev.WinnerTeamID);
                events.Add(ev);

			}
            return View(events);
        }

        // GET: EventsController/Details/5
        public ActionResult Details(int id, string filter = "running")
        {
            //Base url for requests
            var requestLink = "https://api.pandascore.co/csgo/tournaments/";

            //Filter to select from which pool to fetch the data (upcoming, running or finished/ended)
			var jsonFilter = filter + "?";
            var filterID = "filter[id]=" + id.ToString();

            //THIS SHOULD BE A CLIENT SECRET
			var token = "&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA";

            //Request processing with RestSharp
			var fullRequest = requestLink + jsonFilter + filterID + token;
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
            List<MatchCS> matches = new();
			var ev = new EventCS();

			foreach (JObject e in jarray.Cast<JObject>())
            {
                //Set up values from api
                Dictionary<int, string?> teamList = new();
                
                var eventAPIID = e.GetValue("id");
                var name = e.GetValue("name");
                var beginAt = e.GetValue("begin_at");
                var endAt = e.GetValue("end_at");
                var timeType = Models.MatchModels.TimeType.Running;
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
                ev.EventName = name.ToString() == "" ? null : name.Value<string>();
                ev.PrizePool = prizePool.ToString() == "" ? "-" : new string(prizePool.Value<string>().Where(char.IsDigit).ToArray());
                ev.WinnerTeamID = winnerTeamId.ToString() == "" ? -1 : winnerTeamId.Value<int>();

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
                }

                if(matches != null)
                {
					foreach (JObject o in matches.Cast<JObject>())
					{
                        var m = new MatchCS();
                        
					}
				}
                

            //Set up remaining fields
            ev.TeamsList = teamList.Values.ToList();
            ev.WinnerTeamName = teamList.GetValueOrDefault(ev.WinnerTeamID);
            
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
