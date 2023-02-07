using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.UserModels
{
    /// <summary>
    /// This class is inherit from class IdentityUser and it represents a User and his identity
    /// </summary>
    public class UserBase : IdentityUser
    {
        /// <summary>
        /// Name of the userbase
        /// </summary>
        [Required]
        [Display(Name = "Name")]
        [PersonalData]
        public string Name { get; set; }

        /// <summary>
        /// ID of the user
        /// </summary>
        [Required]
        [Display(Name = "User ID")]
        [ForeignKey("Profile")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Profile of the user
        /// </summary>
        [Display(Name = "User")]
        public virtual Profile Profile { get; set; }
    }
}
