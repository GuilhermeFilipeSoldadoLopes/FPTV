using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.UserModels
{
    public class Player
    {
        [Required]
        [Key]
		[Display(Name = "PlayerID")]
		public Guid PlayerId { get; set; }

		[Required]
		[Display(Name = "Player Name")]
		public string? Name { get; set; }

		[Required]
		[Display(Name = "Player Age")]
		public int? Age { get; set; }

		//o ultimo objeto deve ser a equipa atual
		[Required]
		[Display(Name = "Teams")]
		public IDictionary<int, Team>? Teams { get; set; }

		[Required]
		[Display(Name = "Nacionality")]
		public string? Nacionality { get; set; }

		[Required]
		[Display(Name = "Profile Picture")]
		public string? Image { get; set; }
	}
}
