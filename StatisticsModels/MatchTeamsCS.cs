using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FPTV.Models.StatisticsModels
{
    public class MatchTeamsCS
    {
        [Required]
        [Key]
        public Guid MatchCSId { get; set; }

        [Required]
        [ForeignKey("TeamCSId")]
        public Guid TeamCSId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "Image")]
        public string Image { get; set; }
    }
}