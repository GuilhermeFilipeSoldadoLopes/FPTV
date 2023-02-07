using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Display(Name = "Error Log ID")]
        public Guid FavTeamsListId { get; set; }

        /// <summary>
        /// Name of the team
        /// </summary>
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Which game is it CS:GO or Valorant
        /// </summary>
        [Required]
        [Display(Name = "isCSGO")]
        public bool isGame { get; set; }

        /// <summary>
        /// Image of the team
        /// </summary>
        [Required]
        [Display(Name = "Team Image")]
        public string TeamImage { get; set; }

        /// <summary>
        /// ID of the user
        /// </summary>
        [Required]
        [Display(Name = "User ID")]
        [ForeignKey("Profile")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Profile of the user
        /// </summary>
        [Display(Name = "User")]
        public virtual Profile Profile { get; set; }
    }
}
