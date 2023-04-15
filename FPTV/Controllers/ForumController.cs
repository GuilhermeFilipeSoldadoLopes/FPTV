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
        public ActionResult Profile(Profile profile)
        {
            return View(profile);
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
       
        public async Task<ActionResult> React(ReactionType reaction, Guid commentId, int topicId, string game)
        {
            ViewBag.Game = game;
			var user = await _userManager.GetUserAsync(User);
			var profile = _context.Profiles.Single(p => p.Id == user.ProfileId);
            var topic = _context.Topics.FirstOrDefault(t => t.TopicId== topicId);
            var comment = _context.Comments.FirstOrDefault(c => c.CommentId.Equals(commentId));
            var react = _context.Reactions
                .Include(r => r.Comment)
                .Include(r => r.Profile)
                .FirstOrDefault(r => r.Comment.CommentId == comment.CommentId && r.Profile.Id == profile.Id && r.ReactionEmoji == reaction);

            if(react != null)
            {
                _context.Reactions.Remove(react);
                await _context.SaveChangesAsync();
				return RedirectToAction("Topic", new { id = topicId, game = ViewBag.Game });
			}

            if(comment == null || topic == null)
            {
                return View("Error404");
            }


			var r = new Reaction()
			{
				Comment = comment,
				Profile = profile,
				ReactionEmoji = reaction,
			};

            await _context.Reactions.AddAsync(r);
            await _context.SaveChangesAsync();

			try
			{
                return RedirectToAction("Topic", new { id = topicId, game = ViewBag.Game });
            }
            catch
            {
                return View();
            }
        }
    }
}
