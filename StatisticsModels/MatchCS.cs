using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FPTV.Models.StatisticsModels
{
    public class MatchCS
    {
        [Required]
        [Key]
        [Display(Name = "Id of a csgo match")]
        public Guid MatchCSId { get; set; }

        [Required]
        [ForeignKey("MatchesCSId")]
        [Display(Name = "Id of a csgo matches")]
        public Guid MatchesCSId { get; set; }

        [Display(Name = "List of stats of players")]
        public ICollection<MatchPlayerStatsCS>? PlayerStatsList { get; set; }

        [Required]
        [Display(Name = "Score of round")]
        public string RoundsScore { get; set; }

        [Display(Name = "Map")]
        public string? Map { get; set; }

        [Display(Name = "List of Teams")] 
        public ICollection<MatchTeamsCS>? TeamsList { get; set; }

        [Display(Name = "Id of the winner team")]
        public Guid? WinnerTeamId { get; set; }

        [Display(Name = "Name of the winner team")]
        public string? WinnerTeamName { get; set; }
    }
}
