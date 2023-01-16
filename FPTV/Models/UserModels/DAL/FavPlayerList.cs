using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.UserModels.DAL
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
        [ForeignKey("User")]
        public Guid UserId { get; set; }


        [Display(Name = "User")]
        public virtual Profile User { get; set; }
    }
}
