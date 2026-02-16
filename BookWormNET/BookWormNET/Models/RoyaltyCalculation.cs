using System;
using System.Collections.Generic;

namespace BookWormNET.Models;

public partial class RoyaltyCalculation
{
    public int RoycalId { get; set; }

    public decimal? RoyaltyPercent { get; set; }

    public DateOnly? RoycalTrandate { get; set; }

    public decimal? TotalAmount { get; set; }

    public decimal? TotalRoyalty { get; set; }

    public int ItemId { get; set; }

    public int ProductId { get; set; }

    //public virtual TransactionItem Item { get; set; } = null!;
    //public virtual LibraryPackagePurchaseItem Item { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<ProductBeneficiary> ProductBeneficiaries { get; set; } = new List<ProductBeneficiary>();
}
