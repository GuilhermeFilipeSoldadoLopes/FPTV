using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using FPTV.Models.UserModels;

namespace FPTV.Models.StatisticsModels
{
    public class MatchTeamsCS
    {
        [Required]
        [Key]
        [Display(Name = "Id of a csgo match")]
        public Guid MatchCSId { get; set; }

		[Required]
		[Display(Name = "MatchCS API ID")]
		public int MatchCSAPIID { get; set; }

        [Display(Name = "TeamCS")]
        public Team? TeamCS { get; set; }

		[Required]
		[Display(Name = "API Id of a csgo team")]
		public int TeamCSAPIId { get; set; }

		[Required]
        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string? Location { get; set; }

        [Required]
        [Display(Name = "Image")]
        public string? Image { get; set; }

        public MatchTeamsCS(Guid MatchCSId)
        {
            this.MatchCSId = MatchCSId;
        }
    }
}