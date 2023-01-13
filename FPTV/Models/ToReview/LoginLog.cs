using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.ToReview
{
    public class LoginLog
    {
        [Key]
        [Display(Name = "Login Log ID")]
        public int loginLogId { get; set; }
        
        [Required]
        [Display(Name = "Authentication Type")]
        public string authenticationType { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime date { get; set; }

        [Required]
        [Display(Name = "User Account ID")]
        [ForeignKey("User")]
        public int userAccountId { get; set; }

        [Required]
        [Display(Name = "User")]
        public User? user { get; set; }
    }
}
