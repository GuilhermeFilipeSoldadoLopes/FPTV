using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.ToReview
{
    public class ProfilePicture
    {
        [Key]
        [Display(Name = "Picture ID")]
        public int picture_Id { get; set; }

        [Required]
        [Display(Name = "Profile Picture")]
        public string profilePicture { get; set; }
    }
}
