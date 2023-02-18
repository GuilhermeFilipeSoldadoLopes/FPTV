using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.StatisticsModels
{
    public class MatchVal
    {
        [Required]
        [Key]
        public Guid MatchValId { get; set; }

        [Required]
        [ForeignKey("MatchesValId")]
        public Guid MatchesValId { get; set; }

        public ICollection<MatchPlayerStatsVal>? PlayerStatsList { get; set; }

        [Required]
        [Display(Name = "Score of round")]
        public string RoundsScore { get; set; }

        [Display(Name = "Map")]
        public string? Map { get; set; }

        public ICollection<MatchTeamsVal> TeamsList { get; set; }

        public Guid? WinnerTeamId { get; set; }
        
        [Display(Name = "Name of the winner team")]
        public string? WinnerTeamName { get; set; }

    }
}
