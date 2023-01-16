using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.AuthenticationModels.DAL
{
    public class AuthenticationChange
    {
        [Display(Name = "AuthenticationChange ID")]
        public Guid AuthenticationChangeId { get; set; }

        [Required(ErrorMessage = "Token is required for any change.")]
        public string Token { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email
        {
            get => Email;
            set
            {
                Email = value;
                Changed = "email";
            }
        }

        [Display(Name = "Password")]
        public string Password
        {
            get => Password;
            set
            {
                Password = value;
                Changed = "password";
            }
        }

        [Required]
        [Display(Name = "Changed")]
        public string Changed { get; set; }

        [Required]
        [Display(Name = "User Account Id")]
        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }

        [Display(Name = "User Account")]
        public virtual UserAccount UserAccount { get; set; }
    }
}
