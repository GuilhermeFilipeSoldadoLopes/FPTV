using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.ToReview
{
    public class User
    {
        [Key]
        [Display(Name = "User ID")]
        public int userId { get; set; }

        [Required]
        [Display(Name = "User Type")]
        public string userType { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string name { get; set; }

        [Required]
        [Display(Name = "Flag")]
        public string flag { get; set; }

        [Required]
        [Display(Name = "Biography")]
        public string biography { get; set; }

        [Required]
        [Display(Name = "Profile Picture")]
        [ForeignKey("ProfilePicture")]
        public string profilePicture { get; set; }

        [Required]
        [Display(Name = "Registration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime registration_Date { get; set; }

        [Required]
        [Display(Name = "Profile Picture")]
        public ProfilePicture? picture { get; set; }




    }
}
