using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.Authentication.DAL
{
    public class Token
    {
        [Key]
        [Display(Name = "ID")]
        public Guid TokenId { get; set; }

        [Required]
        [Display(Name = "TokenString")]
        public string TokenString { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        [MaxLength(250)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartTime { get; set; } = DateTime.Now;

        //A token expira em 2 horas
        [Required]
        [Display(Name = "End Time")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EndTime { get; set; } = DateTime.Now.AddHours(2);

        [Required]
        [Display(Name = "User Account Id")]
        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }

        [Required]
        [Display(Name = "User Account")]
        public virtual UserAccount UserAccount { get; set; }
    }
}
