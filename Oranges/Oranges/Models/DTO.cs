using System.ComponentModel.DataAnnotations;

namespace Oranges.Models
{
    public class mealRequestDTO
    {

        public int RestaurantId { get; set; }

        public string Name { get; set; } = null!;

        public decimal? Price { get; set; }

        public string? Description { get; set; }

        public IFormFile? Image { get; set; }

    }
    public class VotingSessionDto
    {
        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one restaurant must be selected for the voting session.")]
        public List<int> RestaurantIds { get; set; }
    }

}
