using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.NovaVersão.UserModels.DAL
{
    public class UserBase : IdentityUser
    {
        [Required]
        [Display(Name = "Name")]
        [PersonalData]
        public string Name { get; set; }

        [Required]
        [Display(Name = "User ID")]
        [ForeignKey("Profile")]
        public Guid UserId { get; set; }

        [Display(Name = "User")]
        public virtual Profile Profile { get; set; }
    }
}
