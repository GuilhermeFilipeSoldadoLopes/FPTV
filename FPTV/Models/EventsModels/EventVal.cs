using FPTV.Models.MatchesModels;
using FPTV.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.EventsModels
{
    public class EventVal
    {
        [Required]
        [Key]
        public Guid EventValID { get; set; }

        [Required]
        [Display(Name = "Event API ID")]
        public int EventAPIID { get; set; }

        [Required]
        [Display(Name = "Event Name")]
        public string? EventName { get; set; }

        [Required]
        [Display(Name = "LeagueName")]
        public string? LeagueName { get; set; }

        [Required]
        [Display(Name = "EventImage")]
        public string? EventImage { get; set; }

        [Required]
        [Display(Name = "Event Link")]
        public string? EventLink { get; set; }

        [Required]
        [Display(Name = "Time Type")]
        public TimeType TimeType { get; set; }

        [Required]
        [Display(Name = "Finished")]
        public bool Finished { get; set; }

        
        [Display(Name = "Begin At")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? BeginAt { get; set; }

        
        [Display(Name = "End At")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EndAt { get; set; }

        [Required]
        [Display(Name = "Matches Val API ID")]
        public int MatchesValAPIID { get; set; }

        [Required]
        [Display(Name = "Team list")]
        public ICollection<Team>? TeamsList { get; set; }

        [Required]
        [Display(Name = "Prize pool")]
        public string? PrizePool { get; set; }

        [Display(Name = "Winner Team")]
        public Team? WinnerTeam { get; set; }

        [Required]
        [Display(Name = "Winner Team API ID")]
        public int? WinnerTeamAPIID { get; set; }

        [Required]
        [Display(Name = "Winner Team Name")]
        public string? WinnerTeamName { get; set; }

        [Required]
        [Display(Name = "Tier")]
        public char? Tier { get; set; }
    }
}