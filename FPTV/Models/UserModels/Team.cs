using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.UserModels
{
    public class Team
    {
        [Required]
        [Key]
		[Display(Name = "TeamId")]
		public Guid TeamId { get; set; }

        [Required]
        [Display(Name = "Team API ID")]
        public int? TeamAPIID { get; set; }

        [Display(Name = "Teams Name")]
		public string? Name { get; set; }

		[Display(Name = "Players of the team")]
		public ICollection<Player>? Players { get; set; }

		[Display(Name = "Coach Name")]
		public string? CoachName { get; set; }
    
		[Display(Name = "World Rank")]
		public int? WorldRank { get; set; }

		[Display(Name = "Total Winnings")]
		public int? Winnings { get; set; }

		[Display(Name = "Total Losses")]
		public int? Losses { get; set; }

		[Display(Name = "Team Image")]
		public string? Image { get; set; }

		[Display(Name = "Game")]
		public GameType? Game { get; set; }
	}
}
