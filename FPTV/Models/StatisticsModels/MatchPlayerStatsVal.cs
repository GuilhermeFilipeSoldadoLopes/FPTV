using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FPTV.Models.UserModels;

namespace FPTV.Models.StatisticsModels
{
    public class MatchPlayerStatsVal
    {
		[Required]
		[Key]
		[Display(Name = "Id of a MatchPlayerStatsVal")]
		public Guid MatchPlayerStatsValID { get; set; }

        [Required]
        [Display(Name = "MatchVal")]
        public MatchVal MatchVal { get; set; }

		[Required]
		[Display(Name = "MatchVal API ID")]
		public int MatchValAPIID { get; set; }

        [Required]
        [Display(Name = "PlayerVal")]
        public Player? PlayerVal { get; set; }

		[Required]
		[Display(Name = "API Id of a valorant player")]
		public int? PlayerValAPIId { get; set; }

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
        [Display(Name = "ADR")]
        public float? ADR { get; set; }

        [Required]
        [Display(Name = "Kast")] 
        public float? Kast { get; set; }

        [Required]
        [Display(Name = "HeadShots")] 
        public float? HeadShots { get; set; }

        [Required]
        [Display(Name = "KD_Diff")] 
        public float? KD_Diff { get; set; }

        [Required]
        [Display(Name = "Name of a player")] 
        public string? PlayerName { get; set; }
    }
}