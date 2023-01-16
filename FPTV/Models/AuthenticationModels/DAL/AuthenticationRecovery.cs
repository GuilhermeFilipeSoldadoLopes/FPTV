using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.Authentication.DAL
{
    public class AuthenticationRecovery
    {
        [Display(Name = "AuthenticationRecovery ID")]
        public Guid AuthenticationRecoveryId { get; set; }

        [Required(ErrorMessage = "Token is required for any change.")]
        [Display(Name = "Token")]
        public string Token { get; set; }

        [Required(ErrorMessage = "New Password is required.")]
        [Display(Name = "NewPassword")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirmation of New Password is required.")]
        [Compare("Password", ErrorMessage = "New Password and Confirmation of New Password must match.")]
        [Display(Name = "ConfirmNewPassword")]
        public string ConfirmNewPassword { get; set; }

        [Required]
        [Display(Name = "User Account Id")]
        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }

        [Display(Name = "User Account")]
        public virtual UserAccount UserAccount { get; set; }
    }
}
