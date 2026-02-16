namespace BookWormNET.Models;

public partial class LibraryPackagePurchase
{
    public int PurchaseId { get; set; }
    public long TransactionId { get; set; }
    public int UserId { get; set; }
    public int PackageId { get; set; }

    public decimal PackagePrice { get; set; }
    public int AllowedBooks { get; set; }
    public decimal AvgBookPrice { get; set; }

    public DateTime PurchaseDate { get; set; }

    public virtual Transaction Transaction { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual LibraryPackage Package { get; set; } = null!;

    public virtual ICollection<LibraryPackagePurchaseItem> Items { get; set; }
        = new List<LibraryPackagePurchaseItem>();
}
