using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FPTV.Models.StatisticsModels
{
    public class MatchPlayerStatsCS
    {
        [Required]
        [Key]
        [Display(Name = "Id of a csgo match")]
        public Guid MatchCSId { get; set; }

        [Required]
        [ForeignKey("PlayerCSId")]
        [Display(Name = "Id of a csgo player")]
        public Guid PlayerCSId { get; set; }

        [Required]
        [Display(Name = "Kills")]
        public int Kills { get; set; }

        [Required]
        [Display(Name = "Deaths")]
        public int Deaths { get; set; }

        [Required]
        [Display(Name = "Assists")]
        public int Assists { get; set; }

        [Required]
        [Display(Name = "FlashAssist")]
        public int FlashAssist { get; set; }

        [Required]
        [Display(Name = "ADR")]
        public float ADR { get; set; }

        [Required]
        [Display(Name = "Kast")]
        public float Kast { get; set; }

        [Required]
        [Display(Name = "HeadShots")]
        public float HeadShots { get; set; }

        [Required]
        [Display(Name = "KD Diff")]
        public float KD_Diff { get; set; }

        [Required]
        [Display(Name = "Name of a player")]
        public string PlayerName { get; set; }
    }
}