using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.DAL
{
    public class Comment
    {
        [Key]
        [Display(Name = "Comment ID")]
        public Guid commentId { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime date { get; set; }

        [Required]
        [Display(Name = "Comment")]
        public string comment { get; set; }

        [Required]
        [Display(Name = "Topic ID")]
        [ForeignKey("Topic")]
        public Guid topicsId { get; set; }

        [Required]
        [Display(Name = "User ID")]
        [ForeignKey("User")]
        public Guid userId { get; set; }

        [Required]
        [Display(Name = "User")]
        public User? user { get; set; }

        [Required]
        [Display(Name = "Topic")]
        public Topic? topic { get; set; }
    }
}
