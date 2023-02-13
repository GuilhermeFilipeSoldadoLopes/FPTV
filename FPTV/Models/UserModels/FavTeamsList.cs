using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.UserModels
{
    public class FavTeamsList
    {
        [Key]
        [Display(Name = "Fav Player List ID")]
        public Guid FavTeamsListId { get; set; }
        public Team[] Teams { get; set; }
        [ForeignKey("Profile")]
        public Guid ProfileId { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
