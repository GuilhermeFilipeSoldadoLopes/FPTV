using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FPTV.Models.UserModels;

namespace FPTV.Models.StatisticsModels
{
    public class MatchPlayerStatsCS
    {
		[Required]
		[Key]
		[Display(Name = "Id of a MatchPlayerStatsCS")]
		public Guid MatchPlayerStatsCSID { get; set; }

        [Display(Name = "MatchCS")]
        public MatchCS Match { get; set; }

		[Required]
		[Display(Name = "MatchCS API ID")]
		public int MatchAPIID { get; set; }

        [Required]
        [Display(Name = "PlayerCS")]
        public Player? Player { get; set; }

		[Required]
		[Display(Name = "API Id of a csgo player")]
		public int PlayerAPIId { get; set; }

		[Required]
        [Display(Name = "Kills")]
        public int? Kills { get; set; }

        [Required]
        [Display(Name = "Deaths")]
        public int? Deaths { get; set; }

        [Required]
        [Display(Name = "Assists")]
        public int? Assists { get; set; }

        [Required]
        [Display(Name = "FlashAssist")]
        public int? FlashAssist { get; set; }

        [Required]
        [Display(Name = "ADR")]
        public double? ADR { get; set; }

        [Required]
        [Display(Name = "HeadShots")]
        public double? HeadShots { get; set; }

        [Required]
        [Display(Name = "KD Diff")]
        public double? KD_Diff { get; set; }

        [Required]
        [Display(Name = "Name of a player")]
        public string? PlayerName { get; set; }
    }
}