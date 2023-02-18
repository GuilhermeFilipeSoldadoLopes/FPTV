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
		[Display(Name = "EventId")]
		[ForeignKey("EventId")]
		public Guid EventId { get; set; }

		[Required]
		[Display(Name = "EventName")]
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
		[Display(Name = "TimeType")]
		public TimeType TimeType { get; set; }

		[Required]
		[Display(Name = "HaveStats")]
		public bool HaveStats { get; set; }

		/*[Required]
		[Display(Name = "MatchesList")]
		public ICollection<MatchVal> MatchesList { get; set; }*/

		[Required]
		[Display(Name = "NumberOfGames")]
		public int NumberOfGames { get; set; }

		[Required]
		[Display(Name = "TeamsIdList")]
		public List<Guid> TeamsIdList { get; set; }
		//public ICollection<Team> TeamsIdList { get; set; }

		[Display(Name = "WinnerTeamId")]
		public Guid? WinnerTeamId { get; set; }

		[Display(Name = "WinnerTeamName")]
		public string? WinnerTeamName { get; set; }

		[Required]
		[Display(Name = "Tier")]
		public char Tier { get; set; }

		[Required]
		[Display(Name = "LiveSupported")]
		public bool LiveSupported { get; set; }

		[Required]
		[Display(Name = "StreamList")]
		public ICollection<Stream> StreamList { get; set; }

		[Required]
		[Display(Name = "LeagueName")]
		public string LeagueName { get; set; }

		[Required]
		[Display(Name = "LeagueId")]
		public Guid LeagueId { get; set; }

		[Display(Name = "LeagueLink")]
		public string? LeagueLink { get; set; }
	}
}
