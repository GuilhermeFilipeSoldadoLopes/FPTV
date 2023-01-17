using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.AntigaVersão.AuthenticationModels.DAL
{
    public class Mail
    {
        [Display(Name = "Mail ID")]
        public Guid MailId { get; set; }

        [Required]
        [Display(Name = "Message")]
        [MaxLength(500)]
        public string Message { get; set; }

        [Required]
        [Display(Name = "Sender Mail")]
        [EmailAddress]
        [MaxLength(250)]
        public string SenderMail { get; set; }

        [Required]
        [Display(Name = "Receiver Mail")]
        [EmailAddress]
        [MaxLength(250)]
        public string ReceiverMail { get; set; }

        [Required]
        [Display(Name = "Sent Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime SentDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "User Account Id")]
        [ForeignKey("UserAccount")]
        public Guid UserAccountId { get; set; }

        [Display(Name = "User Account")]
        public virtual UserAccount UserAccount { get; set; }
    }
}
