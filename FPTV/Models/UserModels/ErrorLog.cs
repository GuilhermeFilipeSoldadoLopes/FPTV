using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.UserModels
{
    

     /// <summary>
     /// This class is used to log errors in the application.
     /// </summary>
     public class ErrorLog
     {
        [Key]
        [Display(Name = "Error Log ID")]
        public Guid ErrorLogId { get; set; }

        //Descrição do erro
        [Required]
        [Display(Name = "Error")]
        public string? Error { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        [Display(Name = "User ID")]
        [ForeignKey("Profile")]
        public Guid? UserId { get; set; }

        [Display(Name = "User")]
        /// <summary>
        /// Gets or sets the Profile associated with this object.
        /// </summary>
        public virtual Profile? Profile { get; set; }
    }
}
