using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.Authentication.DAL
{
    public class AuthenticationRecovery
    {
        [Key]
        [Display(Name = "ID")]
        public Guid AuthenticationRecoveryId { get; set; }

        [Required(ErrorMessage = "Token is required for any change.")]
        public string token { get; set; }

        [Required(ErrorMessage = "New Password is required.")]
        public string newPassword { get; set; }

        [Required(ErrorMessage = "Confirmation of New Password is required.")]
        [Compare("Password", ErrorMessage = "New Password and Confirmation of New Password must match.")]
        public string confirmNewPassword { get; set; }

        [Required]
        [Display(Name = "User Account Id")]
        [ForeignKey("UserAccount")]
        public Guid userAccountID { get; set; }

        [Required]
        [Display(Name = "User Account")]
        public UserAccount? userAccount { get; set; }
    }
}
