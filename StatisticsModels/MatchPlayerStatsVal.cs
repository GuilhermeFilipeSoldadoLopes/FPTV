using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.StatisticsModels
{
    public class MatchPlayerStatsVal
    {
        [Required]
        [Key]
        public Guid MatchValId { get; set; }

        [Required]
        [ForeignKey("PlayerValId")]
        public Guid PlayerValId { get; set; }

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
        [Display(Name = "ADR")]
        public float ADR { get; set; }

        [Required]
        [Display(Name = "Kast")] 
        public float Kast { get; set; }

        [Required]
        [Display(Name = "HeadShots")] 
        public float HeadShots { get; set; }

        [Required]
        [Display(Name = "KD_Diff")] 
        public float KD_Diff { get; set; }

        [Required]
        [Display(Name = "Name of a player")] 
        public string PlayerName { get; set; }

    }
}