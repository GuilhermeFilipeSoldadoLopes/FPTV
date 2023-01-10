using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.Authentication
{
    public class UserAccount
    {
        [Key]
        [Display(Name = "ID")]
        public Guid userAccountID { get; set; }

        [Required]
        [Display(Name = "Authentication Type")]
        [EnumDataType(typeof(AuthenticationTypes))]
        public string authenticationType { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        [MaxLength(250)]
        public string email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [MaxLength(25)]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        [Display(Name = "User Id")]
        [ForeignKey("User")]
        public Guid userId { get; set; }

        [Required]
        [Display(Name = "User")]
        public User? user { get; set; }
    }
}
