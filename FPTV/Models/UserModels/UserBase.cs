using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.UserModels
{
    /// <summary>
    /// Represents the base class for user identity objects.
    /// </summary>
    public class UserBase : IdentityUser
    {
        public Profile Profile { get; set; }

        [ForeignKey("ProfileId")]
        public Guid ProfileId { get; set; }
    }
}
