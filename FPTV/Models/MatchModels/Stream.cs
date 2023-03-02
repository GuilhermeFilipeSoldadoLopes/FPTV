using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.MatchModels
{
	public class Stream
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		[Display(Name = "Stream Link")]
		public string StreamLink { get; set; }

		[Required]
		[Display(Name = "Stream Language")]
		public string StreamLanguage { get; set; }
	}
}
