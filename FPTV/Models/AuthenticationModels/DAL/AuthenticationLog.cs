using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;

namespace FPTV.Models.Authentication.DAL
{
    public class AuthenticationLog
    {
        [Key]
        [Display(Name = "ID")]
        public Guid AuthenticationLogId { get; set; }

        [Required]
        [Display(Name = "Authentication Type")]
        [EnumDataType(typeof(AuthenticationTypes))]
        public string authenticationType { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime date { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "User Account Id")]
        [ForeignKey("UserAccount")]
        public Guid userAccountID { get; set; }

        [Required]
        [Display(Name = "User Account")]
        public UserAccount? userAccount { get; set; }
    }
}
