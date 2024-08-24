using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Oranges.Models
{
    public class OrderListViewModel
    {
        [Required]
        public string OrderListName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [Required]
        public DateTime user { get; set; }

        // You might also want to include the list of restaurants for the dropdown
        public List<SelectListItem> Restaurants { get; set; }
    }

}
