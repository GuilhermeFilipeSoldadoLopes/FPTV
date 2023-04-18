using FPTV.Data;
using FPTV.Models.Forum;
using FPTV.Models.UserModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

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
        public async Task<ActionResult> IndexAsync()
        {
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return View("~/Views/Home/Error403.cshtml");
			}
			var profile = _context.Profiles.FirstOrDefault(p => p.Id == user.ProfileId);
			if (profile == null)
			{
				return View("~/Views/Home/Error403.cshtml");
			}

			ViewBag.Game = "";
            ViewBag.page = "Forum";
			var topics = _context.Topics.Include(t => t.Profile).ThenInclude(p => p.User).Include(t => t.Comments).ThenInclude(c => c.Reactions).ToList();
            return View(topics);
        }


		public ActionResult Topic(int id)
        {
			ViewBag.Game = "";
			ViewBag.page = "Forum";
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
				return View("~/Views/Home/Error404.cshtml");
			}

            return View(topic);
        }

        // GET: ForumController/Create
        public ActionResult NewTopic()
        {
            ViewBag.page = "Forum";
			ViewBag.Game = "";
			return View();
        }

        // POST: ForumController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(IFormCollection collection)
        {
			ViewBag.Game = "";
            ViewBag.page = "Forum";


            var user = await _userManager.GetUserAsync(User);
            
            if(user == null)
            {
				return View("~/Views/Home/Error404.cshtml");
            }
            var profile = _context.Profiles.FirstOrDefault(p => p.Id == user.ProfileId);

			if (profile == null)
			{
				return View("~/Views/Home/Error404.cshtml");
			}
            var topic = new Topic
            {
                Content = collection["Content"],
                Profile = profile,
                Comments = new List<Comment>(),
                Date = DateTime.Now,
                Title = collection["Title"],
                ProfileId = user.ProfileId,
                Reported = false,
            };
            await _context.Topics.AddAsync(topic);
            await _context.SaveChangesAsync();
            try
            {
                return RedirectToAction("Topic", new {id = topic.TopicId});
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

        public ActionResult ReportComment(Guid id, int topicId)
        {
			ViewBag.Game = "";
			ViewBag.page = "Forum";
            var comment = _context.Comments.Include(c => c.Profile).FirstOrDefault(c => c.CommentId == id);
            if (comment == null)
            {
				return View("~/Views/Home/Error404.cshtml");
			}

            comment.Reported= true;
            _context.SaveChanges();
			return RedirectToAction("Topic", new { id = topicId});
		}

        public ActionResult ReportPost(int id) 
        {
			ViewBag.Game = "";
			ViewBag.page = "Forum";
            var post = _context.Topics.FirstOrDefault(t => t.TopicId == id);

            if (post == null) 
            {
				return View("~/Views/Home/Error404.cshtml");
			}

            post.Reported= true;
            _context.SaveChanges();

			return RedirectToAction("Topic", new { id});
		}

        // POST: ForumController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<ActionResult> CommentAsync(IFormCollection collection)
        {
			ViewBag.Game = "";
            ViewBag.page = "Forum";
            int.TryParse(collection["TopicId"], out int id);
            var topic = _context.Topics.FirstOrDefault(t => t.TopicId == id);
			var user = await _userManager.GetUserAsync(User);

			if (user == null)
			{
				return View("Index");
			}
			var profile = _context.Profiles.Single(p => p.Id == user.ProfileId);
            var comment = new Comment
            {
                Reactions = new List<Reaction>(),
				Profile = profile,
				Date = DateTime.Now,
                Text = collection["Text"],
                Topic = topic,
                Reported = false,
			};
			await _context.Comments.AddAsync(comment);
			await _context.SaveChangesAsync();
			try
            {
                return RedirectToAction("Topic", new { id});
            }
            catch
            {
                return View();
            }
        }

        // GET: ForumController/Delete/5
        public async Task<ActionResult> DeletePostAsync(int id)
        {
			ViewBag.Game = "";
			ViewBag.page = "Forum";
            var post = _context.Topics.FirstOrDefault(t => t.TopicId == id);
            var user = await _userManager.GetUserAsync(User);
            if (post == null || post.ProfileId != user.ProfileId)
            {
				return View("~/Views/Home/Error404.cshtml");
			}
            post.Content = "This post has been deleted.";
            post.Title = "[Deleted post]";

            await _context.SaveChangesAsync();

            return RedirectToAction("Topic", new { id });
        }

		public async Task<ActionResult> DeleteCommentAsync(Guid id, int topicId)
		{
			ViewBag.Game = "";
            ViewBag.page = "Forum";
            var comment = _context.Comments.Include(c => c.Profile).FirstOrDefault(c => c.CommentId == id);
			var user = await _userManager.GetUserAsync(User);
			if (comment == null || !comment.Profile.Id.Equals(user.ProfileId))
			{
				return View("~/Views/Home/Error404.cshtml");
			}
			comment.Text = "[Deleted comment]";

			await _context.SaveChangesAsync();

			return RedirectToAction("Topic", new { id = topicId});
		}

		// POST: ForumController/Delete/5

		public async Task<ActionResult> React(ReactionType reaction, Guid commentId, int topicId)
        {
            ViewBag.Game = "";
            ViewBag.page = "Forum";
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
				return RedirectToAction("Topic", new { id = topicId});
			}

            if(comment == null || topic == null)
            {
				return View("~/Views/Home/Error404.cshtml");
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
                return RedirectToAction("Topic", new { id = topicId});
            }
            catch
            {
                return View();
            }
        }

		public ActionResult Rules()
		{
			ViewBag.Game = "";
			ViewBag.page = "Forum";
			return View();
		}

        public ActionResult BugsAndSuggestions()
        {
            ViewBag.Game = "";
            ViewBag.page = "Forum";
            return View();
        }
    }
}
