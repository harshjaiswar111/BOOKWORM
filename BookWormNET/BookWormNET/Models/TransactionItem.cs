using System;
using System.Collections.Generic;

namespace BookWormNET.Models;

public partial class TransactionItem
{
    public int ItemId { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public int? ProductId { get; set; }

    public long? TransactionId { get; set; }

    public virtual Product? Product { get; set; }

    //public virtual ICollection<RoyaltyCalculation> RoyaltyCalculations { get; set; } = new List<RoyaltyCalculation>();

    public virtual Transaction? Transaction { get; set; }
}
