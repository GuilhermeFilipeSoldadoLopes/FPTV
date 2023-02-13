using System.ComponentModel.DataAnnotations;

namespace FPTV.Models.UserModels
{
    public class Team
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Player[]? Players { get; set; }

    }
}
