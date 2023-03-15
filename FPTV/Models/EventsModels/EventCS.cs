using FPTV.Models.MatchesModels;
using FPTV.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPTV.Models.EventsModels
{
    public class EventCS
    {
        [Required]
        [Key]
        public Guid EventCSID { get; set; }

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
        [Display(Name = "Event Link")]
        public string? EventLink { get; set; }

        [Required]
        [Display(Name = "Time Type")]
        public TimeType TimeType { get; set; }

        [Required]
        [Display(Name = "Finished")]
        public bool Finished { get; set; }

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

        [Display(Name = "MatchesCS")]
        public MatchesCS MatchesCS { get; set; }

        [Required]
        [Display(Name = "Matches CS API ID")]
        public int MatchesCSAPIID { get; set; }

        [Required]
        [NotMapped]
        [Display(Name = "Team list")]
        public ICollection<string>? TeamsList { get; set; }

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