using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.DAL
{
    public class ProfilePicture
    {
        [Key]
        [Display(Name = "Picture ID")]
        public Guid PictureId { get; set; }

        [Required]
        [Display(Name = "Profile Picture")]
        public string Picture { get; set; }
    }
}
