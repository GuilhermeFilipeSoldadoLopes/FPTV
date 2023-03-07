using System.ComponentModel.DataAnnotations;
using FPTV.Models.ToReview;

namespace FPTV.Models.UserModels
{
    /// <summary>
    /// This class will represent a player
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Id of the player
        /// </summary>
        [Required]
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the player
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// Team of the player
        /// </summary>
        public Team? team { get; set; }

    }
}
