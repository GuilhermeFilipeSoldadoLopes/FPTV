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

        [Required]
		[Display(Name = "Teams Name")]
		public string? Name { get; set; }

		[Display(Name = "Players of the team")]
		public ICollection<Player>? Players { get; set; }

		[Required]
		[Display(Name = "Couch Name")]
		public string? CouchName { get; set; }

		[Required]
		[Display(Name = "World Rank")]
		public int? WorldRank { get; set; }

		[Required]
		[Display(Name = "Total Winnings")]
		public int? Winnings { get; set; }

		[Required]
		[Display(Name = "Total Losses")]
		public int? Losses { get; set; }

		[Required]
		[Display(Name = "Team Image")]
		public string? Image { get; set; }

		[Required]
		[Display(Name = "Game")]
		public GameType? game { get; set; }
	}
}
