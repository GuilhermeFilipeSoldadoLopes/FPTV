using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.UserModels
{
    /// <summary>
    /// This class represent a reaction from a comment
    /// </summary>
    public class Reaction
    {
        /// <summary>
        /// ID of the reaction
        /// </summary>
        [Display(Name = "Reaction ID")]
        public Guid ReactionId { get; set; }

        /// <summary>
        /// Emoji used in the reaction
        /// </summary>
        [Required]
        [Display(Name = "Reaction")]
        public string ReactionCode { get; set; } 

        /// <summary>
        /// ID of the user
        /// </summary>
        [Required]
        [Display(Name = "User Id")]
        //[ForeignKey("User")]
        public Guid UserId { get; set; }

        /// <summary>
        /// ID of the comment
        /// </summary>
        [Required]
        [Display(Name = "Comment ID")]
        //[ForeignKey("Comment")]
        public Guid CommentId { get; set; }

        /// <summary>
        /// Profile of the user who made the reaction
        /// </summary>
        [Display(Name = "User")]
        public virtual Profile Profile { get; set; }

        /// <summary>
        /// Comment to which the reaction was made
        /// </summary>
        [Display(Name = "Comment")]
        public virtual Comment Comment { get; set; }
    }
}
