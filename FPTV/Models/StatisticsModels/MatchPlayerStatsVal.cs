using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.StatisticsModels
{
    /// <summary>
    /// This class will represent the statistics of the players
    /// </summary>
    public class MatchPlayerStatsVal
    {
        /// <summary>
        /// Id of the valorant match
        /// </summary>
        [Required]
        [Key]
        [Display(Name = "Id of a valorant match")]
        public Guid MatchValId { get; set; }

        /// <summary>
        /// Id of the player
        /// </summary>
        [Required]
        [ForeignKey("PlayerValId")]
        [Display(Name = "Id of a valorant player")]
        public Guid PlayerValId { get; set; }

        /// <summary>
        /// Number of kills of the player
        /// </summary>
        [Required]
        [Display(Name = "Kills")]
        public int Kills { get; set; }

        /// <summary>
        /// Number of deaths of the player
        /// </summary>
        [Required]
        [Display(Name = "Deaths")]
        public int Deaths { get; set; }

        /// <summary>
        /// Number of assists of the player
        /// </summary>
        [Required]
        [Display(Name = "Assists")]
        public int Assists { get; set; }

        /// <summary>
        /// ADR of the player
        /// </summary>
        [Required]
        [Display(Name = "ADR")]
        public float ADR { get; set; }

        /// <summary>
        /// kast of the player
        /// </summary>
        [Required]
        [Display(Name = "Kast")] 
        public float Kast { get; set; }

        /// <summary>
        /// Number of headshots of the player
        /// </summary>
        [Required]
        [Display(Name = "HeadShots")] 
        public float HeadShots { get; set; }

        /// <summary>
        /// Kill Deaths difference of the player
        /// </summary>
        [Required]
        [Display(Name = "KD_Diff")] 
        public float KD_Diff { get; set; }

        /// <summary>
        /// Name of the player
        /// </summary>
        [Required]
        [Display(Name = "Name of a player")] 
        public string PlayerName { get; set; }

    }
}