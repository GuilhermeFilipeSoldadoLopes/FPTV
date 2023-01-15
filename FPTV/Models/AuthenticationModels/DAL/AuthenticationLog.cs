using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;

namespace FPTV.Models.Authentication.DAL
{
    public class AuthenticationLog
    {
        [Display(Name = "AuthenticationLog ID")]
        public Guid AuthenticationLogId { get; set; }

        [Required]
        [Display(Name = "Authentication Type")]
        [EnumDataType(typeof(AuthenticationType))]
        public string AuthenticationType { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "User Account Id")]
        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }

        [Display(Name = "User Account")]
        public virtual UserAccount UserAccount { get; set; }
    }
}
