using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.UserModels
{
    public class FavTeamsList
    {
        [Key]
        [Display(Name = "Fav Teams List ID")]
        public Guid FavTeamsListId { get; set; }

		[Display(Name = "Fav Teams List")]
		public ICollection<Team>? Teams { get; set; }

        [ForeignKey("ProfileId")]
		[Display(Name = "User ProfileID")]
		public Guid? ProfileId { get; set; }

		[Display(Name = "User Profile")]
		public virtual Profile? Profile { get; set; }
    }
}