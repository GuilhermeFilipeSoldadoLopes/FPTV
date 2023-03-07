using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FPTV.Models.StatisticsModels
{
    /// <summary>
    /// This class will represent a cs:go match (map)
    /// </summary>
    public class MatchCS
    {
        /// <summary>
		/// Id of the cs:go match (map)
        /// </summary>
        [Required]
        [Key]
        [Display(Name = "Id of a csgo match")]
        public int MatchCSId { get; set; }

        /// <summary>
        /// Id of the cs:go match (map) return by the api
        /// </summary>
        [Required]
        [Display(Name = "MatchCS API ID")]
        public int MatchCSAPIID { get; set; }

        /// <summary>
        /// Id of the cs:go match
        /// </summary>
        [ForeignKey("MatchesCSId")]
        [Display(Name = "Id of a csgo matches")]
        public Guid MatchesCSId { get; set; }

        /// <summary>
        /// Id of the cs:go match return by the api
        /// </summary>
        [Display(Name = "API Id of a csgo matches")]
        public int MatchesCSAPIId { get; set; }

        /// <summary>
        /// List of statistcs of the players
        /// </summary>
        [Display(Name = "List of stats of players")]
        public ICollection<MatchPlayerStatsCS>? PlayerStatsList { get; set; }

        /// <summary>
        /// Score of the match
        /// </summary>
        [Display(Name = "Score of round")]
        public string RoundsScore { get; set; }

        /// <summary>
        /// Map of the match
        /// </summary>
        [Display(Name = "Map")]
        public string? Map { get; set; }

        /// <summary>
        /// List of teams
        /// </summary>
        [Display(Name = "List of Teams")] 
        public ICollection<MatchTeamsCS>? TeamsList { get; set; }

        /// <summary>
        /// Id of the winner team
        /// </summary>
        [Display(Name = "Id of the winner team")]
        public Guid? WinnerTeamId { get; set; }

        /// <summary>
        /// Id of the winner team return by the api
        /// </summary>
        [Display(Name = "API Id of the winner team")]
        public int? WinnerTeamAPIId { get; set; }

        /// <summary>
        /// Name of the winner team
        /// </summary>
        [Display(Name = "Name of the winner team")]
        public string? WinnerTeamName { get; set; }
    }
}
