using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.UserModels
{
    /// <summary>
    /// This class represents a user profile.
    /// </summary>
    public class Profile
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        public UserBase User { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }

        [Display(Name = "Biography")]
        public string? Biography { get; set; }

        [Required]
        [Display(Name = "Registration Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Profile Picture")]
        public virtual byte[]? Picture { get; set; }

        [Display(Name = "Favorite Players List")]
        public virtual FavPlayerList? PlayerList { get; set; }

        [Display(Name = "Favorite Teams List")]
        public virtual FavTeamsList? TeamsList { get; set; }

        [Display(Name = "Country")]
        public string? Country { get; set; }

        [Display(Name = "Flag")]
        public string? Flag { get; set; }
    }
}
