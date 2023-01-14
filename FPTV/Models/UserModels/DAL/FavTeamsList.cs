using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.DAL
{
    public class FavTeamsList
    {
        [Key]
        [Display(Name = "Error Log ID")]
        public Guid favTeamsListId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string name { get; set; }

        [Required]
        [Display(Name = "Team Image")]
        public string teamImage { get; set; }

        [Required]
        [Display(Name = "User ID")]
        [ForeignKey("Profile")]
        public Guid userId { get; set; }

        [Required]
        [Display(Name = "User")]
        public Profile? user { get; set; }
    }
}
