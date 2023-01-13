using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.ToReview
{
    public class FavTeamsList
    {
        [Key]
        [Display(Name = "Error Log ID")]
        public int favTeamsListId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string name { get; set; }

        [Required]
        [Display(Name = "Team Image")]
        public string teamImage { get; set; }

        [Required]
        [Display(Name = "User ID")]
        [ForeignKey("User")]
        public int userId { get; set; }

        [Required]
        [Display(Name = "User")]
        public User? user { get; set; }
    }
}
