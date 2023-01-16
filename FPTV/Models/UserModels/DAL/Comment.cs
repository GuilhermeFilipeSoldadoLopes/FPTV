using Humanizer;

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.UserModels.DAL
{
    public class Comment
    {
       
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

        [Required]
        [Display(Name = "User ID")]
        //[ForeignKey("User")]
        public Guid UserId { get; set; }

        
        [Required]
        [Display(Name = "Topic ID")]
        //[ForeignKey("Topic")] 
        public Guid TopicId { get; set; }

        [Display(Name = "User")]
        public virtual Profile User { get; set; }

        
        [Display(Name = "Topic")]
        public virtual Topic Topic { get; set; }

        public ICollection<Reaction> Reactions { get; set; }
    }
}