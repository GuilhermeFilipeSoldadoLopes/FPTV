using FPTV.Data;
using FPTV.Models.Forum;
using FPTV.Models.UserModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FPTV.Controllers
{
    public class ForumController : Controller
    {
        private readonly FPTVContext _context;
        private readonly UserManager<UserBase> _userManager;

        public ForumController(FPTVContext context, UserManager<UserBase> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: ForumController
        public ActionResult Index(string game = "csgo")
        {
            ViewBag.Game = game;
			Enum.TryParse(game, true, out GameType gametype);
			var topics = _context.Topics.Include(t => t.Profile).ThenInclude(p => p.User).Include(t => t.Comments).ThenInclude(c => c.Reactions).Where(t => t.GameType == gametype).ToList();
            return View(topics);
        }

       
        public ActionResult Topic(int id, string game = "csgo")
        {
            ViewBag.Game = game;
            var topic = _context.Topics
                .Include(t => t.Profile)
                .ThenInclude(p => p.User)
                .Include(t => t.Comments)
                .ThenInclude(c => c.Reactions)
                .Include(t => t.Comments)
                .ThenInclude(c => c.Profile)
                .ThenInclude(p => p.User)
                .FirstOrDefault(t => t.TopicId == id);
            if(topic == default)
            {
                return View();
            }

            return View(topic);
        }

        // GET: ForumController/Create
        public ActionResult NewTopic(string game = "csgo")
        {
            ViewBag.Game = game;
            return View();
        }

        // POST: ForumController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(IFormCollection collection)
        {
            ViewBag.Game = collection["Game"];
            
			
			var user = await _userManager.GetUserAsync(User);
            
            if(user == null)
            {
				
				return View("Index");
            }
            var profile = _context.Profiles.Single(p => p.Id == user.ProfileId);
            Enum.TryParse<GameType>(ViewBag.Game, true, out GameType gameType);
            var topic = new Topic
            {
                Content = collection["Content"],
                Profile = profile,
                Comments = new List<Comment>(),
                Date = DateTime.Now,
                Title = collection["Title"],
                GameType = gameType,
                ProfileId = user.ProfileId,
            };
            await _context.Topics.AddAsync(topic);
            await _context.SaveChangesAsync();
            try
            {
                return RedirectToAction("Topic", new {id = topic.TopicId,game = ViewBag.Game});
            }
            catch
            {
                return View();
            }
            // ToDo: Implement redirect, check forms in view
        }

        // GET: ForumController/Edit/5
        public ActionResult Profile(Guid id)
        {
            var user = _context.UserBase.FirstOrDefault(u => u.Id == id.ToString());
            return View();
        }

        // POST: ForumController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<ActionResult> CommentAsync(IFormCollection collection)
        {
			ViewBag.Game = collection["Game"];
            int.TryParse(collection["TopicId"], out int id);
            var topic = _context.Topics.FirstOrDefault(t => t.TopicId == id);
			var user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return View("Index");
			}
			var profile = _context.Profiles.Single(p => p.Id == user.ProfileId);
			Enum.TryParse<GameType>(ViewBag.Game, true, out GameType gameType);
            var comment = new Comment
            {
                Reactions = new List<Reaction>(),
				Profile = profile,
				Date = DateTime.Now,
                Text = collection["Text"],
                Topic = topic,
			};
			await _context.Comments.AddAsync(comment);
			await _context.SaveChangesAsync();
			try
            {
                return RedirectToAction("Topic", new { id, game = ViewBag.Game});
            }
            catch
            {
                return View();
            }
        }

        // GET: ForumController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ForumController/Delete/5
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
