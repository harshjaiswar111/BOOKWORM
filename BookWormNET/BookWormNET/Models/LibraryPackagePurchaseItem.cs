namespace BookWormNET.Models;

public partial class LibraryPackagePurchaseItem
{
    public int ItemId { get; set; }
    public int PurchaseId { get; set; }
    public int ProductId { get; set; }

    public decimal RoyaltyPercent { get; set; }
    public decimal RoyaltyAmount { get; set; }

    public virtual LibraryPackagePurchase Purchase { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;

    //public virtual ICollection<RoyaltyCalculation> RoyaltyCalculations { get; set; }
    //    = new List<RoyaltyCalculation>();
}
