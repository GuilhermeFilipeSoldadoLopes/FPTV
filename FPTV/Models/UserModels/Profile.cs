using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

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
        [Required]
        [Key]
        public Guid Id { get; set; }

        public UserBase User { get; set; }

        /// <summary>
        /// ID of the user
        /// </summary>
        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Biography(description) of the profile
        /// </summary>
        [Display(Name = "Biography")]
        public string? Biography { get; set; }

        /// <summary>
        /// Date of the registration
        /// </summary>
        [Required]
        [Display(Name = "Registration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RegistrationDate { get; set; }

        /// <summary>
        /// Picture of the Profile Picture
        /// </summary>
        [Display(Name = "Profile Picture")]
        public virtual byte[]? Picture { get; set; }

        public FavPlayerList? PlayerList { get; set; }

        public FavTeamsList? TeamsList { get; set; }

        [Display(Name = "Country")]
        public string? Country { get; set; }
    }
}
