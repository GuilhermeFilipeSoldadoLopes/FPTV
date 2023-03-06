using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.UserModels
{
    /// <summary>
    /// This class represents a topic of the forum
    /// </summary>
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
        /// ID of the profile
        /// </summary>
        [Display(Name = "User ID")]
        [ForeignKey("ProfileId")]
        public Guid ProfileId { get; set; }

        /// <summary>
        /// Profile of the user
        /// </summary>
        [Display(Name = "User")]
        public virtual Profile Profile { get; set; }
    }
}
