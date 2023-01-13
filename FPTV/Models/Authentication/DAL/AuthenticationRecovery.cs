using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.Authentication.DAL
{
    public class AuthenticationRecovery
    {
        [Required(ErrorMessage = "New Password is required.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirmation of New Password is required.")]
        [Compare("Password", ErrorMessage = "New Password and Confirmation of New Password must match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
