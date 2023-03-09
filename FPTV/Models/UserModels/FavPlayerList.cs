using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FPTV.Models.UserModels
{
    public class FavPlayerList
    {
        [Key]
        [Display(Name = "Fav Player List ID")]
        public Guid FavPlayerListId { get; set; }

		[Display(Name = "Fav Players List")]
		public ICollection<Player>? Players { get; set; }

		[ForeignKey("ProfileId")]
		[Display(Name = "User ProfileID")]
		public Guid? ProfileId { get; set; }

		[Display(Name = "User Profile")]
		public virtual Profile? Profile { get; set; }
	}
}