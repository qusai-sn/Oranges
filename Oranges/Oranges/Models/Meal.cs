using System;
using System.Collections.Generic;

namespace Oranges.Models;

public partial class Meal
{
    public int MealId { get; set; }

    public string MealName { get; set; } = null!;

    public decimal Price { get; set; }

    public int? RestaurantId { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Restaurant? Restaurant { get; set; }
}
