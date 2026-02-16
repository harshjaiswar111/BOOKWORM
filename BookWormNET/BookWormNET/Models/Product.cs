using System;
using System.Collections.Generic;

namespace BookWormNET.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public decimal? DiscountPercent { get; set; }

    public ulong? IsLibrary { get; set; }

    public int? MinRentDays { get; set; }

    public decimal ProductBaseprice { get; set; }

    public string? ProductDescriptionLong { get; set; }

    public string? ProductDescriptionShort { get; set; }

    public string? ProductImage { get; set; }

    public string ProductIsbn { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public DateOnly? ProductOffPriceExpirydate { get; set; }

    public decimal? ProductOfferprice { get; set; }

    public decimal? RentPerDay { get; set; }

    public ulong? IsRentable { get; set; }

    public decimal? RoyaltyPercent { get; set; }

    public int? AttributeId { get; set; }

    public int? ProductAuthor { get; set; }

    public int? ProductGenere { get; set; }

    public int? ProductLang { get; set; }

    public int? ProductType { get; set; }

    public int? ProductPublisher { get; set; }

    public virtual Attribute? Attribute { get; set; }

    public virtual ICollection<Beneficiary> Beneficiaries { get; set; } = new List<Beneficiary>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<MyLibrary> MyLibraries { get; set; } = new List<MyLibrary>();

    public virtual ICollection<MyShelf> MyShelves { get; set; } = new List<MyShelf>();

    public virtual ICollection<PdfBook> PdfBooks { get; set; } = new List<PdfBook>();

    public virtual ICollection<ProductAttribute> ProductAttributes { get; set; } = new List<ProductAttribute>();

    public virtual Author? ProductAuthorNavigation { get; set; }

    public virtual ICollection<ProductBeneficiary> ProductBeneficiaries { get; set; } = new List<ProductBeneficiary>();

    public virtual Genere? ProductGenereNavigation { get; set; }

    public virtual Language? ProductLangNavigation { get; set; }

    public virtual Publisher? ProductPublisherNavigation { get; set; }

    public virtual ProductTypeMaster? ProductTypeNavigation { get; set; }

    public virtual ICollection<RoyaltyCalculation> RoyaltyCalculations { get; set; } = new List<RoyaltyCalculation>();

    public virtual ICollection<TransactionItem> TransactionItems { get; set; } = new List<TransactionItem>();

    
}
        