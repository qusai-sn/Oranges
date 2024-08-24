using System;
using System.Collections.Generic;

namespace Oranges.Models;

public partial class OrderList
{
    public int OrderListId { get; set; }

    public string OrderListName { get; set; } = null!;

    public string? Description { get; set; }

    public int? CreatedByUserId { get; set; }

    public int? RestaurantId { get; set; }

    public bool? IsLocked { get; set; }

    public decimal? DeliveryPrice { get; set; }

    public virtual User? CreatedByUser { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Restaurant? Restaurant { get; set; }
}
