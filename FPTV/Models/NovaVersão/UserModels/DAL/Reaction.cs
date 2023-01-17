using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.NovaVersão.UserModels.DAL
{
    public class Reaction
    {
        [Display(Name = "Reaction ID")]
        public Guid ReactionId { get; set; }

        [Required]
        [Display(Name = "Reaction")]
        public string ReactionCode { get; set; } //rever o nome reactioncode //ReactionEmoji - o que achas?

        [Required]
        [Display(Name = "User Id")]
        //[ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required]
        [Display(Name = "Comment ID")]
        //[ForeignKey("Comment")]
        public Guid CommentId { get; set; }

        [Display(Name = "User")]
        public virtual Profile User { get; set; }

        [Display(Name = "Comment")]
        public virtual Comment Comment { get; set; }
    }
}
