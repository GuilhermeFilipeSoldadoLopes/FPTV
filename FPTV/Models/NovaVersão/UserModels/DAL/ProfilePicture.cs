using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.NovaVersão.UserModels.DAL
{
    public class ProfilePicture
    {
        [Key]
        [Display(Name = "Picture ID")]
        public Guid PictureId { get; set; }

        [Display(Name = "Profile Picture")]
        public string Picture { get; set; }
    }
}
