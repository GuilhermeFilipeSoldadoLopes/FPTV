using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.ToReview
{
    public class ErrorLog
    {
        [Key]
        [Display(Name = "Error Log ID")]
        public int errorLogId { get; set; }

        [Required]
        [Display(Name = "Error")]
        public string error { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime date { get; set; }

        [Required]
        [Display(Name = "User ID")]
        [ForeignKey("User")]
        public int userId { get; set; }

        [Required]
        [Display(Name = "User")]
        public User? user { get; set; }
    }
}
