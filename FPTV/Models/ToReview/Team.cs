using System.ComponentModel.DataAnnotations;
using FPTV.Models.UserModels;

namespace FPTV.Models.ToReview
{
    //Modelo Players tambem
    public class Team
    {
        [Required]
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public Player[]? Players { get; set; }

    }
}
