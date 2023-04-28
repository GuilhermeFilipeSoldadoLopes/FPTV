using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.MatchesModels
{
    /// <summary>
    /// Stream class provides a generic view of a sequence of bytes.
    /// </summary>
    public class Stream
    {
        [Key]
        [Display(Name = "Stream Id")]
        public Guid StreamId { get; set; }

        [Required]
        [Display(Name = "Stream Link")]
        public string? StreamLink { get; set; }

        [Required]
        [Display(Name = "Stream Language")]
        public string? StreamLanguage { get; set; }
    }
}
