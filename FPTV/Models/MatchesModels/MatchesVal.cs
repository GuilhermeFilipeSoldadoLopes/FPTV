using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FPTV.Models.EventsModels;
using FPTV.Models.StatisticsModels;
using FPTV.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace FPTV.Models.MatchesModels
{
    public class MatchesVal
    {
        [Required]
        [Key]
        public Guid MatchesValId { get; set; }

        [Required]
        [Display(Name = "MatchesVal API ID")]
        public int MatchesValAPIID { get; set; }

        [Display(Name = "EventVal")]
        public EventVal EventVal { get; set; }

        [Required]
        [Display(Name = "Event API ID")]
        public int EventAPIID { get; set; }

        [Required]
        [Display(Name = "Event Name")]
        public string? EventName { get; set; }

        [Required]
        [Display(Name = "Begin At")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? BeginAt { get; set; }

        [Required]
        [Display(Name = "End At")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EndAt { get; set; }

        [Required]
        [Display(Name = "Finished")]
        public bool IsFinished { get; set; }

        [Required]
        [Display(Name = "Time Type")]
        public TimeType TimeType { get; set; }

        [Required]
        [Display(Name = "Have Stats")]
        public bool HaveStats { get; set; }

        [Required]
        [Display(Name = "MatchesVal List")]
        public ICollection<MatchVal>? MatchesList { get; set; }

        [Required]
        [Display(Name = "Number Of Games")]
        public int? NumberOfGames { get; set; }

        [Required]
        [NotMapped]
        [Display(Name = "Score")]
        public IDictionary<int, int>? Score { get; set; }

        [NotMapped]
        [Display(Name = "Teams List")]
        public ICollection<Team>? TeamsList { get; set; }

        [Required]
        [NotMapped]
        [Display(Name = "Teams API Id List")]
        public ICollection<int>? TeamsAPIIDList { get; set; }

		[Display(Name = "Winner Team")]
        public Team? WinnerTeam { get; set; }

        [Required]
        [Display(Name = "Winner Team API ID")]
        public int? WinnerTeamAPIId { get; set; }

        [Required]
        [Display(Name = "Winner Team Name")]
        public string? WinnerTeamName { get; set; }

        [Required]
        [Display(Name = "Tier")]
        public char? Tier { get; set; }

        [Required]
        [Display(Name = "Live Supported")]
        public bool LiveSupported { get; set; }

        [Required]
        [Display(Name = "Stream List")]
        public ICollection<Stream>? StreamList { get; set; }

        [Display(Name = "League Name")]
        public string? LeagueName { get; set; }

        [Display(Name = "League Id")]
        public int? LeagueId { get; set; }

        [Display(Name = "League Link")]
        public string? LeagueLink { get; set; }
    }
}