using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FPTV.Models.UserModels;

namespace FPTV.Models.Forum
{
    public class Topic
    {
        [Display(Name = "Topic ID")]
        public Guid TopicId { get; set; }

        [Required]
        [Display(Name = "GameType")]
        public GameType? GameType { get; set; }

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
		public ICollection<Comment>? Comments { get; set; }
	}
}
