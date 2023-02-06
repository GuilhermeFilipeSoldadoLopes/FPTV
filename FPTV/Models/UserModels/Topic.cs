using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.UserModels
{
    public class Topic
    {
        /// <summary>
        /// ID of the topic
        /// </summary>
        [Display(Name = "Topic ID")]
        public Guid TopicId { get; set; }

        /// <summary>
        /// Title of the topic
        /// </summary>
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        /// <summary>
        /// Content of the topic
        /// </summary>
        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        /// <summary>
        /// Date the topic was created
        /// </summary>
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        /// <summary>
        /// ID of the user
        /// </summary>
        [Required]
        [Display(Name = "User ID")]
        //[ForeignKey("User")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Profile of the user
        /// </summary>
        [Display(Name = "User")]
        public virtual Profile Profile { get; set; }

        /// <summary>
        /// List of Comments made to this topic
        /// </summary>
        public ICollection<Comment> Comments { get; set; }
    }
}
