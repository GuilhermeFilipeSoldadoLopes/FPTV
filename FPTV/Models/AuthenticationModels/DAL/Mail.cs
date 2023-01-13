using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FPTV.Models.Authentication.DAL
{
    public class Mail
    {
        [Key]
        [Display(Name = "ID")]
        public Guid mailId { get; set; }

        [Required]
        [Display(Name = "Message")]
        [MaxLength(500)]
        public string message { get; set; }

        [Required]
        [Display(Name = "Sender Mail")]
        [EmailAddress]
        [MaxLength(250)]
        public string senderMail { get; set; }

        [Required]
        [Display(Name = "Receiver Mail")]
        [EmailAddress]
        [MaxLength(250)]
        public string receiverMail { get; set; }

        [Required]
        [Display(Name = "Sent Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime sendedDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "User Account Id")]
        [ForeignKey("UserAccount")]
        public Guid userAccountID { get; set; }

        [Required]
        [Display(Name = "User Account")]
        public UserAccount? userAccount { get; set; }
    }
}
