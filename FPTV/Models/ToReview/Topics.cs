using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FPTV.Models.ToReview
{
    public class Topics
    {
        [Key]
        [Display(Name = "Topics ID")]
        public int topicsId { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string title { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string content { get; set; }

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
