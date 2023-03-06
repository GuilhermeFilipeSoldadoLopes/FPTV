using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FPTV.Models.UserModels
{
    /// <summary>
    /// This class represents a list of favorite players of a user
    /// </summary>
    public class FavPlayerList
    {
        /// <summary>
        /// ID of the Favorite Players List
        /// </summary>
        [Key]
        [Display(Name = "Fav Player List ID")]
        public Guid FavPlayerListId { get; set; }

        public Player[]? Players { get; set; }

        [ForeignKey("ProfileId")]
        public Guid ProfileId { get; set; }

        /// <summary>
        /// Profile of the user
        /// </summary>
        public virtual Profile Profile { get; set; }
    }
}
