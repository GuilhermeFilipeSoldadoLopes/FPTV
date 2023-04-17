using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FPTV.Models.UserModels;

namespace FPTV.Models.Forum
{
    public class Comment
    {
        [Key]
        [Display(Name = "Comment ID")]
        public Guid CommentId { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Comment")]
        public string? Text { get; set; }

        //[Display(Name = "User ID")]
        //[ForeignKey("ProfileId")]
        //public Guid ProfileId { get; set; }

        [Display(Name = "User")]
        public Profile? Profile { get; set; }

        [Display(Name = "Topic")]
        public Topic? Topic { get; set; }

		[Required]
		[Display(Name = "Reactions")]
        public ICollection<Reaction>? Reactions { get; set; }

        [Required]
        [Display(Name = "isReported")]
        public bool Reported { get; set; }
    }
}