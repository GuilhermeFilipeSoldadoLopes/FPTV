using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using FPTV.Models.ToReview;

namespace FPTV.Models.UserModels
{
    /// <summary>
    /// /// <summary>
    /// This class represents a list of favorite teams of a user
    /// </summary>
    /// </summary>
    public class FavTeamsList
    {
        /// <summary>
        /// ID of the Favorite Teams List
        /// </summary>
        [Key]
        [Display(Name = "Fav Player List ID")]
        public Guid FavTeamsListId { get; set; }

        public Team[] Teams { get; set; }

        [ForeignKey("ProfileId")]
        public Guid ProfileId { get; set; }

        /// <summary>
        /// Profile of the user
        /// </summary>
        public virtual Profile Profile { get; set; }
    }
}
