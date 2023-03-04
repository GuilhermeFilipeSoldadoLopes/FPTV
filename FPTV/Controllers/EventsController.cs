using FPTV.Models.BLL.Events;
using FPTV.Models.EventModels;
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
            var client = new RestClient("https://api.pandascore.co/csgo/tournaments/running?sort=&page=1&per_page=6&token=QjxkIEQTAFmy992BA0P-k4urTl4PiGYDL4F-aqeNmki0cgP0xCA");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            var json = client.Execute(request).Content;
            if (json == null)
            {
                return View();
            }
            Console.WriteLine(json);
            var jarray = JArray.Parse(json);
            List<EventCS> events = new(); 
            Dictionary<int, string?> teamList = new();
            foreach(JObject e in jarray.Cast<JObject>()) 
            {
                var ev = new EventCS();
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

                ev.EventAPIID = eventAPIID == null ? -1 : eventAPIID.Value<int>();
                ev.BeginAt = beginAt == null ? null : beginAt.Value<DateTime>();
                ev.EndAt = endAt == null ? null : endAt.Value<DateTime>();
                ev.TimeType = timeType;
                ev.Tier = tier == null ? null : tier.Value<char>();
                ev.EventLink = league == null ? null : league.Value<string>("url");
                ev.Finished = false;
                ev.EventName = name == null ? null : name.Value<string>();
                ev.PrizePool = prizePool == null ? null : prizePool.Value<string>();
                ev.WinnerTeamID = winnerTeamId == null ? -1 : winnerTeamId.Value<int>(); 
                
                if (teams != null)
                {
                    foreach (JObject o in (JArray)teams.Value<string>())
                    {
                        var teamNameValue = o.GetValue("name");
                        var teamIdValue = o.GetValue("id");
                        var teamId = teamIdValue == null ? -1 : teamIdValue.Value<int>();
                        var teamName = teamNameValue == null ? null : teamNameValue.Value<string>();
                        teamList.Add(teamId, teamName);

                    }
                }

                ev.TeamsList = teamList.Values.ToList();
                ev.WinnerTeamName = teamList.GetValueOrDefault(ev.WinnerTeamID);
                events.Add(ev);
                Console.WriteLine(ev.EventAPIID);

            }
            return View(events);
        }

        // GET: EventsController/Details/5
        public ActionResult Details(int id)
        {
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
