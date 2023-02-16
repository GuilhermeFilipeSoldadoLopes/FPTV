using System.ComponentModel.DataAnnotations;
namespace FPTV.Models.UserModels
{
    public class Player
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
        public string? name { get; set; }
        public Team? team { get; set; }

    }
}
