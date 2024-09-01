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
        public VotingSession VotingSession { get; set; }
        public List<int> RestaurantIds { get; set; }
    }

}
