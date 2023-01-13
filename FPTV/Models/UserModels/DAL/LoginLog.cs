using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.DAL
{
    public class LoginLog
    {
        [Key]
        [Display(Name = "Login Log ID")]
        public Guid loginLogId { get; set; }

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
        public Guid userAccountId { get; set; }

        [Required]
        [Display(Name = "User")]
        public Profile? user { get; set; }
    }
}
