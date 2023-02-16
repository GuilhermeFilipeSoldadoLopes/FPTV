using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.UserModels
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
        public string Text { get; set; }

        [Display(Name = "User ID")]
        [ForeignKey("ProfileId")]
        public Guid ProfileId { get; set; }


        [Display(Name = "Topic ID")]
        [ForeignKey("TopicId")] 
        public Guid TopicId { get; set; }

        [Display(Name = "User")]
        public virtual Profile Profile { get; set; }


        [Display(Name = "Topic")]
        public virtual Topic Topic { get; set; }

        public ICollection<Reaction> Reactions { get; set; }
    }
}