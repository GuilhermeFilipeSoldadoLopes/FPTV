using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.StatisticsModels
{
    /// <summary>
    /// This class will represent a valorant match (map)
    /// </summary>
    public class MatchVal
    {
        /// <summary>
		/// Id of the valorant match (map)
        /// </summary>
        [Required]
        [Key]
        [Display(Name = "Id of valorant match")]
        public Guid MatchValId { get; set; }

        /// <summary>
        /// Id of the valorant match (map) return by the api
        /// </summary>
        [Required]
        [Display(Name = "MatchVal API ID")]
        public int MatchValAPIID { get; set; }

        /// <summary>
        /// Id of the valorant match
        /// </summary>
        [ForeignKey("MatchesValId")]
        [Display(Name = "Id of valorant matches")]
        public Guid MatchesValId { get; set; }

        /// <summary>
        /// Id of the valorant match return by the api
        /// </summary>
        [Display(Name = "API Id of a val matches")]
        public int MatchesValAPIId { get; set; }

        /// <summary>
        /// List of statistcs of the players
        /// </summary>
        [Display(Name = "List of stats of players")] 
        public ICollection<MatchPlayerStatsVal>? PlayerStatsList { get; set; }

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
        public ICollection<MatchTeamsVal>? TeamsList { get; set; }

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
