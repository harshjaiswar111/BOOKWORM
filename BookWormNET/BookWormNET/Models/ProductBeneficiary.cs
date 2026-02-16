using System;
using System.Collections.Generic;

namespace BookWormNET.Models;

public partial class ProductBeneficiary
{
    public int ProdbenId { get; set; }

    public decimal? RoyaltyReceived { get; set; }

    public int BeneficiaryId { get; set; }

    public int ProductId { get; set; }

    public int RoycalId { get; set; }

    public virtual Beneficiary Beneficiary { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual RoyaltyCalculation Roycal { get; set; } = null!;
}
