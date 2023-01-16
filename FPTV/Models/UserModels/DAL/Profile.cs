using FPTV.Models.Authentication.DAL;
using FPTV.Models.UserModels.DAL;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.DAL
{
    public class Profile
    {
        [Key]
        [Display(Name = "User ID")]
        public Guid UserId { get; set; }

        [Required]
        [Display(Name = "User Type")]
        [EnumDataType(typeof(UserType))]
        public UserType UserType { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Flag")]
        public string Flag { get; set; }

        [Required]
        [Display(Name = "Biography")]
        public string Biography { get; set; }

        [Required]
        [Display(Name = "Registration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RegistrationDate { get; set; }

        [Required]
        [Display(Name = "Profile Picture")]
        [ForeignKey("ProfilePicture")]
        public Guid ProfilePictureId { get; set; }

        [Display(Name = "Profile Picture")]
        public virtual ProfilePicture Picture { get; set; }
    }
}
