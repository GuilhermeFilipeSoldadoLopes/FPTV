using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.DAL
{
    public class FavPlayerList
    {
        [Key]
        [Display(Name = "Fav Player List ID")]
        public Guid favPlayerListId { get; set; }

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
        [ForeignKey("Profile")]
        public Guid userId { get; set; }

        [Required]
        [Display(Name = "User")]
        public Profile? user { get; set; }
    }
}
