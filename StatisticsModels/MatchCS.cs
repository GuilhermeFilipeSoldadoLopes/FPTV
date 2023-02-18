using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FPTV.Models.StatisticsModels
{
    public class MatchCS
    {
        [Required]
        [Key]
        public Guid MatchCSId { get; set; }

        [Required]
        [ForeignKey("MatchesCSId")]
        public Guid MatchesCSId { get; set; }

        public ICollection<MatchPlayerStatsCS>? PlayerStatsList { get; set; }

        [Required]
        [Display(Name = "Score of round")]
        public string RoundsScore { get; set; }

        [Display(Name = "Map")]
        public string? Map { get; set; }

        public ICollection<MatchTeamsCS> TeamsList { get; set; }

        public Guid? WinnerTeamId { get; set; }

        [Display(Name = "Name of the winner team")]
        public string? WinnerTeamName { get; set; }
    }
}
