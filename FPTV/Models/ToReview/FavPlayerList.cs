using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.ToReview
{
    public class FavPlayerList
    {
        [Key]
        [Display(Name = "Fav Player List ID")]
        public int favPlayerListId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string name { get; set; }

        [Required]
        [Display(Name = "Team")]
        public string team { get; set; }

        [Required]
        [Display(Name = "Player Image")]
        [ForeignKey("Picture")]
        public string playerImage { get; set; }

        [Required]
        [Display(Name = "User ID")]
        [ForeignKey("User")]
        public int userId { get; set; }

        [Required]
        [Display(Name = "User")]
        public User? user { get; set; }
    }
}
