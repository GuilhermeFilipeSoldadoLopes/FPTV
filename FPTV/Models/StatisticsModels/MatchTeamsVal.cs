using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FPTV.Models.UserModels;

namespace FPTV.Models.StatisticsModels
{
    public class MatchTeamsVal
    {
        [Required]
        [Key]
        [Display(Name = "Id of a valorant match")]
        public Guid MatchValId { get; set; }

		[Required]
		[Display(Name = "MatchVal API ID")]
		public int MatchValAPIID { get; set; }

        [Display(Name = "TeamVal")]
        public Team? TeamVal { get; set; }

		[Required]
		[Display(Name = "API Id of a valorant team")]
		public int? TeamValAPIId { get; set; }

		[Required]
        [Display(Name = "Name")] 
        public string? Name { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string? Location { get; set; }

        [Required]
        [Display(Name = "Image")] 
        public string? Image { get; set; }
    }
}