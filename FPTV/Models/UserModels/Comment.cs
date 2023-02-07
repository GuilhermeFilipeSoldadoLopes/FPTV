using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.UserModels
{
    /// <summary>
    /// This class represent a comment from a topic
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// ID of the comment
        /// </summary>
        [Display(Name = "Comment ID")]
        public Guid CommentId { get; set; }

        /// <summary>
        /// Date of comment
        /// </summary>
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Comment the user writes
        /// </summary>
        [Required]
        [Display(Name = "Comment")]
        public string Text { get; set; }

        /// <summary>
        /// ID of the user
        /// </summary>
        [Required]
        [Display(Name = "User ID")]
        //[ForeignKey("User")]
        public Guid UserId { get; set; }

        /// <summary>
        /// ID of the topic
        /// </summary>
        [Required]
        [Display(Name = "Topic ID")]
        //[ForeignKey("Topic")] 
        public Guid TopicId { get; set; }

        /// <summary>
        /// Profile of the user
        /// </summary>
        [Display(Name = "User")]
        public virtual Profile Profile { get; set; }

        /// <summary>
        /// Topic in which the comment is published 
        /// </summary>
        [Display(Name = "Topic")]
        public virtual Topic Topic { get; set; }

        /// <summary>
        /// Reactions made to the comment
        /// </summary>
        public ICollection<Reaction> Reactions { get; set; }
    }
}