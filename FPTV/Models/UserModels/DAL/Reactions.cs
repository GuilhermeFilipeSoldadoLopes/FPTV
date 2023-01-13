using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.DAL
{
    public class Reactions
    {
        [Key]
        [Display(Name = "Reactions ID")]
        public Guid reactionsId { get; set; }

        [Required]
        [Display(Name = "Reaction")]
        public string reaction { get; set; }

        [Required]
        [Display(Name = "User Id")]
        [ForeignKey("User")]
        public Guid userId { get; set; }

        [Required]
        [Display(Name = "Comment ID")]
        [ForeignKey("Comments")]
        public Guid commentId { get; set; }

        [Required]
        [Display(Name = "User")]
        public Profile? user { get; set; }

        [Required]
        [Display(Name = "Comments")]
        public Comment? comment { get; set; }
    }
}
