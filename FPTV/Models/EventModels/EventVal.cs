using FPTV.Models.MatchModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace FPTV.Models.EventModels
{
    /// <summary>
    /// This class will represent a valorant event (tournament)
    /// </summary>
    public class EventVal
    {
        /// <summary>
        /// ID of the event
        /// </summary>
        [Required]
        [Key]
        public Guid EventValID { get; set; }

        /// <summary>
        /// ID of the event returned by de api
        /// </summary>
        [Required]
        [Display(Name = "Event API ID")]
        public int EventAPIID { get; set; }

        /// <summary>
        /// Name of the event
        /// </summary>
        [Required]
        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        /// <summary>
        /// Link to the event
        /// </summary>
        [Required]
        [Display(Name = "Event Link")]
        public string EventLink { get; set; }

        /// <summary>
        /// State of the event
        /// </summary>
        [Required]
        [Display(Name = "Time Type")]
        public TimeType TimeType { get; set; }

        /// <summary>
        /// If the event is finished
        /// </summary>
        [Required]
        [Display(Name = "Finished")]
        public bool Finished { get; set; }

        /// <summary>
        /// Begin date and time of the event
        /// </summary>
        [Required]
        [Display(Name = "Begin At")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime BeginAt { get; set; }

        /// <summary>
        /// End date and time of the event
        /// </summary>
        [Required]
        [Display(Name = "End At")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EndAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(Name = "Matches Val ID")]
        public Guid MatchesValID { get; set; }

        /// <summary>
        /// List of the participating teams
        /// </summary>
        [Required]
        [NotMapped]
        [Display(Name = "Team list")]
        public List<string> TeamsList { get; set; }

        /// <summary>
        /// Prize pool of the event
        /// </summary>
        [Required]
        [Display(Name = "Prize pool")]
        public string PrizePool { get; set; }

        /// <summary>
        /// Id of the event winner team
        /// </summary>
        [Display(Name = "Winner Team ID")]
        public Guid? WinnerTeamID { get; set; }

        /// <summary>
        /// Name of the event winner team
        /// </summary>
        [Display(Name = "Winner Team Name")]
        public string? WinnerTeamName { get; set; }

        /// <summary>
        /// Trie pool of the event
        /// </summary>
        [Required]
        [Display(Name = "Tier")]
        public char Tier { get; set; }
    }
}
