using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FPTV.Models.UserModels;

namespace FPTV.Models.MatchModels
{
	public class MatchesVal
	{
		[Required]
		[Key]
		public Guid MatchesValId { get; set; }

		[Required]
		[Display(Name = "Event Id")]
		[ForeignKey("EventId")]
		public Guid EventId { get; set; }

		[Required]
		[Display(Name = "Event Name")]
		public string EventName { get; set; }

		[Required]
		[Display(Name = "Begin At")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime BeginAt { get; set; }

		[Required]
		[Display(Name = "End At")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime EndAt { get; set; }

		[Required]
		[Display(Name = "Finished")]
		public bool IsFinished { get; set; }

		[Required]
		[Display(Name = "Time Type")]
		public TimeType TimeType { get; set; }

		[Required]
		[Display(Name = "Have Stats")]
		public bool HaveStats { get; set; }

		/*[Required]
		[Display(Name = "MatchesList")]
		public ICollection<MatchVal> MatchesList { get; set; }*/

		[Required]
		[Display(Name = "Number Of Games")]
		public int NumberOfGames { get; set; }

		[Required]
		[Display(Name = "Teams Id List")]
		public List<Guid> TeamsIdList { get; set; }

		[Display(Name = "Winner Team Id")]
		public Guid? WinnerTeamId { get; set; }

		[Display(Name = "Winner Team Name")]
		public string? WinnerTeamName { get; set; }

		[Required]
		[Display(Name = "Tier")]
		public char Tier { get; set; }

		[Required]
		[Display(Name = "Live Supported")]
		public bool LiveSupported { get; set; }

		[Required]
		[Display(Name = "Stream List")]
		public ICollection<Stream> StreamList { get; set; }

		[Required]
		[Display(Name = "League Name")]
		public string LeagueName { get; set; }

		[Required]
		[Display(Name = "League Id")]
		public Guid LeagueId { get; set; }

		[Display(Name = "League Link")]
		public string? LeagueLink { get; set; }
	}
}
