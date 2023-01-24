using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.AntigaVersão.UserModels.DAL
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
