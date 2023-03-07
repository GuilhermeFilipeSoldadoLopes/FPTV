using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.StatisticsModels
{
    /// <summary>
    /// This class will represent a valorant team
    /// </summary>
    public class MatchTeamsVal
    {
        /// <summary>
        /// Id of the team
        /// </summary>
        [Required]
        [Key]
        [Display(Name = "Id of a valorant match")]
        public Guid MatchValId { get; set; }

        /// <summary>
        /// Id of the team return by the api
        /// </summary>
        [Required]
        [ForeignKey("TeamValId")]
        [Display(Name = "Id of a valorant team")]
        public Guid TeamValId { get; set; }

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