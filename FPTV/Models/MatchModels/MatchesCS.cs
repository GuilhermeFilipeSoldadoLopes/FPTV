using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FPTV.Models.StatisticsModels;
using FPTV.Models.UserModels;

namespace FPTV.Models.MatchModels
{
    /// <summary>
    /// This class will represent a cs:go match
    /// </summary>
    public class MatchesCS
	{
		/// <summary>
		/// Id of the cs:go match
		/// </summary>
		[Required]
		[Key]
		public Guid MatchesCSId { get; set; }

        /// <summary>
        /// Id of the cs:go match return by the api
        /// </summary>
        [Required]
        [Display(Name = "MatchesCS API ID")]
        public int MatchesCSAPIID { get; set; }

        /// <summary>
        /// Id of the cs:go event
        /// </summary>
        [Display(Name = "Event Id")]
		[ForeignKey("EventId")]
		public Guid EventId { get; set; }

        /// <summary>
        /// Id of the cs:go event return by the api
        /// </summary>
        [Display(Name = "Event API ID")]
        public int EventAPIID { get; set; }

		/// <summary>
		/// Name of the event
		/// </summary>
		[Display(Name = "Event Name")]
		public string EventName { get; set; }

        /// <summary>
        /// Begin date and time of the match
        /// </summary>
        [Required]
		[Display(Name = "Begin At")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime BeginAt { get; set; }

        /// <summary>
        /// End date and time of the match
        /// </summary>
        [Display(Name = "End At")]
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime EndAt { get; set; }

        /// <summary>
        /// If the match is finished
        /// </summary>
        [Required]
		[Display(Name = "Finished")]
		public bool IsFinished { get; set; }

        /// <summary>
        /// State of the event
        /// </summary>
        [Required]
		[Display(Name = "Time Type")]
		public TimeType TimeType { get; set; }

        /// <summary>
        /// If the match has statistics
        /// </summary>
        [Required]
		[Display(Name = "Have Stats")]
		public bool HaveStats { get; set; }

		/// <summary>
		/// List of matches (maps played)
		/// </summary>
		[Required]
		[Display(Name = "MatchesCS List")]
		public ICollection<MatchCS> MatchesList { get; set; }

        /// <summary>
        /// Number of matches (maps played)
        /// </summary>
        [Required]
		[Display(Name = "Number Of Games")]
		public int NumberOfGames { get; set; }

		/// <summary>
		/// List of id teams
		/// </summary>
		[NotMapped]
		[Display(Name = "Teams Id List")]
		public List<Guid> TeamsIdList { get; set; }

        /// <summary>
        /// List of id teams return by the api
        /// </summary>
        [Required]
        [NotMapped]
        [Display(Name = "Teams API Id List")]
        public List<int> TeamsAPIIDList { get; set; }

		/// <summary>
		/// Id of the winner team
		/// </summary>
        [Display(Name = "Winner Team  Id")]
		public Guid? WinnerTeamId { get; set; }

        /// <summary>
        /// Id of the winner team return by the api
        /// </summary>
        [Display(Name = "Winner Team API Id")]
        public int? WinnerTeamAPIId { get; set; }

        /// <summary>
        /// Name of the winner team
        /// </summary>
        [Display(Name = "Winner Team Name")]
		public string? WinnerTeamName { get; set; }

		/// <summary>
		/// Tier of the match
		/// </summary>
		[Required]
		[Display(Name = "Tier")]
		public char Tier { get; set; }

		/// <summary>
		/// If the match is live supported
		/// </summary>
		[Required]
		[Display(Name = "Live Supported")]
		public bool LiveSupported { get; set; }

		/// <summary>
		/// List of streams
		/// </summary>
		[Display(Name = "Stream List")]
		public ICollection<Stream>? StreamList { get; set; }

		/// <summary>
		/// Name of the League
		/// </summary>
		[Display(Name = "League Name")]
		public string LeagueName { get; set; }

        /// <summary>
        /// Id of the League
        /// </summary>
        [Display(Name = "League Id")]
		public int LeagueId { get; set; }

        /// <summary>
        /// Link of the League
        /// </summary>
        [Display(Name = "League Link")]
		public string? LeagueLink { get; set; }
	}
}
