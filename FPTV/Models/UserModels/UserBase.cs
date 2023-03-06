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
        /// Profile of the user
        /// </summary>
        public Profile Profile { get; set; }

        /// <summary>
        /// ID of the profile
        /// </summary>
        [ForeignKey("ProfileId")]
        public Guid ProfileId { get; set; }
    }
}
