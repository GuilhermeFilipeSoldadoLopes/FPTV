using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.UserModels.DAL
{
    public class Topic
    {

        [Display(Name = "Topic ID")]
        public Guid TopicId { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "User ID")]
        //[ForeignKey("User")]
        public Guid UserId { get; set; }

        [Display(Name = "User")]
        public virtual Profile User { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
