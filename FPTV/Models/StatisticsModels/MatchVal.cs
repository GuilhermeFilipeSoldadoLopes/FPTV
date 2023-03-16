using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FPTV.Models.MatchesModels;
using FPTV.Models.UserModels;

namespace FPTV.Models.StatisticsModels
{
    public class MatchVal
    {
        [Required]
        [Key]
        [Display(Name = "Id of valorant match")]
        public Guid MatchValId { get; set; }

        [Required]
        [Display(Name = "MatchVal API ID")]
        public int MatchValAPIID { get; set; }

        [Display(Name = "MatchesVal")]
        public MatchesVal MatchesVal { get; set; }

		[Required]
		[Display(Name = "API Id of a val matches")]
        public int MatchesValAPIId { get; set; }

		[Required]
		[Display(Name = "List of stats of players")] 
        public ICollection<MatchPlayerStatsVal>? PlayerStatsList { get; set; }
        
		[Required]
		[Display(Name = "Score of round")]
        public string? RoundsScore { get; set; }

		[Required]
		[Display(Name = "Map")]
        public string? Map { get; set; }

		[Required]
		[Display(Name = "List of Teams")] 
        public ICollection<MatchTeamsVal>? TeamsList { get; set; }

        [Display(Name = "WinnerTeam")] 
        public Team? WinnerTeam { get; set; }

		[Required]
		[Display(Name = "API Id of the winner team")]
        public int? WinnerTeamAPIId { get; set; }

		[Required]
		[Display(Name = "Name of the winner team")]
        public string? WinnerTeamName { get; set; }
    }
}