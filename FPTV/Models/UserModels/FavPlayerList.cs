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
        public Player[]? Players { get; set; }
        [ForeignKey("ProfileId")]
        public Guid ProfileId { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
