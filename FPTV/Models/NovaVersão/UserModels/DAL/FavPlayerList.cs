using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.NovaVersão.UserModels.DAL
{
    public class FavPlayerList
    {
        [Key]
        [Display(Name = "Fav Player List ID")]
        public Guid FavPlayerListId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Team")]
        public string Team { get; set; }

        [Required]
        [Display(Name = "Player Image")]
        public string PlayerImage { get; set; }

        [Required]
        [Display(Name = "User ID")]
        [ForeignKey("Profile")]
        public Guid UserId { get; set; }

        [Display(Name = "User")]
        public virtual Profile Profile { get; set; }
    }
}
