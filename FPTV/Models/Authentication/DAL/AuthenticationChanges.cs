using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.Authentication.DAL
{
    public class AuthenticationChanges
    {
        [Required(ErrorMessage = "Token is required for any change.")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Confirmation Email is required.")]
        [EmailAddress]
        [Compare("Email", ErrorMessage = "Email and Confirmation Email must match.")]
        public string ConfirmEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirmation Password is required.")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }


    }
}
