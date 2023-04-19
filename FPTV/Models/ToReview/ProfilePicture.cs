using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.ToReview
{
    /// <summary>
    /// This class represents a profile picture for a user.
    /// </summary>
    public class ProfilePicture
    {
        [Key]
        [Display(Name = "Picture ID")]
        public Guid PictureId { get; set; }

        [Display(Name = "Profile Picture")]
        public string? Picture { get; set; }
    }
}
