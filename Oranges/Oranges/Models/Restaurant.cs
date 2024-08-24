using System;
using System.Collections.Generic;

namespace Oranges.Models;

public partial class Restaurant
{
    public int RestaurantId { get; set; }

    public string RestaurantName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();

    public virtual ICollection<OrderList> OrderLists { get; set; } = new List<OrderList>();
}
