using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.StatisticsModels
{
    public class MatchTeamsVal
    {
        [Required]
        [Key]
        public Guid MatchValId { get; set; }

        [Required]
        [ForeignKey("TeamValId")] 
        public Guid TeamValId { get; set; }

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