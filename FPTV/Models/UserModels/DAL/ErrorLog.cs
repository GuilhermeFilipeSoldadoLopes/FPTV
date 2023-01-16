using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.UserModels.DAL
{
    public class ErrorLog
    {
        [Key]
        [Display(Name = "Error Log ID")]
        public Guid ErrorLogId { get; set; }

        [Required]
        [Display(Name = "Error")]
        public string Error { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "User ID")]
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Display(Name = "User")]
        public virtual Profile User { get; set; }
    }
}
