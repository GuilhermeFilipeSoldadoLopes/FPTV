using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.UserModels
{
    /// <summary>
    /// This class represents the profile user of an user
    /// </summary>
    public class ProfilePicture
    {
        /// <summary>
        /// ID of the picture
        /// </summary>
        [Key]
        [Display(Name = "Picture ID")]
        public Guid PictureId { get; set; }

        /// <summary>
        /// Profile Picture
        /// </summary>
        [Display(Name = "Profile Picture")]
        public string Picture { get; set; }
    }
}
