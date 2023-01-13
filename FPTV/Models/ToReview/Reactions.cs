using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.ToReview
{
    public class Reactions
    {
        [Key]
        [Display(Name = "Reactions ID")]
        public int reactionsId { get; set; }

        [Required]
        [Display(Name = "Reaction")]
        public string reaction { get; set; }
        
        [Required]
        [Display(Name = "User Id")]
        [ForeignKey("User")]
        public int userId { get; set; }

        [Required]
        [Display(Name = "Comment ID")]
        [ForeignKey("Comments")]
        public int commentId { get; set; }

        [Required]
        [Display(Name = "User")]
        public User? user { get; set; }

        [Required]
        [Display(Name = "Comments")]
        public Comment? comment { get; set; }
    }
}
