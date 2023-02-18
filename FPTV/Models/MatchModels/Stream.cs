using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.MatchModels
{
	public class Stream
	{
		[Required]
		[Display(Name = "StreamLink")]
		public string StreamLink { get; set; }

		[Required]
		[Display(Name = "StreamLanguage")]
		public string StreamLanguage { get; set; }
	}
}
