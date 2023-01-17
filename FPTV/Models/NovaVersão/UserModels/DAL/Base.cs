using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FPTV.Models.NovaVersão.UserModels.DAL
{
    public class Base : IdentityUser
    {
        [Required]
        [Display(Name = "Name")]
        [PersonalData]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Name")]
        [PersonalData]
        public Profile Profile { get; set; }
    }
}
