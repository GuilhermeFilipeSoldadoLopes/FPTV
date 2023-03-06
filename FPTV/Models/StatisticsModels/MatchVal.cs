using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.StatisticsModels
{
    public class MatchVal
    {
        [Required]
        [Key]
        [Display(Name = "Id of valorant match")]
        public Guid MatchValId { get; set; }

        [Required]
        [Display(Name = "Match API ID")]
        public int MatchAPIID { get; set; }

		[Required]
        [ForeignKey("MatchesValId")]
        [Display(Name = "Id of valorant matches")]
        public Guid MatchesValId { get; set; }

        [Display(Name = "List of stats of players")] 
        public ICollection<MatchPlayerStatsVal>? PlayerStatsList { get; set; }

        [Required]
        [Display(Name = "Score of round")]
        public string RoundsScore { get; set; }

        [Display(Name = "Map")]
        public string? Map { get; set; }

        [Display(Name = "List of Teams")] 
        public ICollection<MatchTeamsVal>? TeamsList { get; set; }

        [Display(Name = "Id of the winner team")] 
        public int? WinnerTeamId { get; set; }
        
        [Display(Name = "Name of the winner team")]
        public string? WinnerTeamName { get; set; }

    }
}
