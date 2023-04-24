using FPTV.Data;
using FPTV.Models.Forum;
using FPTV.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FPTV.Controllers
{

    /// <summary>
    /// The ForumController class is responsible for handling requests related to the forum.
    /// </summary>
    public class ForumController : Controller
    {
        private readonly FPTVContext _context;
        private readonly UserManager<UserBase> _userManager;

        /// <summary>
        /// Constructor for ForumController class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="userManager">The user manager.</param>
        /// <returns>
        /// A new instance of ForumController.
        /// </returns>
        public ForumController(FPTVContext context, UserManager<UserBase> userManager)
        {
            _context = context;
            _userManager = userManager;
        }        

        /// <summary>
        /// Asynchronously checks for an error and returns the Error403 view if an error is found. Otherwise, returns the Forum view with a list of topics.
        /// </summary>
        /// <returns>The Error403 view or the Forum view.</returns>
        public async Task<ActionResult> IndexAsync(string filter = "", string search = "")
        {
            if (await CheckError303())
            {
                return View("~/Views/Home/Error403.cshtml");
            }

            search ??= "";
            ViewBag.Filter = filter;
            ViewBag.Search = search;
            ViewBag.Game = "";
            ViewBag.page = "Forum";
			var topics = _context.Topics.Include(t => t.Profile).ThenInclude(p => p.User).Include(t => t.Comments).ThenInclude(c => c.Reactions).ToList();

			switch (filter)
			{
				case "newest":
					topics = topics.OrderByDescending(t => t.Date).ToList();
					break;
				case "oldest":
					topics = topics.OrderBy(t => t.Date).ToList();
					break;
				case "hot":
					topics = topics.OrderByDescending(t => t.Comments.Select(c => c.Reactions).Count() + t.Comments.Count).ToList();
					break;
				default:
					break;
			}

            if (search != null)
            {
                topics = topics.FindAll(t => t.Title.ToLower().Contains(search.ToLower()));
            } 

            topics = topics.OrderBy(t => t.Deleted).ToList();

			return View(topics);
        }

		public IActionResult ReportedTopicsAndComments()
		{
            var topics = _context.Topics.Include(t => t.Profile).Where(t => t.Reported == true).ToList();

            var comments = _context.Comments.Include(c => c.Profile).Where(c => c.Reported == true).ToList();
            ViewBag.TopicsReported = topics;
            ViewBag.CommentsReported = comments;

            //page = "Forum";
            return View();
		}

		public async Task<ActionResult> DeleteReportedPostAsync(int id)
		{

			if (await CheckError303())
			{
				return View("~/Views/Home/Error403.cshtml");
			}

			ViewBag.Game = "";
			ViewBag.page = "Forum";
			var post = _context.Topics.Include(t => t.Comments).ThenInclude(c => c.Reactions).FirstOrDefault(t => t.TopicId == id);
			var user = await _userManager.GetUserAsync(User);
			var userRoles = await _userManager.GetRolesAsync(user);

			if (post == null)
			{
				return View("~/Views/Home/Error404.cshtml");
			}

			var comments = post.Comments;

			if (userRoles.Contains("admin", StringComparer.OrdinalIgnoreCase) || userRoles.Contains("moderator", StringComparer.OrdinalIgnoreCase) && post.Reported)
			{

				foreach (var comment in comments)
				{
                    if(comment.Reactions != null) 
                    {
						_context.RemoveRange(comment.Reactions);
					}
				}

				_context.RemoveRange(comments);
				_context.Remove(post);

				await _context.SaveChangesAsync();

				return RedirectToAction("ReportedTopicsAndComments");
			}

			return View("~/Views/Home/Error403.cshtml");
		}

		public async Task<ActionResult> DeleteReportedCommentAsync(Guid id)
		{

			if (await CheckError303())
			{
				return View("~/Views/Home/Error403.cshtml");
			}

			ViewBag.Game = "";
			ViewBag.page = "Forum";
			var comment = _context.Comments.Include(c => c.Reactions).FirstOrDefault(c => c.CommentId.Equals(id));
			var user = await _userManager.GetUserAsync(User);
			var userRoles = await _userManager.GetRolesAsync(user);

			if (comment == null)
			{
				return View("~/Views/Home/Error404.cshtml");
			}

			var reactions = comment.Reactions;

			if (userRoles.Contains("admin", StringComparer.OrdinalIgnoreCase) || userRoles.Contains("moderator", StringComparer.OrdinalIgnoreCase) && comment.Reported)
			{

				if(reactions != null)
                {
					_context.RemoveRange(reactions);
				}
			  
				_context.Remove(comment);

				await _context.SaveChangesAsync();

				return RedirectToAction("ReportedTopicsAndComments");
			}

			return View("~/Views/Home/Error403.cshtml");
		}

		[Authorize]
        /// <summary>
        /// Retrieves a topic from the database and returns it to the view.
        /// </summary>
        /// <param name="id">The id of the topic to be retrieved.</param>
        /// <returns>The view with the retrieved topic.</returns>
        public async Task<ActionResult> TopicAsync(int id, string filter = "", string alert = "")
        {
            if (await CheckError303())
            {
                return View("~/Views/Home/Error403.cshtml");
            }

            if (alert != "" && alert != null) {
				ViewBag.Message = alert;
			} else {
				ViewBag.Message = "";
			}

			ViewBag.Filter = filter;
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
            if (topic == default)
            {
                return View("~/Views/Home/Error404.cshtml");
            }

			switch(filter)
            {
				case "newest":
					topic.Comments = topic.Comments.OrderByDescending(c => c.Date).ToList();
					break;
				case "oldest":
					topic.Comments = topic.Comments.OrderBy(c => c.Date).ToList();
					break;
				case "hot":
					topic.Comments = topic.Comments.OrderByDescending(c => c.Reactions.Count).ToList();
					break;
				default:
					break;
            }

            topic.Comments = topic.Comments.OrderBy(c => c.Deleted).ToList();
            return View(topic);
        }

        [Authorize]
        // GET: ForumController/Create
        /// <summary>
        /// This method returns a View for creating a new topic in the forum.
        /// </summary>
        /// <returns>A View for creating a new topic in the forum.</returns>
        public ActionResult NewTopic()
        {
            ViewBag.page = "Forum";
            ViewBag.Game = "";
            return View();
        }

        // POST: ForumController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Creates a new topic in the forum.
        /// </summary>
        /// <param name="collection">The form collection.</param>
        /// <returns>Redirects to the topic page or an error page.</returns>
        public async Task<ActionResult> CreateAsync(IFormCollection collection)
        {
            if (await CheckError303())
            {
                return View("~/Views/Home/Error403.cshtml");
            }

            ViewBag.Game = "";
            ViewBag.page = "Forum";


            var user = await _userManager.GetUserAsync(User);

            if (user == null)
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
				Deleted = false,
            };
            await _context.Topics.AddAsync(topic);
            await _context.SaveChangesAsync();
            try
            {
                return RedirectToAction("Topic", new { id = topic.TopicId });
            }
            catch
            {
                return View();
            }
            // ToDo: Implement redirect, check forms in view
        }

        [Authorize]
        // GET: ForumController/Edit/5
        /// <summary>
        /// Returns the view of the given profile.
        /// </summary>
        /// <param name="profile">The profile to be viewed.</param>
        /// <returns>The view of the given profile.</returns>
        public ActionResult Profile(Guid profileId)
        {
			var profile = _context.Profiles.Include(p => p.PlayerList.Players).Include(p => p.TeamsList.Teams).FirstOrDefault(p => p.Id == profileId);
            var user = _context.UserBase.FirstOrDefault(u => u.ProfileId == profileId);
            var topics = _context.Topics.Where(t => t.ProfileId == profileId).ToList();
            

            if(user == null || profile == null)
            {
                return View("~/Views/Home/Error404.cshtml");
            }

            ViewData["Topics"] = topics;
            ViewData["FavTeamsListValorant"] = profile.TeamsList.Teams.Where(t => t.Game == GameType.Valorant).ToList();
            ViewData["FavPlayerListValorant"] = profile.PlayerList.Players.Where(t => t.Game == GameType.Valorant).ToList(); 
            ViewData["FavTeamsListCSGO"] = profile.TeamsList.Teams.Where(t => t.Game == GameType.CSGO).ToList();
            ViewData["FavPlayerListCSGO"] = profile.PlayerList.Players.Where(t => t.Game == GameType.CSGO).ToList();
            ViewBag.Flag = "/images/Flags/4x3/" + profile.Flag + ".svg";
            ViewData["Username"] = user.UserName;
			return View(profile);
        }

        [Authorize]
        /// <summary>
        /// Reports a comment and sets the Reported flag to true.
        /// </summary>
        /// <param name="id">The ID of the comment to be reported.</param>
        /// <param name="topicId">The ID of the topic the comment belongs to.</param>
        /// <returns>Redirects to the topic page.</returns>
        public ActionResult ReportComment(Guid id, int topicId)
        {
            ViewBag.Game = "";
            ViewBag.page = "Forum";

			var comment = _context.Comments.Include(c => c.Profile).FirstOrDefault(c => c.CommentId == id);
            if (comment == null)
            {
                return View("~/Views/Home/Error404.cshtml");
            }

            DateTime time = DateTime.Now;
			var profile = _context.Profiles.FirstOrDefault(p => p == comment.Profile);
			var user = _context.Users.FirstOrDefault(u => u.ProfileId == profile.Id);
			string message = ("The comment of " + comment.Profile.User.UserName + " was successfully reported at " + time.ToString("HH:mm") + " WEST");

			comment.Reported = true;
            _context.SaveChanges();

            return RedirectToAction("Topic", new { id = topicId, alert = message });
        }

        [Authorize]
        /// <summary>
        /// Reports a post with the given ID.
        /// </summary>
        /// <param name="id">The ID of the post to be reported.</param>
        /// <returns>Redirects to the topic page.</returns>
        public ActionResult ReportPost(int id)
        {
            ViewBag.Game = "";
            ViewBag.page = "Forum";

			var post = _context.Topics.FirstOrDefault(t => t.TopicId == id);

            if (post == null)
            {
                return View("~/Views/Home/Error404.cshtml");
            }

			var profile = _context.Profiles.FirstOrDefault(p => p.Id == post.ProfileId);
			var user = _context.Users.FirstOrDefault(u => u.ProfileId == profile.Id);
			DateTime time = DateTime.Now;
			string message = ("The topic of " + user.UserName + " was successfully reported at " + time.ToString("HH:mm") + " WEST");

			post.Reported = true;
            _context.SaveChanges();

            return RedirectToAction("Topic", new { id, alert = message });
        }

        // POST: ForumController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        /// <summary>
        /// Creates a new comment for a given topic and adds it to the database.
        /// </summary>
        /// <param name="collection">The form collection containing the comment text.</param>
        /// <returns>Redirects to the topic page.</returns>
        public async Task<ActionResult> CommentAsync(IFormCollection collection)
        {
            if (await CheckError303())
            {
                return View("~/Views/Home/Error403.cshtml");
            }

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
				Deleted = false,
			};
			await _context.Comments.AddAsync(comment);
			await _context.SaveChangesAsync();
			try
            {
                return RedirectToAction("Topic", new { id });
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Deletes a post from the forum.
        /// </summary>
        /// <returns>
        /// Redirects to the topic page.
        /// </returns>
        public async Task<ActionResult> DeletePostAsync(int id)
        {
            if (await CheckError303())
            {
                return View("~/Views/Home/Error403.cshtml");
            }

            ViewBag.Game = "";
            ViewBag.page = "Forum";
            var post = _context.Topics.Include(t => t.Comments).ThenInclude(c => c.Reactions).FirstOrDefault(t => t.TopicId == id);
            var user = await _userManager.GetUserAsync(User);
            var userRoles = await _userManager.GetRolesAsync(user);

            if(post == null) 
            {
				return View("~/Views/Home/Error404.cshtml");
			}

			var comments = post.Comments;

			if (userRoles.Contains("admin", StringComparer.OrdinalIgnoreCase) || userRoles.Contains("moderator", StringComparer.OrdinalIgnoreCase))
            {

                foreach(var comment in comments)
                {
					if (comment.Reactions != null)
					{
						_context.RemoveRange(comment.Reactions);
					}
				}

                _context.RemoveRange(comments);
                _context.Remove(post);

				await _context.SaveChangesAsync();

				return RedirectToAction("Index");
			}

            if (post.ProfileId != user.ProfileId)
            {
				return View("~/Views/Home/Error403.cshtml");
			}

			foreach (var comment in comments)
			{
                
			
			}

			_context.RemoveRange(comments);
			_context.Remove(post);

			await _context.SaveChangesAsync();

			return RedirectToAction("Index");
        }

		

		/// <summary>
		/// Deletes a comment from the database.
		/// </summary>
		/// <param name="id">The ID of the comment to delete.</param>
		/// <param name="topicId">The ID of the topic the comment belongs to.</param>
		/// <returns>A redirect to the topic page.</returns>
		public async Task<ActionResult> DeleteCommentAsync(Guid id, int topicId)
        {

            if (await CheckError303())
            {
                return View("~/Views/Home/Error403.cshtml");
            }

            ViewBag.Game = "";
            ViewBag.page = "Forum";
            var comment = _context.Comments.Include(c => c.Profile).Include(c => c.Reactions).FirstOrDefault(c => c.CommentId == id);
			var user = await _userManager.GetUserAsync(User);
			var userRoles = await _userManager.GetRolesAsync(user);

            if(comment == null)
            {
				return View("~/Views/Home/Error404.cshtml");
			}

            if (userRoles.Contains("admin", StringComparer.OrdinalIgnoreCase) || userRoles.Contains("moderator", StringComparer.OrdinalIgnoreCase))
            {
                if(comment.Reactions != null) 
                {
					_context.RemoveRange(comment.Reactions);
				}

                _context.Remove(comment);
                await _context.SaveChangesAsync();

				return RedirectToAction("Topic", new { id = topicId });
			}
			
            if (!comment.Profile.Id.Equals(user.ProfileId))
			{
				return View("~/Views/Home/Error403.cshtml");
			}

			if (comment.Reactions != null)
			{
				_context.RemoveRange(comment.Reactions);
			}

			_context.Remove(comment);
			await _context.SaveChangesAsync();

            return RedirectToAction("Topic", new { id = topicId });
        }

        // POST: ForumController/Delete/5
        /// <summary>
        /// React to a comment with a given reaction type.
        /// </summary>
        /// <param name="reaction">The type of reaction to give.</param>
        /// <param name="commentId">The ID of the comment to react to.</param>
        /// <param name="topicId">The ID of the topic the comment belongs to.</param>
        /// <returns>A redirect to the topic page.</returns>
        public async Task<ActionResult> React(ReactionType reaction, Guid commentId, int topicId)
        {
            if (await CheckError303())
            {
                return View("~/Views/Home/Error403.cshtml");
            }

            ViewBag.Game = "";
            ViewBag.page = "Forum";
            var user = await _userManager.GetUserAsync(User);
            var profile = _context.Profiles.Single(p => p.Id == user.ProfileId);
            var topic = _context.Topics.FirstOrDefault(t => t.TopicId == topicId);
            var comment = _context.Comments.FirstOrDefault(c => c.CommentId.Equals(commentId));
            var react = _context.Reactions
                .Include(r => r.Comment)
                .Include(r => r.Profile)
                .FirstOrDefault(r => r.Comment.CommentId == comment.CommentId && r.Profile.Id == profile.Id);

            if (react != null)
            {
                _context.Reactions.Remove(react);
                await _context.SaveChangesAsync();

                if(react.ReactionEmoji.Equals(reaction))
                {
					return RedirectToAction("Topic", new { id = topicId });
				}

            }

            if (comment == null || topic == null)
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
                return RedirectToAction("Topic", new { id = topicId });
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        /// <summary>
        /// This ActionResult method is used to display the rules page.
        /// </summary>
        /// <returns>A View containing the rules page.</returns>
        public ActionResult Rules()
        {
            ViewBag.Game = "";
            ViewBag.page = "Forum";
            return View();
        }

        /// <summary>
        /// This method is used to render the Bugs and Suggestions page.
        /// </summary>
        /// <returns>
        /// Returns the view for the Bugs and Suggestions page.
        /// </returns>
        public async Task<ActionResult> BugsAndSuggestionsAsync()
        {
            if (await CheckError303())
            {
                return View("~/Views/Home/Error403.cshtml");
            }

            ViewBag.Game = "";
            ViewBag.page = "Forum";
            return View();
        }

        /// <summary>
        /// Retrieves a list of topics from the database and passes them to the view.
        /// </summary>
        /// <returns>A view containing the list of topics.</returns>
        public ActionResult IndexAsyncTest()
        {
            ViewBag.Game = "";
            ViewBag.page = "Forum";
            var topics = _context.Topics.Include(t => t.Profile).ThenInclude(p => p.User).Include(t => t.Comments).ThenInclude(c => c.Reactions).ToList();
            return View(topics);
        }

        /// <summary>
        /// Checks if an error 303 has occurred.
        /// </summary>
        /// <returns>
        /// Returns true if an error 303 has occurred, false otherwise.
        /// </returns>
        private async Task<bool> CheckError303()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return true;
            }
            var profile = _context.Profiles.FirstOrDefault(p => p.Id == user.ProfileId);
            if (profile == null)
            {
                return true;
            }

            return false;
        }
    }
}
