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
            var jarray = JArray.Parse(json);
            foreach(JObject e in jarray.Cast<JObject>()) 
            {

                var ev = new EventCS();
                ev.EventAPIID = e.GetValue("id").Value<int>();
                ev.BeginAt = e.GetValue("begin_at").Value<DateTime>();
                ev.EndAt = e.GetValue("end_at").Value<DateTime>();
                ev.TimeType = Models.MatchModels.TimeType.Running;
                ev.Tier = e.GetValue("tier").Value<char>();
                ev.EventLink = e.GetValue("league").Value<string>("url");


            }
            return View();
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
