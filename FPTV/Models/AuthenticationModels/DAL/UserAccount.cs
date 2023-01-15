using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FPTV.Models.DAL;

namespace FPTV.Models.Authentication.DAL
{
    public class UserAccount
    {
        [Key]
        [Display(Name = "ID")]
        public Guid UserAccountId { get; set; }

        [Required]
        [Display(Name = "Authentication Type")]
        [EnumDataType(typeof(AuthenticationType))]
        public AuthenticationType AuthenticationType { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        [MaxLength(250)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [MaxLength(25)]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Validated")]
        public bool Validated { get; set; } = false;

        [Required]
        [Display(Name = "User Id")]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required]
        [Display(Name = "User")]
        public virtual Profile User { get; set; }

        [Required]
        [Display(Name = "Authentication Changes")]
        private AuthenticationChange _changes = new AuthenticationChange();

        [Required]
        [Display(Name = "Authentication Log")]
        private AuthenticationLog _log = new AuthenticationLog();

        [Required]
        [Display(Name = "Authentication Recovery")]
        private AuthenticationRecovery _recovery = new AuthenticationRecovery();
    }
}
