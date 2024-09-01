using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Oranges.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }
        public string Password { get; set; }
    }


    public class Restaurant
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }

    public class Meals
    {
        [Key]
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }

    public class Orders
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public Users Users { get; set; }
        public int MealId { get; set; }
        public Meals Meals { get; set; }
        public int Count { get; set; }
        public string Payment { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
    }

    public class VotingSession
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public ICollection<Vote> Votes { get; set; } // Relationship with votes
    }

    public class Vote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public Users User { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [ForeignKey("RestaurantId")]
        public Restaurant Restaurant { get; set; }

        [Required]
        public int VotingSessionId { get; set; }

        [ForeignKey("VotingSessionId")]
        public VotingSession VotingSession { get; set; }
    }



}
