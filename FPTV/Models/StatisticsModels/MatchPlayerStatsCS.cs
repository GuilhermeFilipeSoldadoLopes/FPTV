using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace FPTV.Models.StatisticsModels
{
    /// <summary>
    /// This class will represent the statistics of the players
    /// </summary>
    public class MatchPlayerStatsCS
    {
        /// <summary>
        /// Id of the cs:go match
        /// </summary>
        [Required]
        [Key]
        [Display(Name = "Id of a csgo match")]
        public int MatchCSId { get; set; }

        /// <summary>
        /// Id of the player
        /// </summary>
        [Required]
        [ForeignKey("PlayerCSId")]
        [Display(Name = "Id of a csgo player")]
        public int PlayerCSId { get; set; }

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
        /// Number of flash assists of the player
        /// </summary>
        [Required]
        [Display(Name = "FlashAssist")]
        public int FlashAssist { get; set; }

        /// <summary>
        /// ADR of the player
        /// </summary>
        [Required]
        [Display(Name = "ADR")]
        public float ADR { get; set; }

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
        [Display(Name = "KD Diff")]
        public float KD_Diff { get; set; }

        /// <summary>
        /// Name of the player
        /// </summary>
        [Required]
        [Display(Name = "Name of a player")]
        public string PlayerName { get; set; }
    }
}