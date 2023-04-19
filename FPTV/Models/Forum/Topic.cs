using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FPTV.Models.UserModels;

namespace FPTV.Models.Forum
{
    /// <summary>
    /// This class represents a topic that can be discussed.
    /// </summary>
    public class Topic
    {
        [Key]
        [Display(Name = "Topic ID")]
        public int TopicId { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string? Title { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string? Content { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        [Display(Name = "User ID")]
        [ForeignKey("ProfileId")]
        public Guid ProfileId { get; set; }

        [Display(Name = "User")]
        public Profile? Profile { get; set; }

        [Required]
        [Display(Name = "Comments")]
        public ICollection<Comment> Comments { get; set; }

        [Required]
        [Display(Name = "isReported")]
        public bool Reported { get; set; }
    }
}
