using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FPTV.Models.MatchesModels;
using FPTV.Models.UserModels;

namespace FPTV.Models.StatisticsModels
{
    public class MatchCS
    {
        [Required]
        [Key]
        [Display(Name = "Id of a csgo match")]
        public Guid MatchCSId { get; set; }

        [Required]
        [Display(Name = "MatchCS API ID")]
        public int MatchCSAPIID { get; set; }

        [Required]
        [Display(Name = "MatchesCS")]
        public MatchesCS MatchesCS { get; set; }

		[Required]
		[Display(Name = "API Id of a csgo matches")]
        public int MatchesCSAPIId { get; set; }

		[Required]
		[Display(Name = "List of stats of players")]
        public ICollection<MatchPlayerStatsCS>? PlayerStatsList { get; set; }

		[Required]
		[Display(Name = "Score of round")]
        public string? RoundsScore { get; set; }

		[Required]
		[Display(Name = "Map")]
        public string? Map { get; set; }

		[Required]
		[Display(Name = "List of Teams")] 
        public ICollection<MatchTeamsCS>? TeamsList { get; set; }

        [Display(Name = "Winner Team")]
        public Team? WinnerTeam { get; set; }

		[Required]
		[Display(Name = "API Id of the winner team")]
        public int? WinnerTeamAPIId { get; set; }

		[Required]
		[Display(Name = "Name of the winner team")]
        public string? WinnerTeamName { get; set; }
    }
}