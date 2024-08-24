using System;
using System.Collections.Generic;

namespace Oranges.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? OrderListId { get; set; }

    public int? MealId { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public string? OrderDescription { get; set; }

    public int? UserId { get; set; }

    public bool? IsPaid { get; set; }

    public decimal? PaidAmount { get; set; }

    public decimal? RemainingAmount { get; set; }

    public virtual Meal? Meal { get; set; }

    public virtual OrderList? OrderList { get; set; }

    public virtual User? User { get; set; }
}
