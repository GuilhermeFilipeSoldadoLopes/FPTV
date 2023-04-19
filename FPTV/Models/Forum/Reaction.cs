using System.ComponentModel.DataAnnotations;
using FPTV.Models.UserModels;

namespace FPTV.Models.Forum
{
    /// <summary>
    /// Represents a reaction to a post or comment.
    /// </summary>
    public class Reaction
    {
        [Display(Name = "Reaction ID")]
        public Guid ReactionId { get; set; }

        [Required]
        [Display(Name = "Reaction")]
        public ReactionType? ReactionEmoji { get; set; }

        //[Display(Name = "User ID")]
        //[ForeignKey("ProfileId")]
        //public Guid ProfileId { get; set; }

        [Display(Name = "User")]
        public Profile? Profile { get; set; }

        [Display(Name = "Comment")]
        public Comment? Comment { get; set; }
    }
}
