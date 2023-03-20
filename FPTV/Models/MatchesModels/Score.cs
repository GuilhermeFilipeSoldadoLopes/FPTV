using System.ComponentModel.DataAnnotations;
using FPTV.Models.UserModels;

namespace FPTV.Models.MatchesModels
{
    public class Score
    {
        [Required]
        [Key]
        public Guid ScoreID { get; set; }

        [Required]
        [Display(Name = "TeamScore")]
        public int TeamScore { get; set; }

        [Required]
        [Display(Name = "Team")]
        public Team? Team { get; set; }

        [Required]
        [Display(Name = "TeamName")]
        public string TeamName { get; set; }
    }
}
