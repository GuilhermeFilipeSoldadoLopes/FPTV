using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.UserModels.DAL
{
    public class FavTeamsList
    {
        [Key]
        [Display(Name = "Error Log ID")]
        public Guid FavTeamsListId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Team Image")]
        public string TeamImage { get; set; }

        [Required]
        [Display(Name = "User ID")]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Display(Name = "User")]
        public virtual Profile User { get; set; }
    }
}
