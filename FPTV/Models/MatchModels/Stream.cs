using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.MatchModels
{
    /// <summary>
    /// This class represent a web stream of matches
    /// </summary>
    public class Stream
	{
        /// <summary>
        /// ID of the stream
        /// </summary>
        [Key]
		public Guid Id { get; set; }

        /// <summary>
        /// Name of the stream
        /// </summary>
        [Required]
		[Display(Name = "Stream Link")]
		public string StreamLink { get; set; }

        /// <summary>
        /// Link of the stream
        /// </summary>
        [Required]
		[Display(Name = "Stream Language")]
		public string StreamLanguage { get; set; }
	}
}
