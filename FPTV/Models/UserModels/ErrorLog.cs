using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.UserModels
{
    /// <summary>
    /// This class represent all the errors of a user
    /// </summary>
    public class ErrorLog
    {

        /// <summary>
        /// ID of the error log
        /// </summary>
        [Key]
        [Display(Name = "Error Log ID")]
        public Guid ErrorLogId { get; set; }

        /// <summary>
        /// Description of the error
        /// </summary>
        [Required]
        [Display(Name = "Error")]
        public string Error { get; set; }

        /// <summary>
        /// Date of the error log
        /// </summary>
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        /// <summary>
        /// ID of the user
        /// </summary>
        [Required]
        [Display(Name = "User ID")]
        [ForeignKey("Profile")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Profile of the user
        /// </summary>
        [Display(Name = "User")]
        public virtual Profile Profile { get; set; }
    }
}
