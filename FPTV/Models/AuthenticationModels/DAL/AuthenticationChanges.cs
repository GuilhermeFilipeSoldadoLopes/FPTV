using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.Authentication.DAL
{
    public class AuthenticationChanges
    {
        [Key]
        [Display(Name = "ID")]
        public Guid AuthenticationChangesId { get; set; }

        [Required(ErrorMessage = "Token is required for any change.")]
        public string token { get; set; }

        [EmailAddress]
        public string email
        {
            get => email;
            set
            {
                email = value;
                changed = "email";
            }
        }

        public string password
        {
            get => password;
            set
            {
                password = value;
                changed = "password";
            }
        }

        public string changed { get; set; }

        [Required]
        [Display(Name = "User Account Id")]
        [ForeignKey("UserAccount")]
        public Guid userAccountID { get; set; }

        [Required]
        [Display(Name = "User Account")]
        public UserAccount? userAccount { get; set; }
    }
}
