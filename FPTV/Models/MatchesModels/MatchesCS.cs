using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FPTV.Models.EventsModels;
using FPTV.Models.StatisticsModels;
using FPTV.Models.UserModels;

namespace FPTV.Models.MatchesModels
{
    public class MatchesCS
    {
        [Required]
        [Key]
        public Guid MatchesCSId { get; set; }

        [Required]
        [Display(Name = "MatchesCS API ID")]
        public int MatchesAPIID { get; set; }

        [Display(Name = "EventCS")]
        public EventCS Event { get; set; }

        [Required]
        [Display(Name = "Event API ID")]
        public int EventAPIID { get; set; }

        
        [Display(Name = "Event Name")]
        public string? EventName { get; set; }

        
        [Display(Name = "Begin At")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? BeginAt { get; set; }


        [Display(Name = "End At")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EndAt { get; set; }

 
        [Display(Name = "Finished")]
        public bool IsFinished { get; set; }


        [Display(Name = "Time Type")]
        public TimeType TimeType { get; set; }

 
        [Display(Name = "Have Stats")]
        public bool HaveStats { get; set; }

 
        [Display(Name = "MatchesCS List")]
        public ICollection<MatchCS>? MatchesList { get; set; }


        [Display(Name = "Number Of Games")]
        public int? NumberOfGames { get; set; }


        [Display(Name = "Scores")]
        public ICollection<Score>? Scores { get; set; }

 
        [Display(Name = "Teams List")]
        public ICollection<Team>? TeamsList { get; set; }


        [NotMapped]
        [Display(Name = "Teams API Id List")]
        public ICollection<int>? TeamsAPIIDList { get; set; }

        /*[Required]
        [Display(Name = "Winner Team")]
        public Team? WinnerTeam { get; set; }*/


        [Display(Name = "Winner Team API Id")]
        public int? WinnerTeamAPIId { get; set; }

   
        [Display(Name = "Winner Team Name")]
        public string? WinnerTeamName { get; set; }

        [Display(Name = "Tier")]
        public char? Tier { get; set; }


        [Display(Name = "Live Supported")]
        public bool LiveSupported { get; set; }


        [Display(Name = "Stream List")]
        public ICollection<Stream>? StreamList { get; set; }

        [Display(Name = "League Name")]
        public string? LeagueName { get; set; }

        [Display(Name = "League API Id")]
        public int? LeagueId { get; set; }

        [Display(Name = "League Link")]
        public string? LeagueLink { get; set; }
    }
}