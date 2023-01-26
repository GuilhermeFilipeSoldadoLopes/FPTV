using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.UserModels
{
    public class UserBase : IdentityUser
    {
        [Display(Name = "Flag")]
        public string? Flag { get; set; }

        [Display(Name = "Biography")]
        public string? Biography { get; set; }

        [Required]
        [Display(Name = "Registration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RegistrationDate { get; set; }
        [Display(Name = "Profile Picture")]
        public virtual byte[]? Picture { get; set; }

        [Display(Name = "Country")]
        public string? Country { get; set; }
    }
}
