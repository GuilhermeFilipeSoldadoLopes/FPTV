using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.MatchModels
{
	public class Stream
	{
		[Key]
		[Display(Name = "Stream Id")]
		public Guid StreamId { get; set; }

		[Required]
		[Display(Name = "Stream Link")]
		public string? StreamLink { get; set; }

		[Required]
		[Display(Name = "Stream Language")]
		public string? StreamLanguage { get; set; }
	}
}
