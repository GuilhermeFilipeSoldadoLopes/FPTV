using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.UserModels
{
    public class Player
    {
        [Required]
        [Key]
		[Display(Name = "PlayerID")]
		public Guid PlayerId { get; set; }

        [Required]
        [Display(Name = "PlayerAPIID")]
        public int PlayerAPIId { get; set; }

        [Required]
		[Display(Name = "Player Name")]
		public string? Name { get; set; }

		[Required]
		[Display(Name = "Player Age")]
		public int? Age { get; set; }

        [Required]
        [Display(Name = "Player Rating")]
        public float? Rating { get; set; }

        [Required]
		[NotMapped]
		[Display(Name = "All Teams")]
		public ICollection<Team>? Teams { get; set; }

		[Required]
		[Display(Name = "Nationality")]
		public string? Nationality { get; set; }

        [Display(Name = "Flag")]
        public string? Flag { get; set; }

        [Required]
		[Display(Name = "Profile Picture")]
		public string? Image { get; set; }

		[Required]
		[Display(Name = "Game")]
		public GameType? Game { get; set; }
	}
}
