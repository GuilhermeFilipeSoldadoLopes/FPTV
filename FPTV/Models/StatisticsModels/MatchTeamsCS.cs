using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FPTV.Models.StatisticsModels
{
    /// <summary>
    /// This class will represent a cs:go team
    /// </summary>
    public class MatchTeamsCS
    {
        /// <summary>
        /// Id of the team
        /// </summary>
        [Required]
        [Key]
        [Display(Name = "Id of a csgo match")]
        public Guid MatchCSId { get; set; }

        /// <summary>
        /// Id of the team return by the api
        /// </summary>
        [Required]
        [ForeignKey("TeamCSId")]
        [Display(Name = "Id of a csgo team")]
        public Guid TeamCSId { get; set; }

        /// <summary>
        /// Name of the team
        /// </summary>
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Location of the team
        /// </summary>
        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        /// <summary>
        /// Image of the team
        /// </summary>
        [Required]
        [Display(Name = "Image")]
        public string Image { get; set; }
    }
}