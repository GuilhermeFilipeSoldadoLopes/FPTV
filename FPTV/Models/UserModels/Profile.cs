using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace FPTV.Models.UserModels
{
    /// <summary>
    /// This class represents a profile od an user
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// ID of the user
        /// </summary>
        [Key]
        [Display(Name = "User ID")]
        public Guid UserId { get; set; }

        /*[Required]
        [Display(Name = "User Type")]
        [EnumDataType(typeof(UserType))]
        public UserType UserType { get; set; }*/

        /// <summary>
        /// Name of the profile
        /// </summary>
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Flag of the profile
        /// </summary>
        [Display(Name = "Flag")]
        public string Flag { get; set; }

        /// <summary>
        /// Biography(description) of the profile
        /// </summary>
        [Display(Name = "Biography")]
        public string Biography { get; set; }

        /// <summary>
        /// Date of the registration
        /// </summary>
        [Required]
        [Display(Name = "Registration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// ID of the Profile Picture
        /// </summary>
        [Required]
        [Display(Name = "Profile Picture")]
        [ForeignKey("ProfilePicture")]
        public Guid ProfilePictureId { get; set; }

        /// <summary>
        /// Picture of the Profile Picture
        /// </summary>
        [Display(Name = "Profile Picture")]
        public virtual ProfilePicture Picture { get; set; }
    }
}
