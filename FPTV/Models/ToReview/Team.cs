using System.ComponentModel.DataAnnotations;
using FPTV.Models.UserModels;

namespace FPTV.Models.ToReview
{
    /// <summary>
    /// This class will represent a team
    /// </summary>
    //Modelo Players tambem
    public class Team
    {
        /// <summary>
        /// Id of the team
        /// </summary>
        [Required]
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the team
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// List of players of the team
        /// </summary>
        public Player[]? Players { get; set; }

    }
}
