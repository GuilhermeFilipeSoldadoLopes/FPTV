using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.UserModels
{
    public class FavPlayerList
    {
        /// <summary>
        /// ID of the Favorite Players List
        /// </summary>
        [Key]
        [Display(Name = "Fav Player List ID")]
        public Guid FavPlayerListId { get; set; }

        /// <summary>
        /// Name of the player
        /// </summary>
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Team of the player
        /// </summary>
        [Required]
        [Display(Name = "Team")]
        public string Team { get; set; }

        /// <summary>
        /// Which game is it CS:GO or Valorant
        /// </summary>
        [Required]
        [Display(Name = "isCSGO")]
        public bool isGame { get; set; }

        /// <summary>
        /// Image of the player
        /// </summary>
        [Required]
        [Display(Name = "Player Image")]
        public string PlayerImage { get; set; }

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
